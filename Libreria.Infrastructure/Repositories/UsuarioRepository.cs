using Libreria.Applications.Interfaces.Repositories;
using Libreria.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(MainDbContext context) : base(context)
    {
    }


    public async Task<Usuario?> GetByNombreUsuarioAsync(string username)
    {
       return await _dbSet.FirstOrDefaultAsync(u => u.NombreUsuario == username);
    }
}