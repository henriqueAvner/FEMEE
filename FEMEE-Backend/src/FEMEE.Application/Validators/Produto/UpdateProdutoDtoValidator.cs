// FEMEE.Application/Validators/Produto/UpdateProdutoDtoValidator.cs

using FluentValidation;
using FEMEE.Application.DTOs.Produto;

namespace FEMEE.Application.Validators.Produto
{
    /// <summary>
    /// Validador para UpdateProdutoDto.
    /// Valida dados de atualização de produto.
    /// </summary>
    public class UpdateProdutoDtoValidator : AbstractValidator<UpdateProdutoDto>
    {
        /// <summary>
        /// Construtor que define todas as regras de validação.
        /// </summary>
        public UpdateProdutoDtoValidator()
        {
            // ===== VALIDAR NOME =====
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome é obrigatório")
                .Length(3, 256)
                    .WithMessage("Nome deve ter entre 3 e 256 caracteres");

            // ===== VALIDAR DESCRIÇÃO =====
            RuleFor(x => x.Descricao)
                .NotEmpty()
                    .WithMessage("Descrição é obrigatória")
                .MaximumLength(1000)
                    .WithMessage("Descrição não pode ter mais de 1000 caracteres");

            // ===== VALIDAR PREÇO =====
            RuleFor(x => x.Preco)
                .GreaterThan(0)
                    .WithMessage("Preço deve ser maior que zero")
                .LessThan(1000000)
                    .WithMessage("Preço não pode ser maior que 1 milhão");

            // ===== VALIDAR IMAGEM URL =====
            RuleFor(x => x.ImagemUrl)
                .Must(IsValidUrl)
                    .WithMessage("URL da imagem inválida")
                .When(x => !string.IsNullOrEmpty(x.ImagemUrl));

            // ===== VALIDAR ESTOQUE =====
            RuleFor(x => x.Estoque)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Estoque não pode ser negativo")
                .LessThan(1000000)
                    .WithMessage("Estoque não pode ser maior que 1 milhão");
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
