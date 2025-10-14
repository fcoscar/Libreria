using FluentValidation;
using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Repositories;

namespace Libreria.Applications.Validators;

public class CrearLibroValidator : AbstractValidator<PostLibroDto>
{
    private readonly ILibroRepository _libroRepository;
    private readonly IAutorRepository _autorRepository;

    public CrearLibroValidator(ILibroRepository libroRepository, IAutorRepository autorRepository)
    {
        _libroRepository = libroRepository;
        _autorRepository = autorRepository;

        RuleFor(x => x.AnoPublicacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo año de publicación es requerido");

        RuleFor(x => x.Titulo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo título es requerido")
            .MustAsync(async (titulo, ct) =>
            {
                titulo = titulo.Trim().ToLower();
                var libro = await _libroRepository.GetLibroByNombreAsync(titulo);
                return libro == null;
            })
            .WithMessage("Ya existe un libro con ese título");

        RuleFor(x => x.AutorId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El campo autor es requerido")
            .MustAsync(async (autorId, ct) =>
            {
                var autor = await _autorRepository.GetByIdAsync(autorId);
                return autor != null;
            })
            .WithMessage("El autor no existe");
        
        RuleFor(x => new { x.Titulo, x.AutorId })
            .MustAsync(async (m, ct) => !await _libroRepository.ExisteLibroPorTituloAutorAsync(m.Titulo, m.AutorId))
            .WithMessage("Ya existe un libro con ese título para este autor");
    }
}