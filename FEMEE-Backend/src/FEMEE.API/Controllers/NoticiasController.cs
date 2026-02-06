using FEMEE.Application.DTOs.Noticia;
using FEMEE.Application.Interfaces.Services;

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de notícias.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly INoticiaService _noticiaService;
        private readonly IValidator<CreateNoticiaDto> _createValidator;
        private readonly IValidator<UpdateNoticiaDto> _updateValidator;
        private readonly ILogger<NoticiasController> _logger;

        public NoticiasController(
            INoticiaService noticiaService,
            IValidator<CreateNoticiaDto> createValidator,
            IValidator<UpdateNoticiaDto> updateValidator,
            ILogger<NoticiasController> logger)
        {
            _noticiaService = noticiaService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        /// <summary>
        /// Obtém uma notícia pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNoticiaById(int id)
        {
            try
            {
                var noticia = await _noticiaService.GetNoticiaByIdAsync(id);
                return Ok(noticia);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todas as notícias com paginação.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNoticias([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var noticias = await _noticiaService.GetAllNoticiasAsync(page, pageSize);
            return Ok(noticias);
        }

        /// <summary>
        /// Cria uma nova notícia.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateNoticia([FromBody] CreateNoticiaDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var noticia = await _noticiaService.CreateNoticiaAsync(dto);
                return CreatedAtAction(nameof(GetNoticiaById), new { id = noticia.Id }, noticia);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma notícia.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateNoticia(int id, [FromBody] UpdateNoticiaDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var noticia = await _noticiaService.UpdateNoticiaAsync(id, dto);
                return Ok(noticia);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deleta uma notícia.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteNoticia(int id)
        {
            try
            {
                await _noticiaService.DeleteNoticiaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
