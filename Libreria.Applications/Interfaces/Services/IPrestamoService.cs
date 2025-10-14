using Libreria.Applications.DTOs;

namespace Libreria.Applications.Interfaces.Services;

public interface IPrestamoService
{
    Task<List<GetPrestamoDto>> GetPrestamosNoDevueltosAsync();
}