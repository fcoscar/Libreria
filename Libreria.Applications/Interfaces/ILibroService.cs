using Libreria.Applications.DTOs;

namespace Libreria.Applications.Interfaces;

public interface ILibroService
{
    Task<List<GetLibroDto>> GetLibrosAntesDe2000Async();
    Task<PostLibroDto> CrearLibroAsync(PostLibroDto dto);
}
