using FluentValidation;
using FEMEE.Application.DTOs.Time;

namespace FEMEE.Application.Validators.Time
{
    /// <summary>
    /// Validador para CreateTimeDto.
    /// Valida dados de criação de novo time.
    /// </summary>
    public class CreateTimeDtoValidator : AbstractValidator<CreateTimeDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public CreateTimeDtoValidator()
        {
            // ===== VALIDAR NOME =====
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome do time é obrigatório")
                .Length(3, 256)
                    .WithMessage("Nome deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR SLUG =====
            RuleFor(x => x.Slug)
                .NotEmpty()
                    .WithMessage("Slug é obrigatório")
                .Matches(@"^[a-z0-9-]+$")
                    .WithMessage("Slug deve conter apenas letras minúsculas, números e hífens")
                .Length(3, 100)
                    .WithMessage("Slug deve ter entre 3 e 100 caracteres");

            // ===== VALIDAR DATA FUNDAÇÃO =====
            RuleFor(x => x.DataFundacao)
                .NotEmpty()
                    .WithMessage("Data de fundação é obrigatória")
                .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Data de fundação não pode ser no futuro")
                .GreaterThan(DateTime.Now.AddYears(-50))
                    .WithMessage("Data de fundação não pode ser há mais de 50 anos");

            // ===== VALIDAR DESCRIÇÃO =====
            RuleFor(x => x.Descricao)
                .MaximumLength(1000)
                    .WithMessage("Descrição não pode ter mais de 1000 caracteres");

            // ===== VALIDAR LOGO URL =====
            RuleFor(x => x.LogoUrl)
                .NotEmpty()
                    .WithMessage("URL do logo é obrigatória")
                .Must(IsValidUrl)
                    .WithMessage("URL do logo inválida");
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
