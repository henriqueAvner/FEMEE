using FluentValidation;
using FEMEE.Application.DTOs.Jogador;

namespace FEMEE.Application.Validators.Jogador
{
    /// <summary>
    /// Validador para UpdateJogadorDto.
    /// Valida dados de atualização de jogador.
    /// </summary>
    public class UpdateJogadorDtoValidator : AbstractValidator<UpdateJogadorDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public UpdateJogadorDtoValidator()
        {
            // ===== VALIDAR NICKNAME (opcional) =====
            RuleFor(x => x.NickName)
                .Length(3, 50)
                    .WithMessage("Nickname deve ter entre 3 e 50 caracteres")
                .Matches(@"^[a-zA-Z0-9_-]+$")
                    .WithMessage("Nickname deve conter apenas letras, números, hífens e underscores")
                .When(x => !string.IsNullOrEmpty(x.NickName));

            // ===== VALIDAR FUNÇÃO (opcional) =====
            RuleFor(x => x.Funcao)
                .IsInEnum()
                    .WithMessage("Função inválida")
                .When(x => x.Funcao.HasValue);

            // ===== VALIDAR STATUS (opcional) =====
            RuleFor(x => x.Status)
                .IsInEnum()
                    .WithMessage("Status inválido")
                .When(x => x.Status.HasValue);

            // ===== VALIDAR TIME ID (opcional) =====
            RuleFor(x => x.TimeId)
                .GreaterThan(0)
                    .WithMessage("ID do time inválido")
                .When(x => x.TimeId.HasValue);
        }
    }
}
