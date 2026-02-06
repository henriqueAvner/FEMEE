using FluentValidation;
using FEMEE.Application.DTOs.Campeonato;

namespace FEMEE.Application.Validators.Campeonato
{
    /// <summary>
    /// Validador para CreateCampeonatoDto.
    /// Valida dados de criação de novo campeonato.
    /// </summary>
    public class CreateCampeonatoDtoValidator : AbstractValidator<CreateCampeonatoDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public CreateCampeonatoDtoValidator()
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

            // ===== VALIDAR DATA INÍCIO =====
            RuleFor(x => x.DataInicio)
                .NotEmpty()
                    .WithMessage("Data de início é obrigatória")
                .GreaterThan(DateTime.Now)
                    .WithMessage("Data de início deve ser no futuro");

            // ===== VALIDAR DATA FIM =====
            RuleFor(x => x.DataFim)
                .NotEmpty()
                    .WithMessage("Data de fim é obrigatória")
                .GreaterThan(x => x.DataInicio)
                    .WithMessage("Data de fim deve ser após a data de início");

            // ===== VALIDAR DATA LIMITE INSCRIÇÃO =====
            RuleFor(x => x.DataLimiteInscricao)
                .NotEmpty()
                    .WithMessage("Data limite de inscrição é obrigatória")
                .LessThan(x => x.DataInicio)
                    .WithMessage("Data limite de inscrição deve ser antes da data de início")
                .GreaterThan(DateTime.Now)
                    .WithMessage("Data limite de inscrição deve ser no futuro");

            // ===== VALIDAR LOCAL =====
            RuleFor(x => x.Local)
                .NotEmpty()
                    .WithMessage("Local é obrigatório")
                .Length(3, 256)
                    .WithMessage("Local deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR CIDADE =====
            RuleFor(x => x.Cidade)
                .NotEmpty()
                    .WithMessage("Cidade é obrigatória")
                .Length(2, 100)
                    .WithMessage("Cidade deve ter entre 2 e 100 caracteres");

            // ===== VALIDAR ESTADO =====
            RuleFor(x => x.Estado)
                .NotEmpty()
                    .WithMessage("Estado é obrigatório")
                .Length(2, 100)
                    .WithMessage("Estado deve ter entre 2 e 100 caracteres");

            // ===== VALIDAR PREMIAÇÃO =====
            RuleFor(x => x.Premiacao)
                .GreaterThan(0)
                    .WithMessage("Premiação deve ser maior que zero")
                .LessThan(10000000)
                    .WithMessage("Premiação não pode ser maior que 10 milhões");

            // ===== VALIDAR NÚMERO DE VAGAS =====
            RuleFor(x => x.NumeroVagas)
                .GreaterThan(0)
                    .WithMessage("Número de vagas deve ser maior que zero")
                .LessThan(1000)
                    .WithMessage("Número de vagas não pode ser maior que 1000");

            // ===== VALIDAR JOGO ID =====
            RuleFor(x => x.JogoId)
                .GreaterThan(0)
                    .WithMessage("Jogo é obrigatório");

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
