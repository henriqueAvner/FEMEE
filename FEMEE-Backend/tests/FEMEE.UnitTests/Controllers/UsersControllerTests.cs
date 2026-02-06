using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.User;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IValidator<CreateUserDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateUserDto>> _updateValidatorMock;
        private readonly Mock<ILogger<UsersController>> _loggerMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _createValidatorMock = new Mock<IValidator<CreateUserDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdateUserDto>>();
            _loggerMock = new Mock<ILogger<UsersController>>();

            _controller = new UsersController(
                _userServiceMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object);
        }

        #region GetUserById Tests

        [Fact]
        public async Task GetUserById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var userId = 1;
            var UserResponseDto = new UserResponseDto { Id = userId, Nome = "Test User", Email = "test@email.com" };
            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(UserResponseDto);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId))
                .ThrowsAsync(new KeyNotFoundException("Usuário não encontrado"));

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetAllUsers Tests

        [Fact]
        public async Task GetAllUsers_ReturnsOkWithUsers()
        {
            // Arrange
            var users = new List<UserResponseDto>
            {
                new() { Id = 1, Nome = "User 1", Email = "user1@email.com" },
                new() { Id = 2, Nome = "User 2", Email = "user2@email.com" }
            };
            _userServiceMock.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        #endregion

        #region CreateUser Tests

        [Fact]
        public async Task CreateUser_WithValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new CreateUserDto { Nome = "New User", Email = "new@email.com", Senha = "password123" };
            var createdUser = new UserResponseDto { Id = 1, Nome = dto.Nome, Email = dto.Email };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.CreateUserAsync(dto)).ReturnsAsync(createdUser);

            // Act
            var result = await _controller.CreateUser(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(UsersController.GetUserById), createdResult.ActionName);
            var returnedUser = Assert.IsType<UserResponseDto>(createdResult.Value);
            Assert.Equal(dto.Nome, returnedUser.Nome);
        }

        [Fact]
        public async Task CreateUser_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateUserDto { Nome = "", Email = "", Senha = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Nome", "Nome é obrigatório") });

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateUser(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateUserDto { Nome = "User", Email = "existing@email.com", Senha = "password" };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.CreateUserAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Email já existe"));

            // Act
            var result = await _controller.CreateUser(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdateUser Tests

        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsOk()
        {
            // Arrange
            var userId = 1;
            var dto = new UpdateUserDto { Nome = "Updated User" };
            var updatedUser = new UserResponseDto { Id = userId, Nome = dto.Nome, Email = "test@email.com" };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.UpdateUserAsync(userId, dto)).ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.UpdateUser(userId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(dto.Nome, returnedUser.Nome);
        }

        [Fact]
        public async Task UpdateUser_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var dto = new UpdateUserDto { Nome = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Nome", "Nome inválido") });

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.UpdateUser(userId, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateUser_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            var dto = new UpdateUserDto { Nome = "Updated" };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.UpdateUserAsync(userId, dto))
                .ThrowsAsync(new KeyNotFoundException("Usuário não encontrado"));

            // Act
            var result = await _controller.UpdateUser(userId, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeleteUser Tests

        [Fact]
        public async Task DeleteUser_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(x => x.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            _userServiceMock.Setup(x => x.DeleteUserAsync(userId))
                .ThrowsAsync(new KeyNotFoundException("Usuário não encontrado"));

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
