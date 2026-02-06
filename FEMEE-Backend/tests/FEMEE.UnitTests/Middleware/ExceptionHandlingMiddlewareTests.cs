using FEMEE.API.Middleware;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace FEMEE.UnitTests.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;

        public ExceptionHandlingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        private async Task<ErrorResponse?> GetResponseBody(HttpContext context)
        {
            context.Response.Body.Position = 0;
            using var reader = new StreamReader(context.Response.Body);
            var body = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<ErrorResponse>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithNullNext_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ExceptionHandlingMiddleware(null!, _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ExceptionHandlingMiddleware(_ => Task.CompletedTask, null!));
        }

        #endregion

        #region Success Path Tests

        [Fact]
        public async Task InvokeAsync_WhenNoException_CallsNextMiddleware()
        {
            // Arrange
            var nextCalled = false;
            RequestDelegate next = _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(nextCalled);
        }

        #endregion

        #region ValidationException Tests

        [Fact]
        public async Task InvokeAsync_WhenValidationException_Returns400BadRequest()
        {
            // Arrange
            var errors = new List<ValidationFailure>
            {
                new("Email", "Email é obrigatório"),
                new("Nome", "Nome é obrigatório")
            };
            var validationException = new ValidationException(errors);
            RequestDelegate next = _ => throw validationException;
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
            var response = await GetResponseBody(context);
            Assert.NotNull(response);
            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Erro de validação", response.Message);
            Assert.Contains("Email é obrigatório", response.Details);
        }

        #endregion

        #region UnauthorizedAccessException Tests

        [Fact]
        public async Task InvokeAsync_WhenUnauthorizedAccessException_Returns401Unauthorized()
        {
            // Arrange
            RequestDelegate next = _ => throw new UnauthorizedAccessException("Token inválido");
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
            var response = await GetResponseBody(context);
            Assert.NotNull(response);
            Assert.Equal(401, response.StatusCode);
            Assert.Equal("Não autorizado", response.Message);
        }

        #endregion

        #region KeyNotFoundException Tests

        [Fact]
        public async Task InvokeAsync_WhenKeyNotFoundException_Returns404NotFound()
        {
            // Arrange
            RequestDelegate next = _ => throw new KeyNotFoundException("Usuário não encontrado");
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
            var response = await GetResponseBody(context);
            Assert.NotNull(response);
            Assert.Equal(404, response.StatusCode);
        }

        #endregion

        #region Generic Exception Tests

        [Fact]
        public async Task InvokeAsync_WhenGenericException_Returns500InternalServerError()
        {
            // Arrange
            RequestDelegate next = _ => throw new Exception("Erro interno");
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            var response = await GetResponseBody(context);
            Assert.NotNull(response);
            Assert.Equal(500, response.StatusCode);
        }

        #endregion

        #region Response Format Tests

        [Fact]
        public async Task InvokeAsync_WhenException_ResponseHasJsonContentType()
        {
            // Arrange
            RequestDelegate next = _ => throw new Exception("Test");
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal("application/json", context.Response.ContentType);
        }

        [Fact]
        public async Task InvokeAsync_WhenException_ResponseHasTraceId()
        {
            // Arrange
            RequestDelegate next = _ => throw new Exception("Test");
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();
            context.TraceIdentifier = "test-trace-id";

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            var response = await GetResponseBody(context);
            Assert.NotNull(response);
            Assert.Equal("test-trace-id", response.TraceId);
        }

        [Fact]
        public async Task InvokeAsync_WhenException_ResponseHasTimestamp()
        {
            // Arrange
            RequestDelegate next = _ => throw new Exception("Test");
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            var response = await GetResponseBody(context);
            Assert.NotNull(response);
            Assert.True(response.Timestamp > DateTime.MinValue);
        }

        #endregion
    }
}
