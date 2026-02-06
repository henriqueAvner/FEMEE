using FluentValidation;
using FEMEE.Application.DTOs.Time;

namespace FEMEE.Application.Validators.Time
{
    /// <summary>
    /// Validador para UpdateTimeDto.
    /// Valida dados de atualização de time.
    /// </summary>
    public class UpdateTimeDtoValidator : AbstractValidator<UpdateTimeDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public UpdateTimeDtoValidator()
        {
            // ===== VALIDAR NOME =====
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome do time é obrigatório")
                .Length(3, 256)
                    .WithMessage("Nome deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR DESCRIÇÃO =====
            RuleFor(x => x.Descricao)
                .MaximumLength(1000)
                    .WithMessage("Descrição não pode ter mais de 1000 caracteres");

            // ===== VALIDAR LOGO URL =====
            RuleFor(x => x.LogoUrl)
                .Must(IsValidUrl)
                    .WithMessage("URL do logo inválida")
                .When(x => !string.IsNullOrEmpty(x.LogoUrl));
        }

        /// <summary>
        /// Valida se uma string é uma URL válida.
        /// </summary>
        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
