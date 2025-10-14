namespace Libreria.Applications.DTOs;

public class BusquedaAvanzadaDto
{
    public string? Genero { get; set; }
    public int? AnoPublicacion { get; set; }
    public int? AutorId { get; set; }
    public bool? Popurales { get; set; }
    public int? Top { get; set; } = 10;
    public int? Meses { get; set; } = 3;
}