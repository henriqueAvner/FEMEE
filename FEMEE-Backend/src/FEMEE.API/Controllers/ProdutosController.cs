using FEMEE.Application.DTOs.Produto;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de produtos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IValidator<CreateProdutoDto> _createValidator;
        private readonly IValidator<UpdateProdutoDto> _updateValidator;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(
            IProdutoService produtoService,
            IValidator<CreateProdutoDto> createValidator,
            IValidator<UpdateProdutoDto> updateValidator,
            ILogger<ProdutosController> logger)
        {
            _produtoService = produtoService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        /// <summary>
        /// Obtém um produto pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProdutoById(int id)
        {
            try
            {
                var produto = await _produtoService.GetProdutoByIdAsync(id);
                return Ok(produto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todos os produtos.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProdutos()
        {
            var produtos = await _produtoService.GetAllProdutosAsync();
            return Ok(produtos);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var produto = await _produtoService.CreateProdutoAsync(dto);
                return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um produto.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] UpdateProdutoDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var produto = await _produtoService.UpdateProdutoAsync(id, dto);
                return Ok(produto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um produto.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                await _produtoService.DeleteProdutoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
