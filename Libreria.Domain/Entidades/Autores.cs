using System.ComponentModel.DataAnnotations.Schema;

namespace Libreria.Domain.Entidades;

public class Autores
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
    public ICollection<Libros> Libros { get; set; } = new List<Libros>();
}