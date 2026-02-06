using FEMEE.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Middleware
{
    public class RequestLoggingMiddlewareTests
    {
        private readonly Mock<ILogger<RequestLoggingMiddleware>> _loggerMock;

        public RequestLoggingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<RequestLoggingMiddleware>>();
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Request.Method = "GET";
            context.Request.Path = "/api/test";
            context.Request.Scheme = "https";
            context.Request.Host = new HostString("localhost");
            return context;
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithNullNext_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new RequestLoggingMiddleware(null!, _loggerMock.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new RequestLoggingMiddleware(_ => Task.CompletedTask, null!));
        }

        #endregion

        #region Middleware Chain Tests

        [Fact]
        public async Task InvokeAsync_CallsNextMiddleware()
        {
            // Arrange
            var nextCalled = false;
            RequestDelegate next = _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };
            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(nextCalled);
        }

        [Fact]
        public async Task InvokeAsync_CompletesSuccessfullyWithResponse()
        {
            // Arrange
            RequestDelegate next = ctx =>
            {
                ctx.Response.StatusCode = 200;
                return Task.CompletedTask;
            };
            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();
            var originalBody = new MemoryStream();
            context.Response.Body = originalBody;

            // Act
            await middleware.InvokeAsync(context);

            // Assert - middleware completes successfully
            Assert.Equal(200, context.Response.StatusCode);
        }

        #endregion

        #region Logging Tests

        [Fact]
        public async Task InvokeAsync_LogsRequestInfo()
        {
            // Arrange
            RequestDelegate next = _ => Task.CompletedTask;
            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();
            context.Request.Method = "POST";
            context.Request.Path = "/api/users";

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Requisição iniciada")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_LogsResponseInfo()
        {
            // Arrange
            RequestDelegate next = ctx =>
            {
                ctx.Response.StatusCode = 200;
                return Task.CompletedTask;
            };
            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Requisição concluída")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        #endregion

        #region Exception Propagation Tests

        [Fact]
        public async Task InvokeAsync_WhenNextThrows_PropagatesException()
        {
            // Arrange
            var exception = new InvalidOperationException("Test exception");
            RequestDelegate next = _ => throw exception;
            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                middleware.InvokeAsync(context));
        }

        #endregion

        #region Status Code Tests

        [Fact]
        public async Task InvokeAsync_PreservesStatusCode()
        {
            // Arrange
            RequestDelegate next = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status201Created;
                return Task.CompletedTask;
            };
            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = CreateHttpContext();
            var originalBody = new MemoryStream();
            context.Response.Body = originalBody;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status201Created, context.Response.StatusCode);
        }

        #endregion
    }
}
