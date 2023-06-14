using Microsoft.AspNetCore.Mvc;
using MyBookList.Authentication;
using Microsoft.AspNetCore.Authorization;
using MyBookList.Database;
using MyBookList.Models;

namespace CryptoAnalyzerWebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userService;
    private readonly AuthenticationService _authenticationService;

    public AuthController()
    {
        string connectionString = "mongodb://localhost:27017/";
        string databaseName = "MyBookList";
        _userService = new UserRepository(connectionString, databaseName);
        _authenticationService = new AuthenticationService();
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthRequest model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Authenticate the user and get the User object
        User user = _userService.AuthenticateUser(model.Login, model.Password).Result;

        if (user != null)
        {
            string token = _authenticationService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // Clear session data
        HttpContext.Session.Clear();

        // Return a success response
        return Ok();
    }
    
    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] AuthRequest model)
    {
        // Validate the registration model
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if the username or email already exists
        if (_userService.IsUsernameTaken(model.Login).Result)
        {
            ModelState.AddModelError("Login", "Username is already taken");
            return BadRequest(ModelState);
        }

        // Create a new user object
        var user = new User
        {
            Username = model.Login,
            Password = model.Password,
        };

        // Store the user in the data store
        _userService.AddUser(user);

        // Generate a JWT token for the registered user
        string token = _authenticationService.GenerateJwtToken(user);

        // Return the token in the response
        return Ok(new { Token = token });
    }
    
}
