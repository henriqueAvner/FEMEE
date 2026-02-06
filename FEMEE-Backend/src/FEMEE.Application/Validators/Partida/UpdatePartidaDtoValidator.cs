using FluentValidation;
using FEMEE.Application.DTOs.Partida;

namespace FEMEE.Application.Validators.Partida
{
    /// <summary>
    /// Validador para UpdatePartidaDto.
    /// Valida dados de atualização de partida.
    /// </summary>
    public class UpdatePartidaDtoValidator : AbstractValidator<UpdatePartidaDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public UpdatePartidaDtoValidator()
        {
            // ===== VALIDAR PLACAR TIME A =====
            RuleFor(x => x.PlacarTimeA)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Placar do time A não pode ser negativo")
                .LessThan(1000)
                    .WithMessage("Placar do time A não pode ser maior que 999");

            // ===== VALIDAR PLACAR TIME B =====
            RuleFor(x => x.PlacarTimeB)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Placar do time B não pode ser negativo")
                .LessThan(1000)
                    .WithMessage("Placar do time B não pode ser maior que 999");

            // ===== VALIDAR STATUS =====
            RuleFor(x => x.Status)
                .IsInEnum()
                    .WithMessage("Status inválido");

            // ===== VALIDAR TRANSMISSÃO URL =====
            RuleFor(x => x.TransmissaoUrl)
                .Must(IsValidUrl)
                    .WithMessage("URL da transmissão inválida")
                .When(x => !string.IsNullOrEmpty(x.TransmissaoUrl));
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
