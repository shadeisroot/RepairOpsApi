using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RepairOpsApi.BusinessLogic.Interfaces;
using RepairOpsApi.Controllers;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;
using RepairOpsApi.Helpers;

namespace RepairOpsTest;

[TestFixture]
public class UserTest
{

    private Mock<ILogger<UserController>> _mockLogger;
    private Mock<IUserLogic> _mockUserLogic;
    private UserController _controller;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<UserController>>();
        _mockUserLogic = new Mock<IUserLogic>();
        _controller = new UserController(_mockLogger.Object, _mockUserLogic.Object);
    }

    [Test]
    public void RegisterUser_ValidInput_ReturnsOk()
    {
        // Arrange
        var username = "testuser";
        var password = "password";
        var confirmPassword = "password";

        // Act
        var result = _controller.RegisterUser(username, password, confirmPassword);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public void RegisterUser_PasswordsDoNotMatch_ReturnsBadRequest()
    {
        // Arrange
        var username = "testuser";
        var password = "password";
        var confirmPassword = "differentpassword";

        // Act
        var result = _controller.RegisterUser(username, password, confirmPassword);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public void LoginUser_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var loginRequest = new LoginRequest { username = "testuser", password = "password" };
        _mockUserLogic.Setup(x => x.LoginUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new User { id = 1, username = "testuser" });

        // Act
        var result = _controller.LoginUser(loginRequest);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void LoginUser_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest { username = "testuser", password = "wrongpassword" };
        _mockUserLogic.Setup(x => x.LoginUser(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

        // Act
        var result = _controller.LoginUser(loginRequest);

        // Assert
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }
}