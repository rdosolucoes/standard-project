using StandardProject.Domain.Security.Cryptography;
using StandardProject.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new BCryptNet();
}
