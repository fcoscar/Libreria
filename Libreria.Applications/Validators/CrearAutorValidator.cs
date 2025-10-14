using FluentValidation;
using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;

namespace Libreria.Applications.Validators;

public class CrearAutorValidator : AbstractValidator<CrearAutorDto>
{
    private readonly IAutorRepository _autorRepository;

    public CrearAutorValidator(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;

        RuleFor(c => c.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo nombre es requerido")
            .MustAsync(async (nombre, ct) =>
            {
                nombre = nombre.Trim().ToLower();
                var autorExistente = await _autorRepository.GetByNombreAsync(nombre);
                return autorExistente == null;
            })
            .WithMessage("Ya existe un autor con ese nombre");

        RuleFor(c => c.Nacionalidad)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo nacionalidad es requerido");
    }
}