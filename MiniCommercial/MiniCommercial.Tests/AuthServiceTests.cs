using Xunit;

namespace MiniCommercial.Tests;

public class AuthServiceTests
{
    [Fact]
    public void PasswordHashing_ShouldBeSecureAndVerifiable()
    {
        string password = "SecretPassword123";
        string hash = BCrypt.Net.BCrypt.HashPassword(password);

        Assert.NotEqual(password, hash);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
    }
}