using FluentValidation;
using FEMEE.Application.DTOs.Conquista;

namespace FEMEE.Application.Validators.Conquista
{
    /// <summary>
    /// Validador para UpdateConquistaDto.
    /// </summary>
    public class UpdateConquistaDtoValidator : AbstractValidator<UpdateConquistaDto>
    {
        public UpdateConquistaDtoValidator()
        {
            RuleFor(x => x.TimeId)
                .GreaterThan(0)
                    .WithMessage("TimeId deve ser maior que zero")
                .When(x => x.TimeId.HasValue);

            RuleFor(x => x.Titulo)
                .MaximumLength(256)
                    .WithMessage("Título deve ter no máximo 256 caracteres")
                .When(x => x.Titulo != null);

            RuleFor(x => x.Descricao)
                .MaximumLength(500)
                    .WithMessage("Descrição deve ter no máximo 500 caracteres")
                .When(x => x.Descricao != null);

            RuleFor(x => x.Posicao)
                .GreaterThan(0)
                    .WithMessage("Posição deve ser maior que zero")
                .LessThanOrEqualTo(100)
                    .WithMessage("Posição deve ser no máximo 100")
                .When(x => x.Posicao.HasValue);

            RuleFor(x => x.DataConquista)
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                    .WithMessage("Data da conquista não pode ser no futuro")
                .When(x => x.DataConquista.HasValue);

            RuleFor(x => x.IconeTitulo)
                .MaximumLength(256)
                    .WithMessage("URL do ícone deve ter no máximo 256 caracteres")
                .When(x => x.IconeTitulo != null);
        }
    }
}
