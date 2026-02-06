using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.Campeonato;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class CampeonatosControllerTests
    {
        private readonly Mock<ICampeonatoService> _campeonatoServiceMock;
        private readonly Mock<IValidator<CreateCampeonatoDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateCampeonatoDto>> _updateValidatorMock;
        private readonly Mock<ILogger<CampeonatosController>> _loggerMock;
        private readonly CampeonatosController _controller;

        public CampeonatosControllerTests()
        {
            _campeonatoServiceMock = new Mock<ICampeonatoService>();
            _createValidatorMock = new Mock<IValidator<CreateCampeonatoDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdateCampeonatoDto>>();
            _loggerMock = new Mock<ILogger<CampeonatosController>>();

            _controller = new CampeonatosController(
                _campeonatoServiceMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object);
        }

        #region GetCampeonatoById Tests

        [Fact]
        public async Task GetCampeonatoById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new CampeonatoResponseDto { Id = id, Titulo = "Campeonato Test" };
            _campeonatoServiceMock.Setup(x => x.GetCampeonatoByIdAsync(id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetCampeonatoById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<CampeonatoResponseDto>(okResult.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task GetCampeonatoById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _campeonatoServiceMock.Setup(x => x.GetCampeonatoByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Campeonato não encontrado"));

            // Act
            var result = await _controller.GetCampeonatoById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetCampeonatos Tests

        [Fact]
        public async Task GetCampeonatos_ReturnsOkWithPagedCampeonatos()
        {
            // Arrange
            var pagination = new FEMEE.Application.DTOs.Common.PaginationParams { Page = 1, PageSize = 10 };
            var campeonatos = new List<CampeonatoResponseDto>
            {
                new() { Id = 1, Titulo = "Campeonato 1" },
                new() { Id = 2, Titulo = "Campeonato 2" }
            };
            var pagedResult = new FEMEE.Application.DTOs.Common.PagedResult<CampeonatoResponseDto>
            {
                Items = campeonatos,
                TotalCount = 2,
                Page = 1,
                PageSize = 10
            };
            _campeonatoServiceMock.Setup(x => x.GetCampeonatosPagedAsync(It.IsAny<FEMEE.Application.DTOs.Common.PaginationParams>(), null)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetCampeonatos(pagination, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<FEMEE.Application.DTOs.Common.PagedResult<CampeonatoResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Items.Count());
        }

        #endregion

        #region GetCampeonatosByStatus Tests

        [Fact]
        public async Task GetCampeonatosByStatus_ReturnsOkWithFilteredCampeonatos()
        {
            // Arrange
            var status = StatusCampeonato.Open;
            var campeonatos = new List<CampeonatoResponseDto>
            {
                new() { Id = 1, Titulo = "Campeonato 1", Status = status }
            };
            _campeonatoServiceMock.Setup(x => x.GetCampeonatosByStatusAsync(status)).ReturnsAsync(campeonatos);

            // Act
            var result = await _controller.GetCampeonatosByStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<CampeonatoResponseDto>>(okResult.Value);
            Assert.Single(returned);
        }

        #endregion

        #region CreateCampeonato Tests

        [Fact]
        public async Task CreateCampeonato_WithValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new CreateCampeonatoDto { Titulo = "New Campeonato", JogoId = 1 };
            var created = new CampeonatoResponseDto { Id = 1, Titulo = dto.Titulo };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _campeonatoServiceMock.Setup(x => x.CreateCampeonatoAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.CreateCampeonato(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CampeonatosController.GetCampeonatoById), createdResult.ActionName);
        }

        [Fact]
        public async Task CreateCampeonato_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateCampeonatoDto { Titulo = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Titulo", "Título é obrigatório") });

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateCampeonato(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateCampeonato_WithInvalidOperation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateCampeonatoDto { Titulo = "Campeonato", JogoId = 999 };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _campeonatoServiceMock.Setup(x => x.CreateCampeonatoAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Jogo não encontrado"));

            // Act
            var result = await _controller.CreateCampeonato(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdateCampeonato Tests

        [Fact]
        public async Task UpdateCampeonato_WithValidData_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateCampeonatoDto { Titulo = "Updated Campeonato" };
            var updated = new CampeonatoResponseDto { Id = id, Titulo = dto.Titulo };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _campeonatoServiceMock.Setup(x => x.UpdateCampeonatoAsync(id, dto)).ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdateCampeonato(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<CampeonatoResponseDto>(okResult.Value);
            Assert.Equal(dto.Titulo, returned.Titulo);
        }

        [Fact]
        public async Task UpdateCampeonato_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateCampeonatoDto { Titulo = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Titulo", "Título inválido") });

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.UpdateCampeonato(id, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCampeonato_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var dto = new UpdateCampeonatoDto { Titulo = "Updated" };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _campeonatoServiceMock.Setup(x => x.UpdateCampeonatoAsync(id, dto))
                .ThrowsAsync(new KeyNotFoundException("Campeonato não encontrado"));

            // Act
            var result = await _controller.UpdateCampeonato(id, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeleteCampeonato Tests

        [Fact]
        public async Task DeleteCampeonato_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _campeonatoServiceMock.Setup(x => x.DeleteCampeonatoAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCampeonato(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCampeonato_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _campeonatoServiceMock.Setup(x => x.DeleteCampeonatoAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Campeonato não encontrado"));

            // Act
            var result = await _controller.DeleteCampeonato(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
