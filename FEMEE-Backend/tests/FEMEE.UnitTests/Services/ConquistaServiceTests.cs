using AutoMapper;
using FEMEE.Application.DTOs.Conquista;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Services;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Services
{
    public class ConquistaServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ConquistaService>> _mockLogger;
        private readonly Mock<IRepository<Conquista>> _mockConquistaRepository;
        private readonly Mock<ITimeRepository> _mockTimeRepository;
        private readonly Mock<ICampeonatoRepository> _mockCampeonatoRepository;
        private readonly ConquistaService _service;

        public ConquistaServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ConquistaService>>();
            _mockConquistaRepository = new Mock<IRepository<Conquista>>();
            _mockTimeRepository = new Mock<ITimeRepository>();
            _mockCampeonatoRepository = new Mock<ICampeonatoRepository>();

            _mockUnitOfWork.Setup(u => u.Conquistas).Returns(_mockConquistaRepository.Object);
            _mockUnitOfWork.Setup(u => u.Times).Returns(_mockTimeRepository.Object);
            _mockUnitOfWork.Setup(u => u.Campeonatos).Returns(_mockCampeonatoRepository.Object);

            _service = new ConquistaService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetConquistaByIdAsync_WhenExists_ReturnsConquistaResponseDto()
        {
            // Arrange
            var conquista = new Conquista { Id = 1, Titulo = "Campeão", TimeId = 1 };
            var expectedDto = new ConquistaResponseDto { Id = 1, Titulo = "Campeão" };

            _mockConquistaRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(conquista);
            _mockMapper.Setup(m => m.Map<ConquistaResponseDto>(conquista)).Returns(expectedDto);

            // Act
            var result = await _service.GetConquistaByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Campeão", result.Titulo);
        }

        [Fact]
        public async Task GetConquistaByIdAsync_WhenNotExists_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockConquistaRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Conquista?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetConquistaByIdAsync(999));
        }

        [Fact]
        public async Task GetAllConquistasAsync_ReturnsAllConquistas()
        {
            // Arrange
            var conquistas = new List<Conquista>
            {
                new Conquista { Id = 1, Titulo = "Campeão", TimeId = 1 },
                new Conquista { Id = 2, Titulo = "Vice", TimeId = 2 }
            };
            var expectedDtos = new List<ConquistaResponseDto>
            {
                new ConquistaResponseDto { Id = 1, Titulo = "Campeão" },
                new ConquistaResponseDto { Id = 2, Titulo = "Vice" }
            };

            _mockConquistaRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(conquistas);
            _mockMapper.Setup(m => m.Map<IEnumerable<ConquistaResponseDto>>(conquistas)).Returns(expectedDtos);

            // Act
            var result = await _service.GetAllConquistasAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetConquistasByTimeIdAsync_ReturnsConquistasForTime()
        {
            // Arrange
            var conquistas = new List<Conquista>
            {
                new Conquista { Id = 1, Titulo = "Campeão", TimeId = 1 },
                new Conquista { Id = 2, Titulo = "Vice", TimeId = 2 }
            };

            _mockConquistaRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(conquistas);
            _mockMapper.Setup(m => m.Map<IEnumerable<ConquistaResponseDto>>(It.IsAny<IEnumerable<Conquista>>()))
                .Returns<IEnumerable<Conquista>>(c => c.Select(x => new ConquistaResponseDto { Id = x.Id, Titulo = x.Titulo ?? string.Empty }));

            // Act
            var result = await _service.GetConquistasByTimeIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateConquistaAsync_WithValidData_ReturnsCreatedConquista()
        {
            // Arrange
            var createDto = new CreateConquistaDto { Titulo = "Campeão", TimeId = 1, Posicao = 1 };
            var time = new Time { Id = 1, Nome = "Team Alpha" };
            var conquista = new Conquista { Id = 1, Titulo = "Campeão", TimeId = 1 };
            var responseDto = new ConquistaResponseDto { Id = 1, Titulo = "Campeão" };

            _mockTimeRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(time);
            _mockMapper.Setup(m => m.Map<Conquista>(createDto)).Returns(conquista);
            _mockConquistaRepository.Setup(r => r.AddAsync(conquista)).ReturnsAsync(conquista);
            _mockMapper.Setup(m => m.Map<ConquistaResponseDto>(conquista)).Returns(responseDto);

            // Act
            var result = await _service.CreateConquistaAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateConquistaAsync_WithInvalidTime_ThrowsInvalidOperationException()
        {
            // Arrange
            var createDto = new CreateConquistaDto { Titulo = "Campeão", TimeId = 999, Posicao = 1 };

            _mockTimeRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Time?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateConquistaAsync(createDto));
        }

        [Fact]
        public async Task DeleteConquistaAsync_WhenExists_DeletesSuccessfully()
        {
            // Arrange
            var conquista = new Conquista { Id = 1, Titulo = "Campeão", TimeId = 1 };
            _mockConquistaRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(conquista);

            // Act
            await _service.DeleteConquistaAsync(1);

            // Assert
            _mockConquistaRepository.Verify(r => r.DeleteAsync(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
