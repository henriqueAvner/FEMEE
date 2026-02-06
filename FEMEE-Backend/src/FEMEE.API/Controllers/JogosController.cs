using FEMEE.Application.DTOs.Jogo;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller de jogos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;
        private readonly IValidator<CreateJogoDto> _createValidator;
        private readonly IValidator<UpdateJogoDto> _updateValidator;
        private readonly ILogger<JogosController> _logger;

        public JogosController(
            IJogoService jogoService,
            IValidator<CreateJogoDto> createValidator,
            IValidator<UpdateJogoDto> updateValidator,
            ILogger<JogosController> logger)
        {
            _jogoService = jogoService ?? throw new ArgumentNullException(nameof(jogoService));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém todos os jogos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JogoResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Requisição para obter todos os jogos");
            var jogos = await _jogoService.GetAllJogosAsync();
            return Ok(jogos);
        }

        /// <summary>
        /// Obtém todos os jogos ativos.
        /// </summary>
        [HttpGet("ativos")]
        [ProducesResponseType(typeof(IEnumerable<JogoResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAtivos()
        {
            _logger.LogInformation("Requisição para obter jogos ativos");
            var jogos = await _jogoService.GetJogosAtivosAsync();
            return Ok(jogos);
        }

        /// <summary>
        /// Obtém um jogo pelo ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(JogoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Requisição para obter jogo: {JogoId}", id);
            var jogo = await _jogoService.GetJogoByIdAsync(id);
            return Ok(jogo);
        }

        /// <summary>
        /// Obtém um jogo pelo slug.
        /// </summary>
        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(JogoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            _logger.LogInformation("Requisição para obter jogo pelo slug: {Slug}", slug);
            var jogo = await _jogoService.GetJogoBySlugAsync(slug);
            return Ok(jogo);
        }

        /// <summary>
        /// Cria um novo jogo.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(JogoResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateJogoDto dto)
        {
            _logger.LogInformation("Requisição para criar jogo: {Nome}", dto.Nome);

            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var jogo = await _jogoService.CreateJogoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = jogo.Id }, jogo);
        }

        /// <summary>
        /// Atualiza um jogo existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(JogoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJogoDto dto)
        {
            _logger.LogInformation("Requisição para atualizar jogo: {JogoId}", id);

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var jogo = await _jogoService.UpdateJogoAsync(id, dto);
            return Ok(jogo);
        }

        /// <summary>
        /// Deleta um jogo.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Requisição para deletar jogo: {JogoId}", id);
            await _jogoService.DeleteJogoAsync(id);
            return NoContent();
        }
    }
}
