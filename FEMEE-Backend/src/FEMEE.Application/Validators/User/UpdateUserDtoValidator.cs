using FluentValidation;
using FEMEE.Application.DTOs.User;

namespace FEMEE.Application.Validators.User
{
    /// <summary>
    /// Validador para UpdateUserDto.
    /// Valida dados de atualização de usuário.
    /// </summary>
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public UpdateUserDtoValidator()
        {
            // ===== VALIDAR NOME =====
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome é obrigatório")
                .Length(3, 256)
                    .WithMessage("Nome deve ter entre 3 e 256 caracteres")
                .Matches(@"^[a-zA-ZÀ-ÿ\s]+$")
                    .WithMessage("Nome deve conter apenas letras e espaços");

            // ===== VALIDAR TELEFONE =====
            RuleFor(x => x.Telefone)
                .NotEmpty()
                    .WithMessage("Telefone é obrigatório")
                .Matches(@"^\(\d{2}\)\s?\d{4,5}-\d{4}$")
                    .WithMessage("Telefone inválido. Formato esperado: (XX) XXXXX-XXXX ou (XX) XXXX-XXXX");
        }
    }
}
