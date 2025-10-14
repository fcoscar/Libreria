using Libreria.Applications.Interfaces.Repositories;
using Libreria.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Infrastructure.Repositories;

public class AutorRepository : Repository<Autores>, IAutorRepository

{
    public AutorRepository(MainDbContext context) : base(context)
    {
    }

    public async Task<Autores?> GetByNombreAsync(string nombre)
    {
        return await _dbSet.Where(a => a.Nombre == nombre).FirstOrDefaultAsync();
    }
}