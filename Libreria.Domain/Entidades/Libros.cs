using System.ComponentModel.DataAnnotations.Schema;

namespace Libreria.Domain.Entidades;

public class Libros
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int AutorId { get; set; }
    public int AnoPublicacion { get; set; }
    public string? Genero { get; set; }
    public virtual Autores Autor { get; set; } = null!;
    public virtual ICollection<Prestamos> Prestamos { get; set; } = new List<Prestamos>();
}