using GreenFDFBookapi.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepairOpsApi.BusinessLogic.Interfaces;
using RepairOpsApi.Helpers;

namespace RepairOpsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    
    private readonly ILogger<UserController> _logger;
    private readonly IUserLogic _userLogic;
    private readonly JwtHandler _jwtHandler;

    public UserController(ILogger<UserController> logger, IUserLogic userLogic, JwtHandler jwtHandler)
    {
        _logger = logger;
        _userLogic = userLogic; 
        _jwtHandler = jwtHandler;
    }

    
    [HttpPost("[action]")]
    public IActionResult RegisterUser(string username, string password, string confirmPassword )
    {
        try
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                return BadRequest("Username, password, and confirmation password cannot be empty.");
            }

            if (password != confirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }
            

            _userLogic.RegisterUser(username, password);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering the user.");
            return StatusCode(500, "An error occurred while registering the user.");
        }
    }

    [HttpPost("[action]")]
    public IActionResult LoginUser([FromBody] LoginRequest loginRequest)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.username) || string.IsNullOrWhiteSpace(loginRequest.password))
        {
            return BadRequest("Username and password are required.");
        }

        try
        {
            var user = _userLogic.LoginUser(loginRequest.username, loginRequest.password);

            if (user != null)
            {
                if (_jwtHandler == null)
                {
                    _logger.LogError("JwtHandler is null.");
                    return StatusCode(500, "Internal server error.");
                }

                var token = _jwtHandler.GenerateToken(user);
                var userResponse = new
                {
                    userId = user.id,
                    username = user.username,
                    token = token
                };

                UserSession.getInstance(user);
                return Ok(userResponse);
            }
            else
            {
                return Unauthorized("Invalid username or password.");
            }
        }catch (Exception ex)
        {
            return Unauthorized("invalid username or password");
        }
    }

    [HttpGet("[action]")]
    public IActionResult GetAllUsers()
    {
        try
        {
            //henter liste fra userlogic
            var users = _userLogic.GetAllUsers();
            //returnere listen som JSON
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all users.");
            return StatusCode(500, "An error occurred while getting all users.");
        }
    }
}