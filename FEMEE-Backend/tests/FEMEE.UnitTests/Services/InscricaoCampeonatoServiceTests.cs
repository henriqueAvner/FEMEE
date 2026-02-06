using AutoMapper;
using FEMEE.Application.DTOs.InscricaoCampeonato;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Services;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Services
{
    public class InscricaoCampeonatoServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<InscricaoCampeonatoService>> _mockLogger;
        private readonly Mock<IRepository<InscricaoCampeonato>> _mockInscricaoRepository;
        private readonly Mock<ICampeonatoRepository> _mockCampeonatoRepository;
        private readonly InscricaoCampeonatoService _service;

        public InscricaoCampeonatoServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<InscricaoCampeonatoService>>();
            _mockInscricaoRepository = new Mock<IRepository<InscricaoCampeonato>>();
            _mockCampeonatoRepository = new Mock<ICampeonatoRepository>();

            _mockUnitOfWork.Setup(u => u.InscricoesCampeonato).Returns(_mockInscricaoRepository.Object);
            _mockUnitOfWork.Setup(u => u.Campeonatos).Returns(_mockCampeonatoRepository.Object);

            _service = new InscricaoCampeonatoService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetInscricaoByIdAsync_WhenExists_ReturnsInscricaoResponseDto()
        {
            // Arrange
            var inscricao = new InscricaoCampeonato { Id = 1, CampeonatoId = 1, StatusInscricao = StatusInscricao.Pendente };
            var expectedDto = new InscricaoCampeonatoResponseDto { Id = 1 };

            _mockInscricaoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(inscricao);
            _mockMapper.Setup(m => m.Map<InscricaoCampeonatoResponseDto>(inscricao)).Returns(expectedDto);

            // Act
            var result = await _service.GetInscricaoByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetInscricaoByIdAsync_WhenNotExists_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockInscricaoRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((InscricaoCampeonato?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetInscricaoByIdAsync(999));
        }

        [Fact]
        public async Task GetAllInscricoesAsync_ReturnsAllInscricoes()
        {
            // Arrange
            var inscricoes = new List<InscricaoCampeonato>
            {
                new InscricaoCampeonato { Id = 1, CampeonatoId = 1 },
                new InscricaoCampeonato { Id = 2, CampeonatoId = 1 }
            };
            var expectedDtos = new List<InscricaoCampeonatoResponseDto>
            {
                new InscricaoCampeonatoResponseDto { Id = 1 },
                new InscricaoCampeonatoResponseDto { Id = 2 }
            };

            _mockInscricaoRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(inscricoes);
            _mockMapper.Setup(m => m.Map<IEnumerable<InscricaoCampeonatoResponseDto>>(inscricoes)).Returns(expectedDtos);

            // Act
            var result = await _service.GetAllInscricoesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetInscricoesByStatusAsync_ReturnsFilteredInscricoes()
        {
            // Arrange
            var inscricoes = new List<InscricaoCampeonato>
            {
                new InscricaoCampeonato { Id = 1, StatusInscricao = StatusInscricao.Pendente },
                new InscricaoCampeonato { Id = 2, StatusInscricao = StatusInscricao.Aprovada }
            };

            _mockInscricaoRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(inscricoes);
            _mockMapper.Setup(m => m.Map<IEnumerable<InscricaoCampeonatoResponseDto>>(It.IsAny<IEnumerable<InscricaoCampeonato>>()))
                .Returns<IEnumerable<InscricaoCampeonato>>(i => i.Select(x => new InscricaoCampeonatoResponseDto { Id = x.Id }));

            // Act
            var result = await _service.GetInscricoesByStatusAsync(StatusInscricao.Pendente);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task AprovarInscricaoAsync_WhenPendente_ApprovesSuccessfully()
        {
            // Arrange
            var inscricao = new InscricaoCampeonato { Id = 1, CampeonatoId = 1, StatusInscricao = StatusInscricao.Pendente };
            var campeonato = new Campeonato { Id = 1, NumeroInscritos = 5 };
            var expectedDto = new InscricaoCampeonatoResponseDto { Id = 1 };

            _mockInscricaoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(inscricao);
            _mockCampeonatoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(campeonato);
            _mockMapper.Setup(m => m.Map<InscricaoCampeonatoResponseDto>(inscricao)).Returns(expectedDto);

            // Act
            var result = await _service.AprovarInscricaoAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusInscricao.Aprovada, inscricao.StatusInscricao);
            Assert.Equal(6, campeonato.NumeroInscritos);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AprovarInscricaoAsync_WhenNotPendente_ThrowsInvalidOperationException()
        {
            // Arrange
            var inscricao = new InscricaoCampeonato { Id = 1, StatusInscricao = StatusInscricao.Aprovada };
            _mockInscricaoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(inscricao);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AprovarInscricaoAsync(1));
        }

        [Fact]
        public async Task RejeitarInscricaoAsync_WhenPendente_RejectsSuccessfully()
        {
            // Arrange
            var inscricao = new InscricaoCampeonato { Id = 1, StatusInscricao = StatusInscricao.Pendente };
            var expectedDto = new InscricaoCampeonatoResponseDto { Id = 1 };

            _mockInscricaoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(inscricao);
            _mockMapper.Setup(m => m.Map<InscricaoCampeonatoResponseDto>(inscricao)).Returns(expectedDto);

            // Act
            var result = await _service.RejeitarInscricaoAsync(1, "Documentação incompleta");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusInscricao.Rejeitada, inscricao.StatusInscricao);
            Assert.Equal("Documentação incompleta", inscricao.Observacoes);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteInscricaoAsync_WhenExists_DeletesSuccessfully()
        {
            // Arrange
            var inscricao = new InscricaoCampeonato { Id = 1 };
            _mockInscricaoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(inscricao);

            // Act
            await _service.DeleteInscricaoAsync(1);

            // Assert
            _mockInscricaoRepository.Verify(r => r.DeleteAsync(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
