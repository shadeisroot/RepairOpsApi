using Microsoft.AspNetCore.Identity.Data;
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
    private Mock<IUserRepository> _mockRepository;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
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
    
    [Test]
    public async Task GetUsers_Returns_AllUsers()
    {
        //Arrange
        var users = new List<User> 
        {
            new User { id = 1, username = "Teknikker1" },
            new User { id = 2, username = "Teknikker2" },
            new User { id = 3, username = "Teknikker3" } 
        };
        _mockUserLogic.Setup(logic => logic.GetAllUsers()).Returns(users);
        
        //Act
        var result = _controller.GetAllUsers();
        
        //Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult); // Resultatet skal være OK
        var returnedUsers = okResult.Value as IEnumerable<User>;
        Assert.IsNotNull(returnedUsers); // Sikrer, at det returnerede ikke er null
        Assert.AreEqual(3, returnedUsers.Count()); // Antallet af brugere skal være 3
        Assert.AreEqual("Teknikker1", returnedUsers.First().username); // Den første bruger er 'Teknikker1'
        Assert.AreEqual("Teknikker3", returnedUsers.Last().username); // Den sidste bruger er 'Teknikker3'
    }
}