using FluentValidation;
using FEMEE.Application.DTOs.Jogo;

namespace FEMEE.Application.Validators.Jogo
{
    /// <summary>
    /// Validador para UpdateJogoDto.
    /// </summary>
    public class UpdateJogoDtoValidator : AbstractValidator<UpdateJogoDto>
    {
        public UpdateJogoDtoValidator()
        {
            RuleFor(x => x.Nome)
                .MaximumLength(256)
                    .WithMessage("Nome deve ter no máximo 256 caracteres")
                .When(x => x.Nome != null);

            RuleFor(x => x.Slug)
                .MaximumLength(256)
                    .WithMessage("Slug deve ter no máximo 256 caracteres")
                .Matches(@"^[a-z0-9\-]+$")
                    .WithMessage("Slug deve conter apenas letras minúsculas, números e hífens")
                .When(x => x.Slug != null);

            RuleFor(x => x.Descricao)
                .MaximumLength(1000)
                    .WithMessage("Descrição deve ter no máximo 1000 caracteres")
                .When(x => x.Descricao != null);

            RuleFor(x => x.ImagemUrl)
                .MaximumLength(512)
                    .WithMessage("URL da imagem deve ter no máximo 512 caracteres")
                .When(x => x.ImagemUrl != null);

            RuleFor(x => x.CategoriaJogo)
                .IsInEnum()
                    .WithMessage("Categoria do jogo inválida")
                .When(x => x.CategoriaJogo != null);
        }
    }
}
