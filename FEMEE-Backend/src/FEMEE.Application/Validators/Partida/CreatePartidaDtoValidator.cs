using FluentValidation;
using FEMEE.Application.DTOs.Partida;

namespace FEMEE.Application.Validators.Partida
{
    /// <summary>
    /// Validador para CreatePartidaDto.
    /// Valida dados de criação de nova partida.
    /// </summary>
    public class CreatePartidaDtoValidator : AbstractValidator<CreatePartidaDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public CreatePartidaDtoValidator()
        {
            // ===== VALIDAR CAMPEONATO ID =====
            RuleFor(x => x.CampeonatoId)
                .GreaterThan(0)
                    .WithMessage("Campeonato é obrigatório");

            // ===== VALIDAR TIME A ID =====
            RuleFor(x => x.TimeAId)
                .GreaterThan(0)
                    .WithMessage("Time A é obrigatório");

            // ===== VALIDAR TIME B ID =====
            RuleFor(x => x.TimeBId)
                .GreaterThan(0)
                    .WithMessage("Time B é obrigatório")
                .NotEqual(x => x.TimeAId)
                    .WithMessage("Times devem ser diferentes");

            // ===== VALIDAR DATA E HORA =====
            RuleFor(x => x.DataHora)
                .NotEmpty()
                    .WithMessage("Data e hora são obrigatórias")
                .GreaterThan(DateTime.Now)
                    .WithMessage("Data e hora devem ser no futuro");

            // ===== VALIDAR LOCAL =====
            RuleFor(x => x.Local)
                .NotEmpty()
                    .WithMessage("Local é obrigatório")
                .Length(3, 256)
                    .WithMessage("Local deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR FASE =====
            RuleFor(x => x.Fase)
                .IsInEnum()
                    .WithMessage("Fase inválida");
        }
    }
}
