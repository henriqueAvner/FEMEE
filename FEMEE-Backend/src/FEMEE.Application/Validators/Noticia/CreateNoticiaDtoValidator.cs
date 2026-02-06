using FluentValidation;
using FEMEE.Application.DTOs.Noticia;

namespace FEMEE.Application.Validators.Noticia
{
    /// <summary>
    /// Validador para CreateNoticiaDto.
    /// Valida dados de criação de nova notícia.
    /// </summary>
    public class CreateNoticiaDtoValidator : AbstractValidator<CreateNoticiaDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public CreateNoticiaDtoValidator()
        {
            // ===== VALIDAR TÍTULO =====
            RuleFor(x => x.Titulo)
                .NotEmpty()
                    .WithMessage("Título é obrigatório")
                .Length(3, 256)
                    .WithMessage("Título deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR SLUG =====
            RuleFor(x => x.Slug)
                .NotEmpty()
                    .WithMessage("Slug é obrigatório")
                .Matches(@"^[a-z0-9-]+$")
                    .WithMessage("Slug deve conter apenas letras minúsculas, números e hífens");

            // ===== VALIDAR RESUMO =====
            RuleFor(x => x.Resumo)
                .NotEmpty()
                    .WithMessage("Resumo é obrigatório")
                .Length(10, 500)
                    .WithMessage("Resumo deve ter entre 10 e 500 caracteres");

            // ===== VALIDAR CONTEÚDO =====
            RuleFor(x => x.Conteudo)
                .NotEmpty()
                    .WithMessage("Conteúdo é obrigatório")
                .MinimumLength(50)
                    .WithMessage("Conteúdo deve ter no mínimo 50 caracteres");

            // ===== VALIDAR CATEGORIA =====
            RuleFor(x => x.Categoria)
                .NotEmpty()
                    .WithMessage("Categoria é obrigatória")
                .Length(3, 50)
                    .WithMessage("Categoria deve ter entre 3 e 50 caracteres");

            // ===== VALIDAR IMAGEM URL =====
            RuleFor(x => x.ImagemUrl)
                .Must(IsValidUrl)
                    .WithMessage("URL da imagem inválida");
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
