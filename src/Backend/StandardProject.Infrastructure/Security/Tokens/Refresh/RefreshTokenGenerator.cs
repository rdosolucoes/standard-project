using StandardProject.Domain.Security.Tokens;

namespace StandardProject.Infrastructure.Security.Tokens.Refresh;
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}