using Libreria.Domain.Entidades;

namespace Libreria.Applications.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(Usuario usuario);
    string EncriptarContrasena(string password);
    bool VerificaContrasena(string password, string hash);
}