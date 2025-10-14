using FluentValidation;
using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;

namespace Libreria.Applications.Validators;

public class CrearUsuarioValidator : AbstractValidator<AuthDto.RegisterDto>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CrearUsuarioValidator(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;

        RuleFor(c => c.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo contraseña es requerido");

        RuleFor(c => c.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo usuario es requerido")
            .MustAsync(async (username, ct) =>
            {
                username = username.Trim().ToLower();
                var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(username);
                return usuario == null;
            })
            .WithMessage("El nombre de usuario ya existe.");

        RuleFor(x => x.Role)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(r => new[] { "Admin", "User" }
                .Contains(r, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Rol inválido. Use 'Admin' o 'User'.");
    }
}