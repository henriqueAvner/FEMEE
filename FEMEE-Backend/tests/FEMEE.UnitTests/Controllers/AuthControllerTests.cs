using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.Auth;
using FEMEE.Application.DTOs.User;
using FEMEE.Application.Interfaces.Common;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Domain.Entities.Principal;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthenticationService> _authServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IValidator<LoginRequest>> _loginValidatorMock;
        private readonly Mock<IValidator<RegisterRequest>> _registerValidatorMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthenticationService>();
            _userServiceMock = new Mock<IUserService>();
            _loginValidatorMock = new Mock<IValidator<LoginRequest>>();
            _registerValidatorMock = new Mock<IValidator<RegisterRequest>>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            _controller = new AuthController(
                _authServiceMock.Object,
                _userServiceMock.Object,
                _loginValidatorMock.Object,
                _registerValidatorMock.Object,
                _passwordHasherMock.Object,
                _loggerMock.Object);
        }

        #region Login Tests

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new LoginRequest { Email = "test@email.com", Senha = "password123" };
            var user = new User { Id = 1, Email = "test@email.com", Nome = "Test User", Senha = "hashedPassword" };
            var token = "jwt-token-here";

            _loginValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.GetUserEntityByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Senha, user.Senha))
                .Returns(true);
            _authServiceMock.Setup(x => x.GenerateTokenAsync(user))
                .ReturnsAsync(token);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal(token, response.Token);
            Assert.Equal(user.Id, response.UserId);
            Assert.Equal(user.Email, response.Email);
        }

        [Fact]
        public async Task Login_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest { Email = "", Senha = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Email", "Email é obrigatório") });

            _loginValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest { Email = "test@email.com", Senha = "wrongpassword" };
            var user = new User { Id = 1, Email = "test@email.com", Senha = "hashedPassword" };

            _loginValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.GetUserEntityByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Senha, user.Senha))
                .Returns(false);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest { Email = "nonexistent@email.com", Senha = "password" };

            _loginValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.GetUserEntityByEmailAsync(request.Email))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest { Email = null, Senha = null };

            _loginValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region Register Tests

        [Fact]
        public async Task Register_WithValidData_ReturnsCreated()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Nome = "New User",
                Email = "new@email.com",
                Senha = "password123",
                Telefone = "11999999999"
            };
            var userDto = new UserResponseDto { Id = 1, Nome = request.Nome, Email = request.Email };

            _registerValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<CreateUserDto>()))
                .ReturnsAsync(userDto);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(AuthController.GetCurrentUser), createdResult.ActionName);
        }

        [Fact]
        public async Task Register_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest { Nome = "", Email = "", Senha = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Nome", "Nome é obrigatório") });

            _registerValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Nome = "New User",
                Email = "existing@email.com",
                Senha = "password123"
            };

            _registerValidatorMock.Setup(x => x.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<CreateUserDto>()))
                .ThrowsAsync(new InvalidOperationException("Email já existe"));

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion
    }
}
