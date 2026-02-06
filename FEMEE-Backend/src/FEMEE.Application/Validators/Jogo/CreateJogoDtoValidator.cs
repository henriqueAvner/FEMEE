using FluentValidation;
using FEMEE.Application.DTOs.Jogo;

namespace FEMEE.Application.Validators.Jogo
{
    /// <summary>
    /// Validador para CreateJogoDto.
    /// </summary>
    public class CreateJogoDtoValidator : AbstractValidator<CreateJogoDto>
    {
        public CreateJogoDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome é obrigatório")
                .MaximumLength(256)
                    .WithMessage("Nome deve ter no máximo 256 caracteres");

            RuleFor(x => x.Slug)
                .NotEmpty()
                    .WithMessage("Slug é obrigatório")
                .MaximumLength(256)
                    .WithMessage("Slug deve ter no máximo 256 caracteres")
                .Matches(@"^[a-z0-9\-]+$")
                    .WithMessage("Slug deve conter apenas letras minúsculas, números e hífens");

            RuleFor(x => x.Descricao)
                .MaximumLength(1000)
                    .WithMessage("Descrição deve ter no máximo 1000 caracteres");

            RuleFor(x => x.ImagemUrl)
                .MaximumLength(512)
                    .WithMessage("URL da imagem deve ter no máximo 512 caracteres");

            RuleFor(x => x.CategoriaJogo)
                .IsInEnum()
                    .WithMessage("Categoria do jogo inválida");
        }
    }
}
