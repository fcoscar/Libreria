using Libreria.Applications.DTOs;

namespace Libreria.Applications.Interfaces.Services;

public interface IAutoresService
{
    Task<List<AutorDto>> GetTodosAutoresAsync();
    Task<AutorDto?> GetAutorPorIdAsync(int id);
    Task<AutorDto> CrearAutorAsync(CrearAutorDto dto);
}