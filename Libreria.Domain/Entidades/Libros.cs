namespace Libreria.Domain.Entidades;

public class Libros
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int AutorId { get; set; }
    public int AnoPublicacion { get; set; }
    public string? Genero { get; set; }
    public Autores Autor { get; set; } = null!;
    public ICollection<Prestamos> Prestamos { get; set; } = new List<Prestamos>();
}