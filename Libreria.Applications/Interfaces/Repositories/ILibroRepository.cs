using Libreria.Applications.DTOs;
using Libreria.Domain.Entidades;

namespace Libreria.Applications.Interfaces.Repositories;

public interface ILibroRepository : IRepository<Libros>
{
    Task<List<GetLibroDto>> GetLibrosAntesDe2000Async();
    Task<Libros?> GetLibroByNombreAsync(string titulo);
}