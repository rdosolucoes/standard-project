using Microsoft.EntityFrameworkCore;
using StandardProject.Domain.Entities;
using StandardProject.Domain.Security.Tokens;
using StandardProject.Domain.Services.LoggedUser;
using StandardProject.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StandardProject.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly StandardProjectDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(StandardProjectDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> User()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}
