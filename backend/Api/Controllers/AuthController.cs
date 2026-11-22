using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register(RegisterDto dto)
    {
        var result = await _service.RegisterAsync(dto);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }

        var gebruiker = await _service.GetByEmailAsync(dto.Email);
        return StatusCode(StatusCodes.Status201Created, gebruiker);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponseDto>> Login(LoginDto dto)
    {
        var gelukt = await _service.LoginAsync(dto);
        if (!gelukt)
        {
            return Unauthorized();
        }

        var gebruiker = await _service.GetByEmailAsync(dto.Email);
        return Ok(gebruiker);
    }
}
