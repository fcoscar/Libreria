using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;
using Libreria.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Infrastructure.Repositories;

public class LibroRespository : Repository<Libros>, ILibroRepository
{
    public LibroRespository(MainDbContext context) : base(context)
    {
    }

    public async Task<List<GetLibroDto>> GetLibrosAntesDe2000Async()
    {
        return await _dbSet
            .Where(l => l.AnoPublicacion < 2000)
            .OrderBy(l => l.AnoPublicacion)
            .Select(l => new GetLibroDto()
            {
                Id = l.Id,
                Titulo = l.Titulo,
                AnoPublicacion = l.AnoPublicacion
            })
            .ToListAsync();
    }

    public async Task<Libros?> GetLibroByNombreAsync(string titulo)
    {
        return await _dbSet.Where(l => l.Titulo == titulo).FirstOrDefaultAsync();
    }
}