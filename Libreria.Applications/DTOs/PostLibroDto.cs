namespace Libreria.Applications.DTOs;

public class PostLibroDto
{
    public string Titulo { get; set; } = string.Empty;
    public int AutorId { get; set; }
    public int AñoPublicacion { get; set; }
    public string? Genero { get; set; }
}