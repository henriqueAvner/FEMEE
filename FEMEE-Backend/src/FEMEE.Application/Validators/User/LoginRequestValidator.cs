using FluentValidation;
using FEMEE.Application.DTOs.Auth;

namespace FEMEE.Application.Validators.User
{
    /// <summary>
    /// Validador para LoginRequest.
    /// Valida dados de login.
    /// </summary>
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public LoginRequestValidator()
        {
            // ===== VALIDAR EMAIL =====
            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Email é obrigatório")
                .EmailAddress()
                    .WithMessage("Email inválido");

            // ===== VALIDAR SENHA =====
            RuleFor(x => x.Senha)
                .NotEmpty()
                    .WithMessage("Senha é obrigatória");
        }
    }
}
