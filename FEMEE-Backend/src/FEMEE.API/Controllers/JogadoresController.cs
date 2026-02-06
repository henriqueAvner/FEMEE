using FEMEE.Application.DTOs.Jogador;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de jogadores.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class JogadoresController : ControllerBase
    {
        private readonly IJogadorService _jogadorService;
        private readonly IValidator<CreateJogadorDto> _createValidator;
        private readonly IValidator<UpdateJogadorDto> _updateValidator;
        private readonly ILogger<JogadoresController> _logger;

        public JogadoresController(
            IJogadorService jogadorService,
            IValidator<CreateJogadorDto> createValidator,
            IValidator<UpdateJogadorDto> updateValidator,
            ILogger<JogadoresController> logger)
        {
            _jogadorService = jogadorService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        /// <summary>
        /// Obtém um jogador pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJogadorById(int id)
        {
            try
            {
                var jogador = await _jogadorService.GetJogadorByIdAsync(id);
                return Ok(jogador);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todos os jogadores.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllJogadores()
        {
            var jogadores = await _jogadorService.GetAllJogadoresAsync();
            return Ok(jogadores);
        }

        /// <summary>
        /// Obtém jogadores de um time.
        /// </summary>
        [HttpGet("time/{timeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJogadoresByTime(int timeId)
        {
            var jogadores = await _jogadorService.GetJogadoresByTimeAsync(timeId);
            return Ok(jogadores);
        }

        /// <summary>
        /// Cria um novo jogador.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateJogador([FromBody] CreateJogadorDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var jogador = await _jogadorService.CreateJogadorAsync(dto);
                return CreatedAtAction(nameof(GetJogadorById), new { id = jogador.Id }, jogador);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um jogador.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateJogador(int id, [FromBody] UpdateJogadorDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var jogador = await _jogadorService.UpdateJogadorAsync(id, dto);
                return Ok(jogador);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um jogador.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteJogador(int id)
        {
            try
            {
                await _jogadorService.DeleteJogadorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
