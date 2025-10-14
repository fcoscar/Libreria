using FluentValidation;
using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Libreria.Applications.Validators;

public class CrearPrestamoValidator : AbstractValidator<PostPrestamoDto>
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly ILibroRepository _libroRepository;

    public CrearPrestamoValidator(IPrestamoRepository prestamoRepository, ILibroRepository libroRepository)
    {
        _prestamoRepository = prestamoRepository;
        _libroRepository = libroRepository;

        RuleFor(c => c.FechaPrestamo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo fecha de préstamo es requerido");

        RuleFor(c => c.LibroId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo id libro es requerido")
            .MustAsync(async (libroId, ct) =>
            {
                var libroExiste = await _libroRepository.GetByIdAsync(libroId);
                return libroExiste != null;
            })
            .WithMessage("El libro no existe")
            .MustAsync(async (libroId, ct) =>
            {
                var prestamoExistente = await _prestamoRepository.GetPrestamoByLibroIdAsync(libroId);
                return prestamoExistente == null;
            })
            .WithMessage("Ya existe un préstamo con ese libro");
    }
}