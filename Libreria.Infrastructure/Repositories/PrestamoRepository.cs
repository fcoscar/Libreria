using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;
using Libreria.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Infrastructure.Repositories;

public class PrestamoRepository : Repository<Prestamos>, IPrestamoRepository
{
    public PrestamoRepository(MainDbContext context) : base(context)
    {
    }

    public async Task<List<GetPrestamoDto>> GetPrestamosNoDevueltosAsync()
    { 
        return await _dbSet.Where(p => p.FechaDevolucion == null)
            .Select(p => new GetPrestamoDto()
            {
                AutorId = p.Libro.AutorId,
                Nombre= p.Libro.Autor.Nombre,
                Titulo = p.Libro.Titulo,
                LibroId = p.LibroId,
            })
            .ToListAsync();
    }

    public async Task<GetPrestamoDto?> GetPrestamoByLibroIdAsync(int id)
    {
        return await _dbSet.Where(p => p.LibroId == id)
            .Select(p => new GetPrestamoDto()
            {
                AutorId = p.Libro.AutorId,
                Nombre = p.Libro.Autor.Nombre,
                Titulo = p.Libro.Titulo,
                LibroId = p.LibroId,
            })
            .FirstOrDefaultAsync();
    }
}