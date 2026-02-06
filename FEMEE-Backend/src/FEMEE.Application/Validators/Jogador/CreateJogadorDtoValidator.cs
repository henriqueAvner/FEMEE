using FluentValidation;
using FEMEE.Application.DTOs.Jogador;

namespace FEMEE.Application.Validators.Jogador
{
    /// <summary>
    /// Validador para CreateJogadorDto.
    /// Valida dados de criação de novo jogador.
    /// </summary>
    public class CreateJogadorDtoValidator : AbstractValidator<CreateJogadorDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public CreateJogadorDtoValidator()
        {
            // ===== VALIDAR USER ID =====
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                    .WithMessage("Usuário é obrigatório");

            // ===== VALIDAR NICKNAME =====
            RuleFor(x => x.NickName)
                .NotEmpty()
                    .WithMessage("Nickname é obrigatório")
                .Length(3, 50)
                    .WithMessage("Nickname deve ter entre 3 e 50 caracteres")
                .Matches(@"^[a-zA-Z0-9_-]+$")
                    .WithMessage("Nickname deve conter apenas letras, números, hífens e underscores");

            // ===== VALIDAR FUNÇÃO =====
            RuleFor(x => x.Funcao)
                .IsInEnum()
                    .WithMessage("Função inválida");
        }
    }
}
