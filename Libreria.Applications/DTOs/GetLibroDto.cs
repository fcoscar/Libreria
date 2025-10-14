namespace Libreria.Applications.DTOs;

public class GetLibroDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int AnoPublicacion { get; set; }
}