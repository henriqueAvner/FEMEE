using AutoMapper;
using FEMEE.Application.DTOs.Jogo;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Services;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Services
{
    public class JogoServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<JogoService>> _mockLogger;
        private readonly Mock<IRepository<Jogo>> _mockJogoRepository;
        private readonly JogoService _service;

        public JogoServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<JogoService>>();
            _mockJogoRepository = new Mock<IRepository<Jogo>>();

            _mockUnitOfWork.Setup(u => u.Jogos).Returns(_mockJogoRepository.Object);

            _service = new JogoService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetJogoByIdAsync_WhenJogoExists_ReturnsJogoResponseDto()
        {
            // Arrange
            var jogo = new Jogo { Id = 1, Nome = "CS2", Slug = "cs2", Ativo = true };
            var expectedDto = new JogoResponseDto { Id = 1, Nome = "CS2" };

            _mockJogoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(jogo);
            _mockMapper.Setup(m => m.Map<JogoResponseDto>(jogo)).Returns(expectedDto);

            // Act
            var result = await _service.GetJogoByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("CS2", result.Nome);
        }

        [Fact]
        public async Task GetJogoByIdAsync_WhenJogoNotExists_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockJogoRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Jogo?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetJogoByIdAsync(999));
        }

        [Fact]
        public async Task GetAllJogosAsync_ReturnsAllJogos()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                new Jogo { Id = 1, Nome = "CS2", Slug = "cs2" },
                new Jogo { Id = 2, Nome = "Valorant", Slug = "valorant" }
            };
            var expectedDtos = new List<JogoResponseDto>
            {
                new JogoResponseDto { Id = 1, Nome = "CS2" },
                new JogoResponseDto { Id = 2, Nome = "Valorant" }
            };

            _mockJogoRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(jogos);
            _mockMapper.Setup(m => m.Map<IEnumerable<JogoResponseDto>>(jogos)).Returns(expectedDtos);

            // Act
            var result = await _service.GetAllJogosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetJogosAtivosAsync_ReturnsOnlyActiveJogos()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                new Jogo { Id = 1, Nome = "CS2", Slug = "cs2", Ativo = true },
                new Jogo { Id = 2, Nome = "Valorant", Slug = "valorant", Ativo = false }
            };

            _mockJogoRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(jogos);
            _mockMapper.Setup(m => m.Map<IEnumerable<JogoResponseDto>>(It.IsAny<IEnumerable<Jogo>>()))
                .Returns<IEnumerable<Jogo>>(j => j.Select(x => new JogoResponseDto { Id = x.Id, Nome = x.Nome ?? string.Empty }));

            // Act
            var result = await _service.GetJogosAtivosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateJogoAsync_WithValidData_ReturnsCreatedJogo()
        {
            // Arrange
            var createDto = new CreateJogoDto { Nome = "CS2", Slug = "cs2", CategoriaJogo = CategoriaJogo.CounterStrike };
            var jogo = new Jogo { Id = 1, Nome = "CS2", Slug = "cs2" };
            var responseDto = new JogoResponseDto { Id = 1, Nome = "CS2" };

            _mockJogoRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Jogo>());
            _mockMapper.Setup(m => m.Map<Jogo>(createDto)).Returns(jogo);
            _mockJogoRepository.Setup(r => r.AddAsync(jogo)).ReturnsAsync(jogo);
            _mockMapper.Setup(m => m.Map<JogoResponseDto>(jogo)).Returns(responseDto);

            // Act
            var result = await _service.CreateJogoAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateJogoAsync_WithDuplicateSlug_ThrowsInvalidOperationException()
        {
            // Arrange
            var createDto = new CreateJogoDto { Nome = "CS2", Slug = "cs2", CategoriaJogo = CategoriaJogo.CounterStrike };
            var existingJogos = new List<Jogo> { new Jogo { Id = 1, Slug = "cs2" } };

            _mockJogoRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(existingJogos);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateJogoAsync(createDto));
        }

        [Fact]
        public async Task DeleteJogoAsync_WhenJogoExists_DeletesSuccessfully()
        {
            // Arrange
            var jogo = new Jogo { Id = 1, Nome = "CS2", Slug = "cs2" };
            _mockJogoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(jogo);

            // Act
            await _service.DeleteJogoAsync(1);

            // Assert
            _mockJogoRepository.Verify(r => r.DeleteAsync(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteJogoAsync_WhenJogoNotExists_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockJogoRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Jogo?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteJogoAsync(999));
        }
    }
}
