using FEMEE.Application.DTOs.Common;
using FEMEE.Application.DTOs.Time;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de times.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimesController : ControllerBase
    {
        private readonly ITimeService _timeService;
        private readonly IValidator<CreateTimeDto> _createValidator;
        private readonly IValidator<UpdateTimeDto> _updateValidator;
        private readonly ILogger<TimesController> _logger;

        public TimesController(
            ITimeService timeService,
            IValidator<CreateTimeDto> createValidator,
            IValidator<UpdateTimeDto> updateValidator,
            ILogger<TimesController> logger)
        {
            _timeService = timeService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        /// <summary>
        /// Obtém times com paginação, busca e ordenação.
        /// </summary>
        /// <param name="pagination">Parâmetros de paginação (page, pageSize, search, sortBy, sortDirection)</param>
        /// <returns>Lista paginada de times</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTimes([FromQuery] PaginationParams pagination)
        {
            var result = await _timeService.GetTimesPagedAsync(pagination);
            return Ok(result);
        }

        /// <summary>
        /// Obtém um time pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTimeById(int id)
        {
            try
            {
                var time = await _timeService.GetTimeByIdAsync(id);
                return Ok(time);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Obtém um time pelo slug.
        /// </summary>
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTimeBySlug(string slug)
        {
            try
            {
                var time = await _timeService.GetTimeBySlugAsync(slug);
                return Ok(time);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Obtém times ordenados por ranking.
        /// </summary>
        [HttpGet("ranking")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTimesByRanking()
        {
            var times = await _timeService.GetTimesByRankingAsync();
            return Ok(times);
        }

        /// <summary>
        /// Cria um novo time.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateTime([FromBody] CreateTimeDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var time = await _timeService.CreateTimeAsync(dto);
                return CreatedAtAction(nameof(GetTimeById), new { id = time.Id }, time);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um time.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrCapitao")]
        public async Task<IActionResult> UpdateTime(int id, [FromBody] UpdateTimeDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var time = await _timeService.UpdateTimeAsync(id, dto);
                return Ok(time);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um time.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteTime(int id)
        {
            try
            {
                await _timeService.DeleteTimeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
