using FluentValidation;
using FEMEE.Application.DTOs.User;

namespace FEMEE.Application.Validators.User
{
    /// <summary>
    /// Validador para CreateUserDto.
    /// Valida dados de criação de novo usuário.
    /// </summary>
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public CreateUserDtoValidator()
        {
            // ===== VALIDAR NOME =====
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome é obrigatório")
                .Length(3, 256)
                    .WithMessage("Nome deve ter entre 3 e 256 caracteres")
                .Matches(@"^[a-zA-ZÀ-ÿ\s]+$")
                    .WithMessage("Nome deve conter apenas letras e espaços");

            // ===== VALIDAR EMAIL =====
            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Email é obrigatório")
                .EmailAddress()
                    .WithMessage("Email inválido")
                .MaximumLength(256)
                    .WithMessage("Email não pode ter mais de 256 caracteres");

            // ===== VALIDAR SENHA =====
            RuleFor(x => x.Senha)
                .NotEmpty()
                    .WithMessage("Senha é obrigatória")
                .MinimumLength(8)
                    .WithMessage("Senha deve ter no mínimo 8 caracteres")
                .MaximumLength(256)
                    .WithMessage("Senha não pode ter mais de 256 caracteres")
                .Matches(@"[A-Z]")
                    .WithMessage("Senha deve conter pelo menos uma letra maiúscula")
                .Matches(@"[a-z]")
                    .WithMessage("Senha deve conter pelo menos uma letra minúscula")
                .Matches(@"[0-9]")
                    .WithMessage("Senha deve conter pelo menos um número")
                .Matches(@"[!@#$%^&*()_+\-=\[\]{};':"",.<>?/\\|`~]")
                    .WithMessage("Senha deve conter pelo menos um caractere especial (!@#$%^&*)");

            // ===== VALIDAR TELEFONE =====
            RuleFor(x => x.Telefone)
                .NotEmpty()
                    .WithMessage("Telefone é obrigatório")
                .Matches(@"^\(\d{2}\)\s?\d{4,5}-\d{4}$")
                    .WithMessage("Telefone inválido. Formato esperado: (XX) XXXXX-XXXX ou (XX) XXXX-XXXX");

            // ===== VALIDAR TIPO USUÁRIO =====
            RuleFor(x => x.TipoUsuario)
                .IsInEnum()
                    .WithMessage("Tipo de usuário inválido");
        }
    }
}
