using Libreria.Domain.Entidades;

namespace Libreria.Applications.Interfaces.Repositories;

public interface IAutorRepository : IRepository<Autores>
{
    Task<Autores?> GetByNombreAsync(string nombre);
}