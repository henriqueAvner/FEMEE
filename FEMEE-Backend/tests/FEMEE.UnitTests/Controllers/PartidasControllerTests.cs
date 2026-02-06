using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.Partida;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class PartidasControllerTests
    {
        private readonly Mock<IPartidaService> _partidaServiceMock;
        private readonly Mock<IValidator<CreatePartidaDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdatePartidaDto>> _updateValidatorMock;
        private readonly Mock<ILogger<PartidasController>> _loggerMock;
        private readonly PartidasController _controller;

        public PartidasControllerTests()
        {
            _partidaServiceMock = new Mock<IPartidaService>();
            _createValidatorMock = new Mock<IValidator<CreatePartidaDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdatePartidaDto>>();
            _loggerMock = new Mock<ILogger<PartidasController>>();

            _controller = new PartidasController(
                _partidaServiceMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object);
        }

        #region GetPartidaById Tests

        [Fact]
        public async Task GetPartidaById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new PartidaResponseDto { Id = id };
            _partidaServiceMock.Setup(x => x.GetPartidaByIdAsync(id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetPartidaById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<PartidaResponseDto>(okResult.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task GetPartidaById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _partidaServiceMock.Setup(x => x.GetPartidaByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Partida não encontrada"));

            // Act
            var result = await _controller.GetPartidaById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetAllPartidas Tests

        [Fact]
        public async Task GetAllPartidas_ReturnsOkWithPartidas()
        {
            // Arrange
            var partidas = new List<PartidaResponseDto>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            };
            _partidaServiceMock.Setup(x => x.GetAllPartidasAsync()).ReturnsAsync(partidas);

            // Act
            var result = await _controller.GetAllPartidas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<PartidaResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        #endregion

        #region GetPartidasByCampeonato Tests

        [Fact]
        public async Task GetPartidasByCampeonato_ReturnsOkWithFilteredPartidas()
        {
            // Arrange
            var campeonatoId = 1;
            var partidas = new List<PartidaResponseDto>
            {
                new() { Id = 1, CampeonatoId = campeonatoId },
                new() { Id = 2, CampeonatoId = campeonatoId }
            };
            _partidaServiceMock.Setup(x => x.GetPartidasByCampeonatoAsync(campeonatoId)).ReturnsAsync(partidas);

            // Act
            var result = await _controller.GetPartidasByCampeonato(campeonatoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<PartidaResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        #endregion

        #region GetPartidasByTime Tests

        [Fact]
        public async Task GetPartidasByTime_ReturnsOkWithFilteredPartidas()
        {
            // Arrange
            var timeId = 1;
            var partidas = new List<PartidaResponseDto>
            {
                new() { Id = 1, CampeonatoId = 1 },
                new() { Id = 2, CampeonatoId = 1 }
            };
            _partidaServiceMock.Setup(x => x.GetPartidasByTimeAsync(timeId)).ReturnsAsync(partidas);

            // Act
            var result = await _controller.GetPartidasByTime(timeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<PartidaResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        #endregion

        #region CreatePartida Tests

        [Fact]
        public async Task CreatePartida_WithValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new CreatePartidaDto { CampeonatoId = 1, TimeAId = 1, TimeBId = 2 };
            var created = new PartidaResponseDto { Id = 1, CampeonatoId = dto.CampeonatoId };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _partidaServiceMock.Setup(x => x.CreatePartidaAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.CreatePartida(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(PartidasController.GetPartidaById), createdResult.ActionName);
        }

        [Fact]
        public async Task CreatePartida_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreatePartidaDto { CampeonatoId = 0 };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("CampeonatoId", "CampeonatoId é obrigatório") });

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreatePartida(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreatePartida_WithInvalidOperation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreatePartidaDto { CampeonatoId = 1, TimeAId = 1, TimeBId = 1 };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _partidaServiceMock.Setup(x => x.CreatePartidaAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Times devem ser diferentes"));

            // Act
            var result = await _controller.CreatePartida(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdatePartida Tests

        [Fact]
        public async Task UpdatePartida_WithValidData_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new UpdatePartidaDto { DataHora = DateTime.UtcNow.AddDays(1) };
            var updated = new PartidaResponseDto { Id = id, DataHora = dto.DataHora };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _partidaServiceMock.Setup(x => x.UpdatePartidaAsync(id, dto)).ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdatePartida(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<PartidaResponseDto>(okResult.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task UpdatePartida_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var dto = new UpdatePartidaDto();
            var validationResult = new ValidationResult(new[] { new ValidationFailure("DataHora", "DataHora inválida") });

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.UpdatePartida(id, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePartida_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var dto = new UpdatePartidaDto { DataHora = DateTime.UtcNow };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _partidaServiceMock.Setup(x => x.UpdatePartidaAsync(id, dto))
                .ThrowsAsync(new KeyNotFoundException("Partida não encontrada"));

            // Act
            var result = await _controller.UpdatePartida(id, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeletePartida Tests

        [Fact]
        public async Task DeletePartida_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _partidaServiceMock.Setup(x => x.DeletePartidaAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletePartida(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePartida_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _partidaServiceMock.Setup(x => x.DeletePartidaAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Partida não encontrada"));

            // Act
            var result = await _controller.DeletePartida(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
