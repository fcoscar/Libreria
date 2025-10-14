using Libreria.Applications.DTOs;

namespace Libreria.Applications.Interfaces.Services;

public interface IPrestamoService
{
    Task<List<GetPrestamoDto>> GetPrestamosNoDevueltosAsync();
    Task<bool> EliminarPrestamoAsync(int id);
    Task<GetPrestamoDto?> GetPrestamoByIdAsync(int id);
    Task<bool> ActualizarFechaDevPrestamoAsync(int id, ActualizarFechaDevDto dto);
}