using GreenFDFBookapi.Entities;
using Microsoft.AspNetCore.Mvc;
using RepairOpsApi.BusinessLogic.Interfaces;

namespace RepairOpsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    
    private readonly ILogger<UserController> _logger;
    private readonly IUserLogic _userLogic;

    public UserController(ILogger<UserController> logger, IUserLogic userLogic)
    {
        _logger = logger;
        _userLogic = userLogic; 
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
    public IActionResult LoginUser(string username, string password)
    {
        
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest("Username and password are required.");
        }
        
        try
        {
            var user = _userLogic.LoginUser(username, password);
        
            // Hvis login er succesfuldt, returner brugeroplysningerne
            if (user != null)
            {
                // Opret et objekt med de nødvendige brugeroplysninger
                var userResponse = new
                {
                    userId = user.id, // Skift dette til det relevante felt i dit user-objekt
                    username = user.username // Dette skal også være relevant
                };

                UserSession.getInstance(user);
                return Ok(userResponse); // Returnér brugeroplysningerne som en JSON
            }
            else
            {
                return Unauthorized("Invalid username or password.");
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while logging in the user.");
            return StatusCode(500, "An error occurred while logging in the user.");
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