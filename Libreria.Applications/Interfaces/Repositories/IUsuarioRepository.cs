using Libreria.Domain.Entidades;

namespace Libreria.Applications.Interfaces.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByNombreUsuarioAsync(string username);
}