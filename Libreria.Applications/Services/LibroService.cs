using FluentValidation;
using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces;
using Libreria.Applications.Interfaces.Repositories;
using Libreria.Applications.Interfaces.Services;
using Libreria.Domain.Entidades;
using Microsoft.VisualBasic.CompilerServices;

namespace Libreria.Applications.Services;

public class LibroService : ILibroService
{
    private readonly ILibroRepository _libroRepository;
    private readonly IAutorRepository _autorRepository;
    private readonly IValidator<PostLibroDto> _validator;
    public LibroService(ILibroRepository libroRepository, IAutorRepository autorRepository, IValidator<PostLibroDto> validator)
    {
        _libroRepository = libroRepository;
        _autorRepository = autorRepository;
        _validator = validator;
    }

    public async Task<List<GetLibroDto>> GetTodosLibrosAsync()
    {
        var libros = await _libroRepository.GetAllAsync();
        return libros.Select(l => new GetLibroDto
        {
            Id = l.Id,
            Titulo = l.Titulo,
            AnoPublicacion = l.AnoPublicacion
        }).ToList();
    }

    public async Task<GetLibroDto?> GetLibroPorIdAsync(int id)
    {
        var libro = await _libroRepository.GetByIdAsync(id);
        if (libro == null) return null;
        return new GetLibroDto()
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            AnoPublicacion = libro.AnoPublicacion
        };
    }

    public async Task<List<GetLibroDto>> GetLibrosAntesDe2000Async()
    {
        var libros = await _libroRepository.GetLibrosAntesDe2000Async();
        return libros;
    }

    public async Task<GetLibroDto> CrearLibroAsync(PostLibroDto dto)
    {
        var result  = await _validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            throw new InvalidOperationException(errors);
        }
        var libro = new Libros
        {
            Titulo = dto.Titulo,
            AutorId = dto.AutorId,
            AnoPublicacion = dto.AnoPublicacion,
            Genero = dto.Genero
        };

        await _libroRepository.AddAsync(libro);
        await _libroRepository.SaveChangesAsync();
        var newDto = new GetLibroDto
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            AnoPublicacion = libro.AnoPublicacion
        };
        return newDto;
    }

    public async Task<List<GetLibroDto>> BusquedaAvanzadaAsync(BusquedaAvanzadaDto filtros)
    {
        return await _libroRepository.BusquedaAvanzadaAsync(filtros);
    }

    public async Task<bool> EliminarLibroAsync(int id)
    {
        var libro = await _libroRepository.GetByIdAsync(id);
        if (libro == null) return false;
        await _libroRepository.DeleteAsync(libro);
        await _libroRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActualizarFLibroAsync(int id, PutLibroDto dto)
    {
        var libro = await _libroRepository.GetByIdAsync(id);
        if (libro == null) return false;

        if (dto.anoPublicacion.HasValue && dto.anoPublicacion.Value <= 0)
        {
            throw new InvalidOperationException(
                "El ano de publicacion del libro no puede ser menor o igual a 0");
        }
        
        libro.Titulo = string.IsNullOrEmpty(dto.Titulo) ? libro.Titulo : dto.Titulo;;
        libro.AnoPublicacion = dto.anoPublicacion ?? libro.AnoPublicacion;
        libro.Genero = string.IsNullOrEmpty(dto.Genero) ? libro.Genero : dto.Genero;;
        await _libroRepository.UpdateAsync(libro);
        await _libroRepository.SaveChangesAsync();
        return true;
    }
}