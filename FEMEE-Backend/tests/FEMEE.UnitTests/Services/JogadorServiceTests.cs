using AutoMapper;
using FEMEE.Application.DTOs.Jogador;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Mappings;
using FEMEE.Application.Services;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Services;

/// <summary>
/// Testes unitários para JogadorService.
/// </summary>
public class JogadorServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IJogadorRepository> _jogadorRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<JogadorService>> _loggerMock;
    private readonly JogadorService _sut;

    public JogadorServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _jogadorRepositoryMock = new Mock<IJogadorRepository>();
        _loggerMock = new Mock<ILogger<JogadorService>>();

        _unitOfWorkMock.Setup(u => u.Jogadores).Returns(_jogadorRepositoryMock.Object);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _sut = new JogadorService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object);
    }

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsJogador()
    {
        // Arrange
        var jogadorId = 1;
        var jogador = CreateSampleJogador(jogadorId);
        _jogadorRepositoryMock.Setup(r => r.GetByIdAsync(jogadorId)).ReturnsAsync(jogador);

        // Act
        var result = await _sut.GetJogadorByIdAsync(jogadorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jogadorId, result.Id);
        Assert.Equal(jogador.NickName, result.NickName);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidId = 999;
        _jogadorRepositoryMock.Setup(r => r.GetByIdAsync(invalidId)).ReturnsAsync((Jogador?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetJogadorByIdAsync(invalidId));
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllJogadores()
    {
        // Arrange
        var jogadores = new List<Jogador>
        {
            CreateSampleJogador(1, "Player1"),
            CreateSampleJogador(2, "Player2"),
            CreateSampleJogador(3, "Player3")
        };
        _jogadorRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(jogadores);

        // Act
        var result = await _sut.GetAllJogadoresAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsNewJogador()
    {
        // Arrange
        var createDto = new CreateJogadorDto
        {
            UserId = 1,
            NickName = "NovoPlayer",
            Funcao = FuncaoJogador.Jogador,
            TimeId = null
        };

        _jogadorRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Jogador>()))
            .ReturnsAsync((Jogador j) => { j.Id = 1; return j; });
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _sut.CreateJogadorAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.NickName, result.NickName);
        _jogadorRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Jogador>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WithValidData_UpdatesJogador()
    {
        // Arrange
        var jogadorId = 1;
        var existingJogador = CreateSampleJogador(jogadorId);
        var updateDto = new UpdateJogadorDto
        {
            NickName = "UpdatedNickname",
            Funcao = FuncaoJogador.Capitao
        };

        _jogadorRepositoryMock.Setup(r => r.GetByIdAsync(jogadorId)).ReturnsAsync(existingJogador);
        _jogadorRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Jogador>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _sut.UpdateJogadorAsync(jogadorId, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.NickName, result.NickName);
        _jogadorRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Jogador>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidId = 999;
        var updateDto = new UpdateJogadorDto { NickName = "Test" };
        _jogadorRepositoryMock.Setup(r => r.GetByIdAsync(invalidId)).ReturnsAsync((Jogador?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.UpdateJogadorAsync(invalidId, updateDto));
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesJogador()
    {
        // Arrange
        var jogadorId = 1;
        var existingJogador = CreateSampleJogador(jogadorId);
        _jogadorRepositoryMock.Setup(r => r.GetByIdAsync(jogadorId)).ReturnsAsync(existingJogador);
        _jogadorRepositoryMock.Setup(r => r.DeleteAsync(jogadorId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await _sut.DeleteJogadorAsync(jogadorId);

        // Assert
        _jogadorRepositoryMock.Verify(r => r.DeleteAsync(jogadorId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidId = 999;
        _jogadorRepositoryMock.Setup(r => r.GetByIdAsync(invalidId)).ReturnsAsync((Jogador?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteJogadorAsync(invalidId));
    }

    #endregion

    #region GetByTimeAsync Tests

    [Fact]
    public async Task GetByTimeAsync_WithValidTimeId_ReturnsJogadoresDoTime()
    {
        // Arrange
        var timeId = 1;
        var jogadores = new List<Jogador>
        {
            CreateSampleJogador(1, "Player1", timeId),
            CreateSampleJogador(2, "Player2", timeId),
            CreateSampleJogador(3, "Player3", 2) // Outro time
        };
        _jogadorRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(jogadores);

        // Act
        var result = await _sut.GetJogadoresByTimeAsync(timeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    #endregion

    #region Helper Methods

    private static Jogador CreateSampleJogador(int id, string nickname = "TestPlayer", int? timeId = null)
    {
        return new Jogador
        {
            Id = id,
            UserId = id,
            User = new User
            {
                Id = id,
                Nome = $"User {id}",
                Email = $"user{id}@example.com",
                Senha = "hashedpassword",
                Telefone = "11999999999",
                TipoUsuario = TipoUsuario.Jogador,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            },
            NickName = nickname,
            Funcao = FuncaoJogador.Jogador,
            Status = StatusJogador.Ativo,
            TimeId = timeId,
            DataEntradaTime = DateTime.UtcNow
        };
    }

    #endregion
}
