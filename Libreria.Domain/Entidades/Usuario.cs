using System.ComponentModel.DataAnnotations.Schema;

namespace Libreria.Domain.Entidades;

public class Usuario
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty; // "Admin" o "User"
    public DateTime FechaCreacion { get; set; }
}