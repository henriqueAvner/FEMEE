using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.Jogador;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class JogadoresControllerTests
    {
        private readonly Mock<IJogadorService> _jogadorServiceMock;
        private readonly Mock<IValidator<CreateJogadorDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateJogadorDto>> _updateValidatorMock;
        private readonly Mock<ILogger<JogadoresController>> _loggerMock;
        private readonly JogadoresController _controller;

        public JogadoresControllerTests()
        {
            _jogadorServiceMock = new Mock<IJogadorService>();
            _createValidatorMock = new Mock<IValidator<CreateJogadorDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdateJogadorDto>>();
            _loggerMock = new Mock<ILogger<JogadoresController>>();

            _controller = new JogadoresController(
                _jogadorServiceMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object);
        }

        #region GetJogadorById Tests

        [Fact]
        public async Task GetJogadorById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new JogadorResponseDto { Id = id, NickName = "TestPlayer" };
            _jogadorServiceMock.Setup(x => x.GetJogadorByIdAsync(id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetJogadorById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<JogadorResponseDto>(okResult.Value);
            Assert.Equal(id, returned.Id);
        }

        [Fact]
        public async Task GetJogadorById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _jogadorServiceMock.Setup(x => x.GetJogadorByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Jogador não encontrado"));

            // Act
            var result = await _controller.GetJogadorById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetAllJogadores Tests

        [Fact]
        public async Task GetAllJogadores_ReturnsOkWithJogadores()
        {
            // Arrange
            var jogadores = new List<JogadorResponseDto>
            {
                new() { Id = 1, NickName = "Player1" },
                new() { Id = 2, NickName = "Player2" }
            };
            _jogadorServiceMock.Setup(x => x.GetAllJogadoresAsync()).ReturnsAsync(jogadores);

            // Act
            var result = await _controller.GetAllJogadores();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<JogadorResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        #endregion

        #region GetJogadoresByTime Tests

        [Fact]
        public async Task GetJogadoresByTime_ReturnsOkWithFilteredJogadores()
        {
            // Arrange
            var timeId = 1;
            var jogadores = new List<JogadorResponseDto>
            {
                new() { Id = 1, NickName = "Player1" },
                new() { Id = 2, NickName = "Player2" }
            };
            _jogadorServiceMock.Setup(x => x.GetJogadoresByTimeAsync(timeId)).ReturnsAsync(jogadores);

            // Act
            var result = await _controller.GetJogadoresByTime(timeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<JogadorResponseDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        #endregion

        #region CreateJogador Tests

        [Fact]
        public async Task CreateJogador_WithValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new CreateJogadorDto { NickName = "NewPlayer", UserId = 1 };
            var created = new JogadorResponseDto { Id = 1, NickName = dto.NickName };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _jogadorServiceMock.Setup(x => x.CreateJogadorAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.CreateJogador(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(JogadoresController.GetJogadorById), createdResult.ActionName);
        }

        [Fact]
        public async Task CreateJogador_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateJogadorDto { NickName = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("NickName", "NickName é obrigatório") });

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateJogador(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateJogador_WithDuplicateNickName_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateJogadorDto { NickName = "ExistingPlayer", UserId = 1 };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _jogadorServiceMock.Setup(x => x.CreateJogadorAsync(dto))
                .ThrowsAsync(new InvalidOperationException("NickName já existe"));

            // Act
            var result = await _controller.CreateJogador(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdateJogador Tests

        [Fact]
        public async Task UpdateJogador_WithValidData_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateJogadorDto { NickName = "UpdatedPlayer" };
            var updated = new JogadorResponseDto { Id = id, NickName = dto.NickName };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _jogadorServiceMock.Setup(x => x.UpdateJogadorAsync(id, dto)).ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdateJogador(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<JogadorResponseDto>(okResult.Value);
            Assert.Equal(dto.NickName, returned.NickName);
        }

        [Fact]
        public async Task UpdateJogador_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateJogadorDto { NickName = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("NickName", "NickName inválido") });

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.UpdateJogador(id, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateJogador_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var dto = new UpdateJogadorDto { NickName = "Updated" };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _jogadorServiceMock.Setup(x => x.UpdateJogadorAsync(id, dto))
                .ThrowsAsync(new KeyNotFoundException("Jogador não encontrado"));

            // Act
            var result = await _controller.UpdateJogador(id, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeleteJogador Tests

        [Fact]
        public async Task DeleteJogador_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _jogadorServiceMock.Setup(x => x.DeleteJogadorAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteJogador(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteJogador_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _jogadorServiceMock.Setup(x => x.DeleteJogadorAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Jogador não encontrado"));

            // Act
            var result = await _controller.DeleteJogador(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
