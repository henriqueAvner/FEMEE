using FEMEE.Application.DTOs.Conquista;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller de conquistas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConquistasController : ControllerBase
    {
        private readonly IConquistaService _conquistaService;
        private readonly IValidator<CreateConquistaDto> _createValidator;
        private readonly IValidator<UpdateConquistaDto> _updateValidator;
        private readonly ILogger<ConquistasController> _logger;

        public ConquistasController(
            IConquistaService conquistaService,
            IValidator<CreateConquistaDto> createValidator,
            IValidator<UpdateConquistaDto> updateValidator,
            ILogger<ConquistasController> logger)
        {
            _conquistaService = conquistaService ?? throw new ArgumentNullException(nameof(conquistaService));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém todas as conquistas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ConquistaResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Requisição para obter todas as conquistas");
            var conquistas = await _conquistaService.GetAllConquistasAsync();
            return Ok(conquistas);
        }

        /// <summary>
        /// Obtém uma conquista pelo ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ConquistaResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Requisição para obter conquista: {ConquistaId}", id);
            var conquista = await _conquistaService.GetConquistaByIdAsync(id);
            return Ok(conquista);
        }

        /// <summary>
        /// Obtém conquistas por time.
        /// </summary>
        [HttpGet("time/{timeId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ConquistaResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByTimeId(int timeId)
        {
            _logger.LogInformation("Requisição para obter conquistas do time: {TimeId}", timeId);
            var conquistas = await _conquistaService.GetConquistasByTimeIdAsync(timeId);
            return Ok(conquistas);
        }

        /// <summary>
        /// Obtém conquistas por campeonato.
        /// </summary>
        [HttpGet("campeonato/{campeonatoId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ConquistaResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCampeonatoId(int campeonatoId)
        {
            _logger.LogInformation("Requisição para obter conquistas do campeonato: {CampeonatoId}", campeonatoId);
            var conquistas = await _conquistaService.GetConquistasByCampeonatoIdAsync(campeonatoId);
            return Ok(conquistas);
        }

        /// <summary>
        /// Cria uma nova conquista.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(ConquistaResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateConquistaDto dto)
        {
            _logger.LogInformation("Requisição para criar conquista: {Titulo}", dto.Titulo);

            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var conquista = await _conquistaService.CreateConquistaAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = conquista.Id }, conquista);
        }

        /// <summary>
        /// Atualiza uma conquista existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(ConquistaResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateConquistaDto dto)
        {
            _logger.LogInformation("Requisição para atualizar conquista: {ConquistaId}", id);

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var conquista = await _conquistaService.UpdateConquistaAsync(id, dto);
            return Ok(conquista);
        }

        /// <summary>
        /// Deleta uma conquista.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Requisição para deletar conquista: {ConquistaId}", id);
            await _conquistaService.DeleteConquistaAsync(id);
            return NoContent();
        }
    }
}
