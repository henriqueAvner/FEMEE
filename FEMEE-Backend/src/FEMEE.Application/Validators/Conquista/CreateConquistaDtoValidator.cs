using FluentValidation;
using FEMEE.Application.DTOs.Conquista;

namespace FEMEE.Application.Validators.Conquista
{
    /// <summary>
    /// Validador para CreateConquistaDto.
    /// </summary>
    public class CreateConquistaDtoValidator : AbstractValidator<CreateConquistaDto>
    {
        public CreateConquistaDtoValidator()
        {
            RuleFor(x => x.TimeId)
                .GreaterThan(0)
                    .WithMessage("TimeId é obrigatório e deve ser maior que zero");

            RuleFor(x => x.Titulo)
                .NotEmpty()
                    .WithMessage("Título é obrigatório")
                .MaximumLength(256)
                    .WithMessage("Título deve ter no máximo 256 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(500)
                    .WithMessage("Descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.Posicao)
                .GreaterThan(0)
                    .WithMessage("Posição deve ser maior que zero")
                .LessThanOrEqualTo(100)
                    .WithMessage("Posição deve ser no máximo 100");

            RuleFor(x => x.DataConquista)
                .NotEmpty()
                    .WithMessage("Data da conquista é obrigatória")
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                    .WithMessage("Data da conquista não pode ser no futuro");

            RuleFor(x => x.IconeTitulo)
                .MaximumLength(256)
                    .WithMessage("URL do ícone deve ter no máximo 256 caracteres");
        }
    }
}
