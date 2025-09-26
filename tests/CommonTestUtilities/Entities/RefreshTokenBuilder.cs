using Bogus;
using StandardProject.Domain.Entities;
using StandardProject.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;
public class RefreshTokenBuilder
{
    public static RefreshToken Build(User user)
    {
        return new Faker<RefreshToken>()
            .RuleFor(r => r.Id, _ => 1)
            .RuleFor(r => r.CreatedOn, f => f.Date.Soon(days: StandardProjectRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS))
            .RuleFor(r => r.Value, f => f.Lorem.Word())
            .RuleFor(r => r.UserId, _ => user.Id)
            .RuleFor(r => r.User, _ => user);
    }
}
