using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthDto.LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthDto.LoginResponseDto>>> Login([FromBody] AuthDto.LoginDto dto)
    {
        try
        {
            var response = await _authService.LoginAsync(dto);
            
            if (response == null)
            {
                _logger.LogWarning("Intento de login fallido para usuario: {Username}", dto.Username);
                return Unauthorized(ApiResponse<object>.ErrorResponse(
                    "Credenciales inválidas"));
            }

            _logger.LogInformation("Login exitoso para usuario: {Username}", dto.Username);
            return Ok(ApiResponse<AuthDto.LoginResponseDto>.SuccessResponse(
                response, "Login exitoso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el proceso de login");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] AuthDto.RegisterDto dto)
    {
        try
        {
            var resultado = await _authService.RegisterAsync(dto);
            
            if (!resultado)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "El usuario ya existe"));
            }

            _logger.LogInformation("Usuario registrado exitosamente: {Username}", dto.Username);
            return StatusCode(201, ApiResponse<object>.SuccessResponse(
                new { username = dto.Username, role = dto.Role }, 
                "Usuario registrado exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el registro");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
}