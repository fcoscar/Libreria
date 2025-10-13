namespace Libreria.Domain.Entidades;

public class Prestamos
{
    public int Id { get; set; }
    public int LibroId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public Libros Libro { get; set; }
}