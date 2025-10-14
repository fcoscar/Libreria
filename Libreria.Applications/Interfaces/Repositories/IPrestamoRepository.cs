using Libreria.Applications.DTOs;
using Libreria.Domain.Entidades;

namespace Libreria.Applications.Interfaces.Repositories;

public interface IPrestamoRepository : IRepository<Prestamos> 
{
    Task<List<GetPrestamoDto>> GetPrestamosNoDevueltosAsync();
    Task<GetPrestamoDto?> GetPrestamoByLibroIdAsync(int id);
}