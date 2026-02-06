using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.Noticia;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class NoticiasControllerTests
    {
        private readonly Mock<INoticiaService> _noticiaServiceMock;
        private readonly Mock<IValidator<CreateNoticiaDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateNoticiaDto>> _updateValidatorMock;
        private readonly Mock<ILogger<NoticiasController>> _loggerMock;
        private readonly NoticiasController _controller;

        public NoticiasControllerTests()
        {
            _noticiaServiceMock = new Mock<INoticiaService>();
            _createValidatorMock = new Mock<IValidator<CreateNoticiaDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdateNoticiaDto>>();
            _loggerMock = new Mock<ILogger<NoticiasController>>();

            _controller = new NoticiasController(
                _noticiaServiceMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object);
        }

        #region GetNoticiaById Tests

        [Fact]
        public async Task GetNoticiaById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new NoticiaResponseDto { Id = id, Titulo = "Test Noticia" };
            _noticiaServiceMock.Setup(x => x.GetNoticiaByIdAsync(id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetNoticiaById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<NoticiaResponseDto>(okResult.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task GetNoticiaById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _noticiaServiceMock.Setup(x => x.GetNoticiaByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Notícia não encontrada"));

            // Act
            var result = await _controller.GetNoticiaById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetAllNoticias Tests

        [Fact]
        public async Task GetAllNoticias_ReturnsOkWithPaginatedNoticias()
        {
            // Arrange
            var noticias = new List<NoticiaResponseDto>
            {
                new() { Id = 1, Titulo = "Noticia 1" },
                new() { Id = 2, Titulo = "Noticia 2" }
            };
            _noticiaServiceMock.Setup(x => x.GetAllNoticiasAsync(1, 10)).ReturnsAsync(noticias);

            // Act
            var result = await _controller.GetAllNoticias(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<NoticiaResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        [Fact]
        public async Task GetAllNoticias_WithCustomPagination_ReturnsOk()
        {
            // Arrange
            var noticias = new List<NoticiaResponseDto>
            {
                new() { Id = 3, Titulo = "Noticia 3" }
            };
            _noticiaServiceMock.Setup(x => x.GetAllNoticiasAsync(2, 5)).ReturnsAsync(noticias);

            // Act
            var result = await _controller.GetAllNoticias(2, 5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<NoticiaResponseDto>>(okResult.Value);
            Assert.Single(returned);
        }

        #endregion

        #region CreateNoticia Tests

        [Fact]
        public async Task CreateNoticia_WithValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new CreateNoticiaDto { Titulo = "New Noticia", Conteudo = "Content here" };
            var created = new NoticiaResponseDto { Id = 1, Titulo = dto.Titulo };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _noticiaServiceMock.Setup(x => x.CreateNoticiaAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.CreateNoticia(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(NoticiasController.GetNoticiaById), createdResult.ActionName);
        }

        [Fact]
        public async Task CreateNoticia_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateNoticiaDto { Titulo = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Titulo", "Título é obrigatório") });

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateNoticia(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateNoticia_WithInvalidOperation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateNoticiaDto { Titulo = "Noticia", Conteudo = "Content" };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _noticiaServiceMock.Setup(x => x.CreateNoticiaAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Error creating noticia"));

            // Act
            var result = await _controller.CreateNoticia(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdateNoticia Tests

        [Fact]
        public async Task UpdateNoticia_WithValidData_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateNoticiaDto { Titulo = "Updated Noticia" };
            var updated = new NoticiaResponseDto { Id = id, Titulo = dto.Titulo };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _noticiaServiceMock.Setup(x => x.UpdateNoticiaAsync(id, dto)).ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdateNoticia(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<NoticiaResponseDto>(okResult.Value);
            Assert.Equal(dto.Titulo, returned.Titulo);
        }

        [Fact]
        public async Task UpdateNoticia_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var dto = new UpdateNoticiaDto { Titulo = "Updated" };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _noticiaServiceMock.Setup(x => x.UpdateNoticiaAsync(id, dto))
                .ThrowsAsync(new KeyNotFoundException("Notícia não encontrada"));

            // Act
            var result = await _controller.UpdateNoticia(id, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeleteNoticia Tests

        [Fact]
        public async Task DeleteNoticia_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _noticiaServiceMock.Setup(x => x.DeleteNoticiaAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteNoticia(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteNoticia_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _noticiaServiceMock.Setup(x => x.DeleteNoticiaAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Notícia não encontrada"));

            // Act
            var result = await _controller.DeleteNoticia(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
