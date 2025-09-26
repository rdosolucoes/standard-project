using Azure.Messaging.ServiceBus;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StandardProject.Domain.Enums;
using StandardProject.Domain.Repositories;
using StandardProject.Domain.Repositories.Token;
using StandardProject.Domain.Repositories.User;
using StandardProject.Domain.Security.Cryptography;
using StandardProject.Domain.Security.Tokens;
using StandardProject.Domain.Services.LoggedUser;
using StandardProject.Domain.Services.ServiceBus;
using StandardProject.Infrastructure.DataAccess;
using StandardProject.Infrastructure.DataAccess.Repositories;
using StandardProject.Infrastructure.Extensions;
using StandardProject.Infrastructure.Security.Cryptography;
using StandardProject.Infrastructure.Security.Tokens.Access.Generator;
using StandardProject.Infrastructure.Security.Tokens.Access.Validator;
using StandardProject.Infrastructure.Security.Tokens.Refresh;
using StandardProject.Infrastructure.Services.LoggedUser;
using StandardProject.Infrastructure.Services.ServiceBus;
using System.Reflection;

namespace StandardProject.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncrpter(services);
        AddRepositories(services);
        AddLoggedUser(services);
        AddTokens(services, configuration);
        //AddOpenAI(services, configuration);
        //AddAzureStorage(services, configuration);
        AddQueue(services, configuration);

        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseType.MySql)
        {
            AddDbContext_MySqlServer(services, configuration);
            AddFluentMigrator_MySql(services, configuration);
        }
        else
        {
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);
        }
    }

    private static void AddDbContext_MySqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

        services.AddDbContext<StandardProjectDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();

        services.AddDbContext<StandardProjectDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddMySql5()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("StandardProject.Infrastructure")).For.All();
        });
    }

    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddSqlServer()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("StandardProject.Infrastructure")).For.All();
        });
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddPasswordEncrpter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }

    //private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
    //{
    //    services.AddScoped<IGenerateRecipeAI, ChatGptService>();

    //    var apiKey = configuration.GetValue<string>("Settings:OpenAI:ApiKey");

    //    services.AddScoped(c => new ChatClient(StandardProjectRuleConstants.CHAT_MODEL, apiKey));
    //}

    //private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    //{
    //    var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

    //    if(connectionString.NotEmpty())
    //    {
    //        services.AddScoped<IBlobStorageService>(c => new AzureStorageService(new BlobServiceClient(connectionString)));
    //    }
    //}

    private static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount")!;

        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });

        var deleteQueue = new DeleteUserQueue(client.CreateSender("deleteuser"));

        var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("deleteuser", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1
        }));

        services.AddSingleton(deleteUserProcessor);

        services.AddScoped<IDeleteUserQueue>(options => deleteQueue);
    }
}
