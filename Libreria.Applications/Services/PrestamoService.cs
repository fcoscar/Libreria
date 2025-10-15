using FluentValidation;
using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;
using Libreria.Applications.Interfaces.Services;
using Libreria.Domain.Entidades;

namespace Libreria.Applications.Services;

public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly ILibroRepository _libroRepository;
    private readonly IValidator<PostPrestamoDto> _validator;
    public PrestamoService(IPrestamoRepository prestamoRepository, ILibroRepository libroRepository, IValidator<PostPrestamoDto> validator)
    {
        _prestamoRepository = prestamoRepository;
        _libroRepository = libroRepository;
        _validator = validator;
    }

    public async Task<List<GetPrestamoDto>> GetPrestamosNoDevueltosAsync()
    {
        return await _prestamoRepository.GetPrestamosNoDevueltosAsync();
    }

    public async Task<bool> EliminarPrestamoAsync(int id)
    {
        var prestamo = await _prestamoRepository.GetByIdAsync(id);
        if (prestamo == null) return false;
        await _prestamoRepository.DeleteAsync(prestamo);
        await _prestamoRepository.SaveChangesAsync();
        return true;
    }

    public async Task<GetPrestamoDto?> GetPrestamoByIdAsync(int id)
    {
        var prestamo = await _prestamoRepository.GetByIdAsync(id);
        if (prestamo == null) return null;
        return new GetPrestamoDto()
        {
            Id = prestamo.Id,
            AutorId = prestamo.Libro.AutorId,
            LibroId = prestamo.LibroId,
            Nombre = prestamo.Libro.Autor.Nombre,
            Titulo = prestamo.Libro.Titulo,
        };
    }

    public async Task<bool> ActualizarFechaDevPrestamoAsync(int id, ActualizarFechaDevDto dto)
    {
        var prestamo = await _prestamoRepository.GetByIdAsync(id);
        if (prestamo == null) return false;
        if (dto.FechaDev < prestamo.FechaPrestamo)
        {
            throw new InvalidOperationException(
                "La fecha de devolución no puede ser anterior a la fecha de préstamo");
        }
        prestamo.FechaDevolucion = dto.FechaDev;
        await _prestamoRepository.UpdateAsync(prestamo);
        await _prestamoRepository.SaveChangesAsync();
        return true;
    }

    public async Task<GetPrestamoDto> CrearPrestamoAsync(PostPrestamoDto dto)
    {
        
        var result  = await _validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            var errors = string.Join("; ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            throw new InvalidOperationException(errors);
        }
        
        var libroExiste = await _libroRepository.GetByIdAsync(dto.LibroId);
        if (libroExiste == null)
        {
            throw new InvalidOperationException($"El libro con ID {dto.LibroId} no existe");
        }

        var prestamoExistente = await _prestamoRepository.GetPrestamoByLibroIdAsync(dto.LibroId);
        if (prestamoExistente != null)
        {
            throw new InvalidOperationException($"El libro con ID {dto.LibroId} ya esta prestado");
        }
        
        var prestamo = new Prestamos
        {
            LibroId = dto.LibroId,
            FechaPrestamo = dto.FechaPrestamo,
            FechaDevolucion = null
        };

        await _prestamoRepository.AddAsync(prestamo);
        await _prestamoRepository.SaveChangesAsync();


        return await GetPrestamoByIdAsync(prestamo.Id)
               ?? throw new InvalidOperationException("Error al crear el préstamo");
    }

    public async Task<List<GetPrestamoDto>> GetTodosPrestamosAsync()
    {
        var prestamos = await _prestamoRepository.GetAllAsync();
        return prestamos.Select(p => new GetPrestamoDto
        {
            Id = p.Id,
            AutorId = p.Libro.AutorId,
            LibroId = p.LibroId,
            Nombre = p.Libro.Autor.Nombre,
            Titulo = p.Libro.Titulo,
        }).ToList();
    }
}