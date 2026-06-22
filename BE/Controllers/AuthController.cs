namespace BE.Controllers;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BE.Models;
using BE.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService iAuthService)
    {
        _authService = iAuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        bool registerSuccess = await _authService.Register(dto);
        if (!registerSuccess)
        {
            return BadRequest(new { message = "Registration failed." });
        }
        return Ok(new { message = "Registration successful." });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO dto)
    {
        AuthResponseDTO loginResponse = await _authService.Login(dto);
        return Ok(loginResponse);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshDTO dto)
    {
        var token = await _authService.Refresh(dto);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }
        return Ok(new { token });
    }
}
