using FluentValidation;
using FEMEE.Application.DTOs.Auth;

namespace FEMEE.Application.Validators.Auth
{
    /// <summary>
    /// Validador para RegisterRequest.
    /// Valida dados de registro de novo usuário.
    /// </summary>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public RegisterRequestValidator()
        {
            // ===== VALIDAR NOME =====
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome é obrigatório")
                .MinimumLength(3)
                    .WithMessage("Nome deve ter pelo menos 3 caracteres")
                .MaximumLength(256)
                    .WithMessage("Nome deve ter no máximo 256 caracteres");

            // ===== VALIDAR EMAIL =====
            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Email é obrigatório")
                .EmailAddress()
                    .WithMessage("Email inválido")
                .MaximumLength(256)
                    .WithMessage("Email deve ter no máximo 256 caracteres");

            // ===== VALIDAR SENHA =====
            RuleFor(x => x.Senha)
                .NotEmpty()
                    .WithMessage("Senha é obrigatória")
                .MinimumLength(8)
                    .WithMessage("Senha deve ter pelo menos 8 caracteres")
                .MaximumLength(100)
                    .WithMessage("Senha deve ter no máximo 100 caracteres")
                .Matches("[A-Z]")
                    .WithMessage("Senha deve conter pelo menos uma letra maiúscula")
                .Matches("[a-z]")
                    .WithMessage("Senha deve conter pelo menos uma letra minúscula")
                .Matches("[0-9]")
                    .WithMessage("Senha deve conter pelo menos um número");

            // ===== VALIDAR CONFIRMAÇÃO DE SENHA =====
            RuleFor(x => x.ConfirmacaoSenha)
                .NotEmpty()
                    .WithMessage("Confirmação de senha é obrigatória")
                .Equal(x => x.Senha)
                    .WithMessage("Senhas não correspondem");

            // ===== VALIDAR TELEFONE (opcional) =====
            RuleFor(x => x.Telefone)
                .MaximumLength(20)
                    .WithMessage("Telefone deve ter no máximo 20 caracteres")
                .Matches(@"^\+?[\d\s\-\(\)]+$")
                    .When(x => !string.IsNullOrWhiteSpace(x.Telefone))
                    .WithMessage("Telefone inválido");
        }
    }
}
