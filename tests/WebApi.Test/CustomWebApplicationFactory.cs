using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StandardProject.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private StandardProject.Domain.Entities.User _user = default!;
    private StandardProject.Domain.Entities.RefreshToken _refreshToken = default!;
    private string _password = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<StandardProjectDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                //var blobStorage = new BlobStorageServiceBuilder().Build();
                //services.AddScoped(option => blobStorage);

                services.AddDbContext<StandardProjectDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<StandardProjectDbContext>();

                dbContext.Database.EnsureDeleted();

                StartDatabase(dbContext);
            });
    }

    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetName() => _user.Name;
    public string GetRefreshToken() => _refreshToken.Value;
    public Guid GetUserIdentifier() => _user.UserIdentifier;


    private void StartDatabase(StandardProjectDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        _refreshToken = RefreshTokenBuilder.Build(_user);

        dbContext.Users.Add(_user);

        dbContext.RefreshTokens.Add(_refreshToken);

        dbContext.SaveChanges();
    }
}
