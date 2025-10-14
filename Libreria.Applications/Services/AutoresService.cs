using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;
using Libreria.Applications.Interfaces.Services;
using Libreria.Domain.Entidades;

namespace Libreria.Applications.Services;

public class AutoresService : IAutoresService
{
    private readonly IAutorRepository _autorRepository;

    public AutoresService(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }
    
    public async Task<List<AutorDto>> GetTodosAutoresAsync()
    {
        var autores = await _autorRepository.GetAllAsync();
        return autores.Select(a => new AutorDto()
        {
            Id = a.Id,
            Nombre = a.Nombre,
            Nacionalidad = a.Nacionalidad
        }).ToList();
    }

    public async Task<AutorDto?> GetAutorPorIdAsync(int id)
    {
        var autor = await _autorRepository.GetByIdAsync(id);
        if (autor == null) return null;
        return new AutorDto()
        {
            Id = autor.Id,
            Nombre = autor.Nombre,
            Nacionalidad = autor.Nacionalidad
        };
    }

    public async Task<AutorDto> CrearAutorAsync(CrearAutorDto dto)
    {
        var autorExistente = await _autorRepository.GetByNombreAsync(dto.Nombre);
        if (autorExistente != null)
        {
            throw new InvalidOperationException($"Ya existe un autor con el nombre {dto.Nombre}");
        }
        
        var autor = new Autores()
        {
            Nombre = dto.Nombre,
            Nacionalidad = dto.Nacionalidad
        };

        await _autorRepository.AddAsync(autor);
        await _autorRepository.SaveChangesAsync();

        return await GetAutorPorIdAsync(autor.Id)
               ?? throw new InvalidOperationException("Error al crear el autor");
    }
}