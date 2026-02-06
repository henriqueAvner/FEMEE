using FEMEE.Application.DTOs.InscricaoCampeonato;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller de inscrições de campeonato.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesCampeonatoController : ControllerBase
    {
        private readonly IInscricaoCampeonatoService _inscricaoService;
        private readonly IValidator<CreateInscricaoCampeonatoDto> _createValidator;
        private readonly IValidator<UpdateInscricaoCampeonatoDto> _updateValidator;
        private readonly ILogger<InscricoesCampeonatoController> _logger;

        public InscricoesCampeonatoController(
            IInscricaoCampeonatoService inscricaoService,
            IValidator<CreateInscricaoCampeonatoDto> createValidator,
            IValidator<UpdateInscricaoCampeonatoDto> updateValidator,
            ILogger<InscricoesCampeonatoController> logger)
        {
            _inscricaoService = inscricaoService ?? throw new ArgumentNullException(nameof(inscricaoService));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém todas as inscrições.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(IEnumerable<InscricaoCampeonatoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Requisição para obter todas as inscrições");
            var inscricoes = await _inscricaoService.GetAllInscricoesAsync();
            return Ok(inscricoes);
        }

        /// <summary>
        /// Obtém uma inscrição pelo ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(InscricaoCampeonatoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Requisição para obter inscrição: {InscricaoId}", id);
            var inscricao = await _inscricaoService.GetInscricaoByIdAsync(id);
            return Ok(inscricao);
        }

        /// <summary>
        /// Obtém inscrições por campeonato.
        /// </summary>
        [HttpGet("campeonato/{campeonatoId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<InscricaoCampeonatoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByCampeonatoId(int campeonatoId)
        {
            _logger.LogInformation("Requisição para obter inscrições do campeonato: {CampeonatoId}", campeonatoId);
            var inscricoes = await _inscricaoService.GetInscricoesByCampeonatoIdAsync(campeonatoId);
            return Ok(inscricoes);
        }

        /// <summary>
        /// Obtém inscrições por time.
        /// </summary>
        [HttpGet("time/{timeId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<InscricaoCampeonatoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByTimeId(int timeId)
        {
            _logger.LogInformation("Requisição para obter inscrições do time: {TimeId}", timeId);
            var inscricoes = await _inscricaoService.GetInscricoesByTimeIdAsync(timeId);
            return Ok(inscricoes);
        }

        /// <summary>
        /// Obtém inscrições por status.
        /// </summary>
        [HttpGet("status/{status}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(IEnumerable<InscricaoCampeonatoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByStatus(StatusInscricao status)
        {
            _logger.LogInformation("Requisição para obter inscrições com status: {Status}", status);
            var inscricoes = await _inscricaoService.GetInscricoesByStatusAsync(status);
            return Ok(inscricoes);
        }

        /// <summary>
        /// Cria uma nova inscrição.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(InscricaoCampeonatoResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateInscricaoCampeonatoDto dto)
        {
            _logger.LogInformation("Requisição para criar inscrição no campeonato: {CampeonatoId}", dto.CampeonatoId);

            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var inscricao = await _inscricaoService.CreateInscricaoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = inscricao.Id }, inscricao);
        }

        /// <summary>
        /// Atualiza uma inscrição existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOrCapitao")]
        [ProducesResponseType(typeof(InscricaoCampeonatoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInscricaoCampeonatoDto dto)
        {
            _logger.LogInformation("Requisição para atualizar inscrição: {InscricaoId}", id);

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var inscricao = await _inscricaoService.UpdateInscricaoAsync(id, dto);
            return Ok(inscricao);
        }

        /// <summary>
        /// Aprova uma inscrição.
        /// </summary>
        [HttpPost("{id:int}/aprovar")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(InscricaoCampeonatoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Aprovar(int id)
        {
            _logger.LogInformation("Requisição para aprovar inscrição: {InscricaoId}", id);
            var inscricao = await _inscricaoService.AprovarInscricaoAsync(id);
            return Ok(inscricao);
        }

        /// <summary>
        /// Rejeita uma inscrição.
        /// </summary>
        [HttpPost("{id:int}/rejeitar")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(InscricaoCampeonatoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Rejeitar(int id, [FromQuery] string? motivo = null)
        {
            _logger.LogInformation("Requisição para rejeitar inscrição: {InscricaoId}", id);
            var inscricao = await _inscricaoService.RejeitarInscricaoAsync(id, motivo);
            return Ok(inscricao);
        }

        /// <summary>
        /// Deleta uma inscrição.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Requisição para deletar inscrição: {InscricaoId}", id);
            await _inscricaoService.DeleteInscricaoAsync(id);
            return NoContent();
        }
    }
}
