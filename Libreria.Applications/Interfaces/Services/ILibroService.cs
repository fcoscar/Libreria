using Libreria.Applications.DTOs;

namespace Libreria.Applications.Interfaces.Services;

public interface ILibroService
{
    Task<List<GetLibroDto>> GetLibrosAntesDe2000Async();
    Task<GetLibroDto> CrearLibroAsync(PostLibroDto dto);
}
