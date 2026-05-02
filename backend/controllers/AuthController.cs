
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;


// Identity-based auth controller

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IApplicationUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(IApplicationUserService userService, UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await _userService.LoginUser(dto.Username, dto.Password);
        }
        catch (Exception)
        {
            return Unauthorized("Invalid username or password.");
        }

        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return Ok(new { message = "Logged in, auth cookie set.", role });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            const string defaultRole = "User";
            await _userService.RegisterUser(dto.Username, dto.Password, defaultRole);
            return Ok(new { message = "User registered", role = defaultRole });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await _userService.LogoutUser();
        return Ok(new { message = "Logged out" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult> Me()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return Ok(new { username = user.UserName, role });
    }

  
}