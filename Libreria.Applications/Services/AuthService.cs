using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;
using Libreria.Applications.Interfaces.Services;
using Libreria.Domain.Entidades;
using Microsoft.VisualBasic.CompilerServices;

namespace Libreria.Applications.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IUsuarioRepository _usuarioRepository;
    public AuthService(ITokenService tokenService, IUsuarioRepository usuarioRepository)
    {
        _tokenService = tokenService;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<AuthDto.LoginResponseDto?> LoginAsync(AuthDto.LoginDto dto)
    {
        var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(dto.Username);

        if (usuario == null) return null;

        if (!_tokenService.VerificaContrasena(dto.Password, usuario.Contrasena))
        {
            return null;
        }

        var token = _tokenService.GenerateToken(usuario);
        var expiration = DateTime.UtcNow.AddHours(1);

        return new AuthDto.LoginResponseDto
        {
            Token = token,
            Expiration = expiration,
            Username = usuario.NombreUsuario,
            Role = usuario.Rol
        };
    }

    public async Task<bool> RegisterAsync(AuthDto.RegisterDto dto)
    {
        var usuario = new Usuario
        {
            NombreUsuario = dto.Username,
            Contrasena = _tokenService.EncriptarContrasena(dto.Password),
            Rol = dto.Role,
            FechaCreacion = DateTime.UtcNow
        };

        await _usuarioRepository.AddAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidarUsuarioAsync(string username, string password)
    {
        var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(username);
        
        if (usuario == null) return false;

        return _tokenService.VerificaContrasena(password, usuario.Contrasena);
    }
}