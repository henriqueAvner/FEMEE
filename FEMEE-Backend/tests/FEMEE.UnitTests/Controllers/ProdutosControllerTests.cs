using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.Produto;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class ProdutosControllerTests
    {
        private readonly Mock<IProdutoService> _produtoServiceMock;
        private readonly Mock<IValidator<CreateProdutoDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateProdutoDto>> _updateValidatorMock;
        private readonly Mock<ILogger<ProdutosController>> _loggerMock;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            _produtoServiceMock = new Mock<IProdutoService>();
            _createValidatorMock = new Mock<IValidator<CreateProdutoDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdateProdutoDto>>();
            _loggerMock = new Mock<ILogger<ProdutosController>>();

            _controller = new ProdutosController(
                _produtoServiceMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object);
        }

        #region GetProdutoById Tests

        [Fact]
        public async Task GetProdutoById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new ProdutoResponseDto { Id = id, Nome = "Test Product" };
            _produtoServiceMock.Setup(x => x.GetProdutoByIdAsync(id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetProdutoById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<ProdutoResponseDto>(okResult.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task GetProdutoById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _produtoServiceMock.Setup(x => x.GetProdutoByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Produto não encontrado"));

            // Act
            var result = await _controller.GetProdutoById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetAllProdutos Tests

        [Fact]
        public async Task GetAllProdutos_ReturnsOkWithProdutos()
        {
            // Arrange
            var produtos = new List<ProdutoResponseDto>
            {
                new() { Id = 1, Nome = "Product 1" },
                new() { Id = 2, Nome = "Product 2" }
            };
            _produtoServiceMock.Setup(x => x.GetAllProdutosAsync()).ReturnsAsync(produtos);

            // Act
            var result = await _controller.GetAllProdutos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<ProdutoResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        #endregion

        #region CreateProduto Tests

        [Fact]
        public async Task CreateProduto_WithValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new CreateProdutoDto { Nome = "New Product", Preco = 99.99m };
            var created = new ProdutoResponseDto { Id = 1, Nome = dto.Nome, Preco = dto.Preco };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _produtoServiceMock.Setup(x => x.CreateProdutoAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.CreateProduto(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ProdutosController.GetProdutoById), createdResult.ActionName);
        }

        [Fact]
        public async Task CreateProduto_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateProdutoDto { Nome = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Nome", "Nome é obrigatório") });

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateProduto(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateProduto_WithInvalidOperation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateProdutoDto { Nome = "Product", Preco = 10 };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _produtoServiceMock.Setup(x => x.CreateProdutoAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Error creating product"));

            // Act
            var result = await _controller.CreateProduto(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdateProduto Tests

        [Fact]
        public async Task UpdateProduto_WithValidData_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateProdutoDto { Nome = "Updated Product" };
            var updated = new ProdutoResponseDto { Id = id, Nome = dto.Nome };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _produtoServiceMock.Setup(x => x.UpdateProdutoAsync(id, dto)).ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdateProduto(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<ProdutoResponseDto>(okResult.Value);
            Assert.Equal(dto.Nome, returned.Nome);
        }

        [Fact]
        public async Task UpdateProduto_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var dto = new UpdateProdutoDto { Nome = "Updated" };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _produtoServiceMock.Setup(x => x.UpdateProdutoAsync(id, dto))
                .ThrowsAsync(new KeyNotFoundException("Produto não encontrado"));

            // Act
            var result = await _controller.UpdateProduto(id, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeleteProduto Tests

        [Fact]
        public async Task DeleteProduto_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _produtoServiceMock.Setup(x => x.DeleteProdutoAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduto(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduto_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _produtoServiceMock.Setup(x => x.DeleteProdutoAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Produto não encontrado"));

            // Act
            var result = await _controller.DeleteProduto(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
