using FEMEE.Application.DTOs.Partida;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de partidas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PartidasController : ControllerBase
    {
        private readonly IPartidaService _partidaService;
        private readonly IValidator<CreatePartidaDto> _createValidator;
        private readonly IValidator<UpdatePartidaDto> _updateValidator;
        private readonly ILogger<PartidasController> _logger;

        public PartidasController(
            IPartidaService partidaService,
            IValidator<CreatePartidaDto> createValidator,
            IValidator<UpdatePartidaDto> updateValidator,
            ILogger<PartidasController> logger)
        {
            _partidaService = partidaService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        /// <summary>
        /// Obtém uma partida pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPartidaById(int id)
        {
            try
            {
                var partida = await _partidaService.GetPartidaByIdAsync(id);
                return Ok(partida);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todas as partidas.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPartidas()
        {
            var partidas = await _partidaService.GetAllPartidasAsync();
            return Ok(partidas);
        }

        /// <summary>
        /// Obtém partidas de um campeonato.
        /// </summary>
        [HttpGet("campeonato/{campeonatoId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPartidasByCampeonato(int campeonatoId)
        {
            var partidas = await _partidaService.GetPartidasByCampeonatoAsync(campeonatoId);
            return Ok(partidas);
        }

        /// <summary>
        /// Obtém partidas de um time.
        /// </summary>
        [HttpGet("time/{timeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPartidasByTime(int timeId)
        {
            var partidas = await _partidaService.GetPartidasByTimeAsync(timeId);
            return Ok(partidas);
        }

        /// <summary>
        /// Cria uma nova partida.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreatePartida([FromBody] CreatePartidaDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var partida = await _partidaService.CreatePartidaAsync(dto);
                return CreatedAtAction(nameof(GetPartidaById), new { id = partida.Id }, partida);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma partida.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdatePartida(int id, [FromBody] UpdatePartidaDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var partida = await _partidaService.UpdatePartidaAsync(id, dto);
                return Ok(partida);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Finaliza uma partida.
        /// </summary>
        [HttpPost("{id}/finish")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> FinishPartida(int id, [FromBody] FinishPartidaRequestDto request)
        {
            try
            {
                var partida = await _partidaService.FinishPartidaAsync(id, request.TimeVencedorId, request.PlacarA, request.PlacarB);
                return Ok(partida);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deleta uma partida.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeletePartida(int id)
        {
            try
            {
                await _partidaService.DeletePartidaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
