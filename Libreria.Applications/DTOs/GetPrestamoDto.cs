namespace Libreria.Applications.DTOs;

public class GetPrestamoDto
{
    public int Id { get; set; }
    public int AutorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int LibroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
}