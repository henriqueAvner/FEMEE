using FEMEE.API.Controllers;
using FEMEE.Application.DTOs.Time;
using FEMEE.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Controllers
{
    public class TimesControllerTests
    {
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly Mock<IValidator<CreateTimeDto>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateTimeDto>> _updateValidatorMock;
        private readonly Mock<ILogger<TimesController>> _loggerMock;
        private readonly TimesController _controller;

        public TimesControllerTests()
        {
            _timeServiceMock = new Mock<ITimeService>();
            _createValidatorMock = new Mock<IValidator<CreateTimeDto>>();
            _updateValidatorMock = new Mock<IValidator<UpdateTimeDto>>();
            _loggerMock = new Mock<ILogger<TimesController>>();

            _controller = new TimesController(
                _timeServiceMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object);
        }

        #region GetTimeById Tests

        [Fact]
        public async Task GetTimeById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var timeId = 1;
            var TimeResponseDto = new TimeResponseDto { Id = timeId, Nome = "Test Team", Slug = "test-team" };
            _timeServiceMock.Setup(x => x.GetTimeByIdAsync(timeId)).ReturnsAsync(TimeResponseDto);

            // Act
            var result = await _controller.GetTimeById(timeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTime = Assert.IsType<TimeResponseDto>(okResult.Value);
            Assert.Equal(timeId, returnedTime.Id);
        }

        [Fact]
        public async Task GetTimeById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var timeId = 999;
            _timeServiceMock.Setup(x => x.GetTimeByIdAsync(timeId))
                .ThrowsAsync(new KeyNotFoundException("Time não encontrado"));

            // Act
            var result = await _controller.GetTimeById(timeId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetTimeBySlug Tests

        [Fact]
        public async Task GetTimeBySlug_WithExistingSlug_ReturnsOk()
        {
            // Arrange
            var slug = "test-team";
            var TimeResponseDto = new TimeResponseDto { Id = 1, Nome = "Test Team", Slug = slug };
            _timeServiceMock.Setup(x => x.GetTimeBySlugAsync(slug)).ReturnsAsync(TimeResponseDto);

            // Act
            var result = await _controller.GetTimeBySlug(slug);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTime = Assert.IsType<TimeResponseDto>(okResult.Value);
            Assert.Equal(slug, returnedTime.Slug);
        }

        [Fact]
        public async Task GetTimeBySlug_WithNonExistingSlug_ReturnsNotFound()
        {
            // Arrange
            var slug = "non-existing";
            _timeServiceMock.Setup(x => x.GetTimeBySlugAsync(slug))
                .ThrowsAsync(new KeyNotFoundException("Time não encontrado"));

            // Act
            var result = await _controller.GetTimeBySlug(slug);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region GetTimes Tests

        [Fact]
        public async Task GetTimes_ReturnsOkWithPagedTimes()
        {
            // Arrange
            var pagination = new FEMEE.Application.DTOs.Common.PaginationParams { Page = 1, PageSize = 10 };
            var times = new List<TimeResponseDto>
            {
                new() { Id = 1, Nome = "Team 1", Slug = "team-1" },
                new() { Id = 2, Nome = "Team 2", Slug = "team-2" }
            };
            var pagedResult = new FEMEE.Application.DTOs.Common.PagedResult<TimeResponseDto>
            {
                Items = times,
                TotalCount = 2,
                Page = 1,
                PageSize = 10
            };
            _timeServiceMock.Setup(x => x.GetTimesPagedAsync(It.IsAny<FEMEE.Application.DTOs.Common.PaginationParams>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetTimes(pagination);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<FEMEE.Application.DTOs.Common.PagedResult<TimeResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedResult.Items.Count());
        }

        #endregion

        #region GetTimesByRanking Tests

        [Fact]
        public async Task GetTimesByRanking_ReturnsOkWithSortedTimes()
        {
            // Arrange
            var times = new List<TimeResponseDto>
            {
                new() { Id = 1, Nome = "Team 1", Vitorias = 10 },
                new() { Id = 2, Nome = "Team 2", Vitorias = 5 }
            };
            _timeServiceMock.Setup(x => x.GetTimesByRankingAsync()).ReturnsAsync(times);

            // Act
            var result = await _controller.GetTimesByRanking();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTimes = Assert.IsType<List<TimeResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedTimes.Count);
        }

        #endregion

        #region CreateTime Tests

        [Fact]
        public async Task CreateTime_WithValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new CreateTimeDto { Nome = "New Team", Slug = "new-team" };
            var createdTime = new TimeResponseDto { Id = 1, Nome = dto.Nome, Slug = dto.Slug };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _timeServiceMock.Setup(x => x.CreateTimeAsync(dto)).ReturnsAsync(createdTime);

            // Act
            var result = await _controller.CreateTime(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TimesController.GetTimeById), createdResult.ActionName);
        }

        [Fact]
        public async Task CreateTime_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateTimeDto { Nome = "", Slug = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Nome", "Nome é obrigatório") });

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateTime(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateTime_WithDuplicateSlug_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateTimeDto { Nome = "Team", Slug = "existing-slug" };

            _createValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _timeServiceMock.Setup(x => x.CreateTimeAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Slug já existe"));

            // Act
            var result = await _controller.CreateTime(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region UpdateTime Tests

        [Fact]
        public async Task UpdateTime_WithValidData_ReturnsOk()
        {
            // Arrange
            var timeId = 1;
            var dto = new UpdateTimeDto { Nome = "Updated Team" };
            var updatedTime = new TimeResponseDto { Id = timeId, Nome = dto.Nome };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _timeServiceMock.Setup(x => x.UpdateTimeAsync(timeId, dto)).ReturnsAsync(updatedTime);

            // Act
            var result = await _controller.UpdateTime(timeId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTime = Assert.IsType<TimeResponseDto>(okResult.Value);
            Assert.Equal(dto.Nome, returnedTime.Nome);
        }

        [Fact]
        public async Task UpdateTime_WithInvalidValidation_ReturnsBadRequest()
        {
            // Arrange
            var timeId = 1;
            var dto = new UpdateTimeDto { Nome = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Nome", "Nome inválido") });

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.UpdateTime(timeId, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTime_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var timeId = 999;
            var dto = new UpdateTimeDto { Nome = "Updated" };

            _updateValidatorMock.Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());
            _timeServiceMock.Setup(x => x.UpdateTimeAsync(timeId, dto))
                .ThrowsAsync(new KeyNotFoundException("Time não encontrado"));

            // Act
            var result = await _controller.UpdateTime(timeId, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region DeleteTime Tests

        [Fact]
        public async Task DeleteTime_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var timeId = 1;
            _timeServiceMock.Setup(x => x.DeleteTimeAsync(timeId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTime(timeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTime_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var timeId = 999;
            _timeServiceMock.Setup(x => x.DeleteTimeAsync(timeId))
                .ThrowsAsync(new KeyNotFoundException("Time não encontrado"));

            // Act
            var result = await _controller.DeleteTime(timeId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
