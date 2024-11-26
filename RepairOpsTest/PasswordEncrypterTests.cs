using RepairOpsApi.Helpers;
using RepairOpsApi.Helpers.Interfaces;

namespace RepairOpsTest;

[TestFixture]
public class PasswordEncrypterTests
{
    private IPasswordEncrypter _passwordEncrypter;

    [SetUp]
    public void Setup()
    {
        _passwordEncrypter = new PasswordEncrypter();
    }

    [Test]
    public void EncryptPassword_ValidPassword_ReturnsHashAndSalt()
    {
        // Arrange
        var password = "password";

        // Act
        var (hash, salt) = _passwordEncrypter.EncryptPassword(password);

        // Assert
        Assert.IsNotNull(hash);
        Assert.IsNotNull(salt);
    }

    [Test]
    public void EncryptPasswordWithUsersSalt_ValidPasswordAndSalt_ReturnsHash()
    {
        // Arrange
        var password = "password";
        var salt = "somesalt";

        // Act
        var hash = _passwordEncrypter.EncryptPasswordWithUsersSalt(password, salt);

        // Assert
        Assert.IsNotNull(hash);
    }

    [Test]
    public void EncryptPassword_EmptyPassword_ThrowsArgumentException()
    {
        // Arrange
        var password = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _passwordEncrypter.EncryptPassword(password));
    }

    [Test]
    public void EncryptPasswordWithUsersSalt_EmptyPassword_ThrowsArgumentException()
    {
        // Arrange
        var password = "";
        var salt = "somesalt";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _passwordEncrypter.EncryptPasswordWithUsersSalt(password, salt));
    }
}