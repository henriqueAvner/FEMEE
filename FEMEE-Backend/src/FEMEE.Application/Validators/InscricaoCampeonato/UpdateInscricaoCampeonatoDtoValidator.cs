using FluentValidation;
using FEMEE.Application.DTOs.InscricaoCampeonato;

namespace FEMEE.Application.Validators.InscricaoCampeonato
{
    /// <summary>
    /// Validador para UpdateInscricaoCampeonatoDto.
    /// </summary>
    public class UpdateInscricaoCampeonatoDtoValidator : AbstractValidator<UpdateInscricaoCampeonatoDto>
    {
        public UpdateInscricaoCampeonatoDtoValidator()
        {
            RuleFor(x => x.StatusInscricao)
                .IsInEnum()
                    .WithMessage("Status de inscrição inválido")
                .When(x => x.StatusInscricao.HasValue);

            RuleFor(x => x.TelefoneContato)
                .MaximumLength(20)
                    .WithMessage("Telefone deve ter no máximo 20 caracteres")
                .When(x => x.TelefoneContato != null);

            RuleFor(x => x.EmailContato)
                .EmailAddress()
                    .WithMessage("Email de contato inválido")
                .MaximumLength(100)
                    .WithMessage("Email deve ter no máximo 100 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.EmailContato));

            RuleFor(x => x.NomeCapitao)
                .MaximumLength(256)
                    .WithMessage("Nome do capitão deve ter no máximo 256 caracteres")
                .When(x => x.NomeCapitao != null);

            RuleFor(x => x.NomeTime)
                .MaximumLength(256)
                    .WithMessage("Nome do time deve ter no máximo 256 caracteres")
                .When(x => x.NomeTime != null);

            RuleFor(x => x.Observacoes)
                .MaximumLength(500)
                    .WithMessage("Observações devem ter no máximo 500 caracteres")
                .When(x => x.Observacoes != null);
        }
    }
}
