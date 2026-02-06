using FluentValidation;
using FEMEE.Application.DTOs.Campeonato;

namespace FEMEE.Application.Validators.Campeonato
{
    /// <summary>
    /// Validador para UpdateCampeonatoDto.
    /// Valida dados de atualização de campeonato.
    /// </summary>
    public class UpdateCampeonatoDtoValidator : AbstractValidator<UpdateCampeonatoDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public UpdateCampeonatoDtoValidator()
        {
            // ===== VALIDAR TÍTULO =====
            RuleFor(x => x.Titulo)
                .NotEmpty()
                    .WithMessage("Título é obrigatório")
                .Length(3, 256)
                    .WithMessage("Título deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR DESCRIÇÃO =====
            RuleFor(x => x.Descricao)
                .NotEmpty()
                    .WithMessage("Descrição é obrigatória")
                .MaximumLength(2000)
                    .WithMessage("Descrição não pode ter mais de 2000 caracteres");

            // ===== VALIDAR LOCAL =====
            RuleFor(x => x.Local)
                .NotEmpty()
                    .WithMessage("Local é obrigatório")
                .Length(3, 256)
                    .WithMessage("Local deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR REGULAMENTO URL =====
            RuleFor(x => x.RegulamentoUrl)
                .Must(IsValidUrl)
                    .WithMessage("URL do regulamento inválida")
                .When(x => !string.IsNullOrEmpty(x.RegulamentoUrl));
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
