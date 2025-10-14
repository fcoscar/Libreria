using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;
using Libreria.Applications.Interfaces.Services;

namespace Libreria.Applications.Services;

public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestamoRepository;

    public PrestamoService(IPrestamoRepository prestamoRepository)
    {
        _prestamoRepository = prestamoRepository;
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
}