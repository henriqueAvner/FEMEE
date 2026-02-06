using FluentValidation;
using FEMEE.Application.DTOs.InscricaoCampeonato;

namespace FEMEE.Application.Validators.InscricaoCampeonato
{
    /// <summary>
    /// Validador para CreateInscricaoCampeonatoDto.
    /// Valida dados de inscrição em campeonato.
    /// </summary>
    public class CreateInscricaoCampeonatoDtoValidator : AbstractValidator<CreateInscricaoCampeonatoDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public CreateInscricaoCampeonatoDtoValidator()
        {
            // ===== VALIDAR CAMPEONATO ID =====
            RuleFor(x => x.CampeonatoId)
                .GreaterThan(0)
                    .WithMessage("Campeonato é obrigatório");

            // ===== VALIDAR TIME ID =====
            RuleFor(x => x.TimeId)
                .GreaterThan(0)
                    .WithMessage("Time é obrigatório");
        }
    }
}
