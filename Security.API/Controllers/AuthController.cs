using Microsoft.AspNetCore.Mvc;
using Security.Application.DTOs;
using Security.Application.Services;

namespace Security.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        if (result.IsFailure)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(Register), new { id = result.Value.Id }, result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        if (result.IsFailure)
        {
            if (result.Errors.Contains("Invalid credentials"))
                return Unauthorized(result.Errors);

            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost("validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<bool> ValidateToken([FromBody] string token)
    {
        var result = _authService.ValidateToken(token);
        return Ok(result.Value);
    }
}