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
}