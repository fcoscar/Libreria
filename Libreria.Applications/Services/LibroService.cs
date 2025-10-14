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
    public LibroService(ILibroRepository libroRepository, IAutorRepository autorRepository)
    {
        _libroRepository = libroRepository;
        _autorRepository = autorRepository;
    }

    public async Task<List<GetLibroDto>> GetLibrosAntesDe2000Async()
    {
        var libros = await _libroRepository.GetLibrosAntesDe2000Async();
        return libros;
    }

    public async Task<GetLibroDto> CrearLibroAsync(PostLibroDto dto)
    {
        try
        {
            var autorExiste = await _autorRepository.GetByIdAsync(dto.AutorId);
            if (autorExiste == null)
            {
                throw new InvalidOperationException($"El autor con ID {dto.AutorId} no existe");
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
            var newLibro = await _libroRepository.GetByIdAsync(libro.Id);
            var newDto = new GetLibroDto
            {
                Id = newLibro.Id,
                Titulo = newLibro.Titulo,
                AñoPublicacion = newLibro.AnoPublicacion
            };
        
            return newDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}