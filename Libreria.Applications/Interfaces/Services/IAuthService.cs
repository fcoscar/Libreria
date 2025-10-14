using Libreria.Applications.DTOs;

namespace Libreria.Applications.Interfaces.Services;

public interface IAuthService
{
    Task<AuthDto.LoginResponseDto?> LoginAsync(AuthDto.LoginDto dto);
    Task<bool> RegisterAsync(AuthDto.RegisterDto dto);
    Task<bool> ValidarUsuarioAsync(string username, string password);
}