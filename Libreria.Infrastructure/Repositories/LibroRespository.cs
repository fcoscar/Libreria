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

    public async Task<List<GetLibroDto>> BusquedaAvanzadaAsync(BusquedaAvanzadaDto filtros)
    {
        var query = _dbSet.Include(l => l.Autor).AsQueryable();

        if (!string.IsNullOrEmpty(filtros.Genero))
            query = query.Where(l => l.Genero == filtros.Genero);

        if (filtros.AnoPublicacion.HasValue)
            query = query.Where(l => l.AnoPublicacion == filtros.AnoPublicacion.Value);

        if (filtros.AutorId.HasValue)
            query = query.Where(l => l.AutorId == filtros.AutorId);

        if (filtros.Popurales ?? false)
        {
            
            query = query.Where(l => l.Prestamos.Count > 0);

            if (filtros.Top.HasValue)
            {
                query = query.Take(filtros.Top.Value);
            }
            
            if(filtros.Meses.HasValue)
            {
                var fechaLimite = DateTime.Now.AddMonths(-filtros.Meses.Value);
                query = query.Where(l => l.Prestamos.Any(p => p.FechaPrestamo >= fechaLimite));
            }
            
            var top10 = query.Take(10);
            return await top10
                .OrderByDescending(l => l.Prestamos.Count)
                .Select(l => new GetLibroDto()
                {
                    Titulo = l.Titulo,
                    AnoPublicacion = l.AnoPublicacion,
                    Id = l.Id
                })
                .ToListAsync();
        }

        var items = await query
            .OrderBy(l => l.Id)
            .Select(l => new GetLibroDto()
            {
                Titulo = l.Titulo,
                AnoPublicacion = l.AnoPublicacion,
                Id = l.Id
            })
            .ToListAsync();
            
        return items;
    }

    public async Task<bool> ExisteLibroPorTituloAutorAsync(string titulo, int autor)
    {
        var libro = await _dbSet.Where(l => l.Titulo == titulo && l.AutorId == autor).FirstOrDefaultAsync();
        return libro != null;
    }
}