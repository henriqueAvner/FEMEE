using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Store;
using FEMEE.Domain.Enums;

namespace FEMEE.UnitTests.Domain;

/// <summary>
/// Testes para as Entidades do Domain.
/// Verificam inicialização de coleções e propriedades de navegação.
/// </summary>
public class EntitiesTests
{
    // ========== USER ==========

    [Fact]
    public void User_ShouldInitializeCollections()
    {
        // Act
        var user = new User();

        // Assert
        Assert.NotNull(user.Noticias);
        Assert.NotNull(user.InscricoesCampeonatos);
        Assert.Empty(user.Noticias);
        Assert.Empty(user.InscricoesCampeonatos);
    }

    [Fact]
    public void User_ShouldHaveDefaultValues()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Null(user.Nome);
        Assert.Null(user.Email);
        Assert.Null(user.Senha);
        Assert.Null(user.Telefone);
        Assert.Equal(default(DateTime), user.DataCriacao);
        Assert.Equal(default(DateTime), user.DataAtualizacao);
        Assert.Equal(default(TipoUsuario), user.TipoUsuario);
    }

    [Fact]
    public void User_ShouldSetProperties()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "hashedpassword",
            Telefone = "11999999999",
            TipoUsuario = TipoUsuario.Administrador,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("Test User", user.Nome);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("hashedpassword", user.Senha);
        Assert.Equal("11999999999", user.Telefone);
        Assert.Equal(TipoUsuario.Administrador, user.TipoUsuario);
    }

    // ========== TIME ==========

    [Fact]
    public void Time_ShouldInitializeCollections()
    {
        // Act
        var time = new Time();

        // Assert
        Assert.NotNull(time.Jogadores);
        Assert.NotNull(time.PartidasCasaTeam);
        Assert.NotNull(time.PartidasVisitanteTeam);
        Assert.NotNull(time.InscricoesCampeonatos);
        Assert.NotNull(time.Conquistas);
    }

    [Fact]
    public void Time_ShouldHaveDefaultValues()
    {
        // Act
        var time = new Time();

        // Assert
        Assert.Equal(0, time.Id);
        Assert.Null(time.Nome);
        Assert.Null(time.Slug);
        Assert.Equal(0, time.TitulosTime);
        Assert.Equal(0, time.Vitorias);
        Assert.Equal(0, time.Derrotas);
        Assert.Equal(0, time.Empates);
        Assert.Equal(0, time.Pontos);
    }

    [Fact]
    public void Time_ShouldSetProperties()
    {
        // Arrange
        var time = new Time
        {
            Id = 1,
            Nome = "Test Team",
            Slug = "test-team",
            TitulosTime = 5,
            Vitorias = 10,
            Derrotas = 3,
            Empates = 2,
            Pontos = 32
        };

        // Assert
        Assert.Equal(1, time.Id);
        Assert.Equal("Test Team", time.Nome);
        Assert.Equal("test-team", time.Slug);
        Assert.Equal(5, time.TitulosTime);
        Assert.Equal(10, time.Vitorias);
        Assert.Equal(3, time.Derrotas);
        Assert.Equal(2, time.Empates);
        Assert.Equal(32, time.Pontos);
    }

    // ========== CAMPEONATO ==========

    [Fact]
    public void Campeonato_ShouldInitializeCollections()
    {
        // Act
        var campeonato = new Campeonato();

        // Assert
        Assert.NotNull(campeonato.Partidas);
        Assert.NotNull(campeonato.InscricoesCampeonatos);
        Assert.NotNull(campeonato.Conquistas);
    }

    [Fact]
    public void Campeonato_ShouldHaveDefaultValues()
    {
        // Act
        var campeonato = new Campeonato();

        // Assert
        Assert.Equal(0, campeonato.Id);
        Assert.Null(campeonato.Titulo);
        Assert.Null(campeonato.Descricao);
        Assert.Equal(0, campeonato.NumeroVagas);
        Assert.Equal(0, campeonato.NumeroInscritos);
        Assert.Equal(0m, campeonato.Premiacao);
    }

    [Fact]
    public void Campeonato_ShouldSetProperties()
    {
        // Arrange
        var dataInicio = DateTime.UtcNow;
        var dataFim = dataInicio.AddMonths(1);

        var campeonato = new Campeonato
        {
            Id = 1,
            Titulo = "Campeonato Test",
            Descricao = "Descrição do campeonato",
            DataInicio = dataInicio,
            DataFim = dataFim,
            NumeroVagas = 16,
            NumeroInscritos = 8,
            Premiacao = 10000m,
            Status = StatusCampeonato.Open,
            FaseCampeonato = Fase.Grupos
        };

        // Assert
        Assert.Equal(1, campeonato.Id);
        Assert.Equal("Campeonato Test", campeonato.Titulo);
        Assert.Equal(16, campeonato.NumeroVagas);
        Assert.Equal(10000m, campeonato.Premiacao);
        Assert.Equal(StatusCampeonato.Open, campeonato.Status);
        Assert.Equal(Fase.Grupos, campeonato.FaseCampeonato);
    }

    // ========== PARTIDA ==========

    [Fact]
    public void Partida_ShouldHaveDefaultValues()
    {
        // Act
        var partida = new Partida();

        // Assert
        Assert.Equal(0, partida.Id);
        Assert.Equal(0, partida.CampeonatoId);
        Assert.Equal(0, partida.TimeAId);
        Assert.Equal(0, partida.TimeBId);
        Assert.Null(partida.TimeVencedorId);
        Assert.Equal(0, partida.PlacarTimeA);
        Assert.Equal(0, partida.PlacarTimeB);
    }

    [Fact]
    public void Partida_ShouldSetProperties()
    {
        // Arrange
        var dataHora = DateTime.UtcNow;
        var partida = new Partida
        {
            Id = 1,
            CampeonatoId = 1,
            TimeAId = 1,
            TimeBId = 2,
            DataHora = dataHora,
            Local = "Arena Test",
            Fase = Fase.Semifinal,
            Status = StatusPartida.Agendada,
            PlacarTimeA = 2,
            PlacarTimeB = 1,
            TimeVencedorId = 1
        };

        // Assert
        Assert.Equal(1, partida.Id);
        Assert.Equal(1, partida.CampeonatoId);
        Assert.Equal(1, partida.TimeAId);
        Assert.Equal(2, partida.TimeBId);
        Assert.Equal("Arena Test", partida.Local);
        Assert.Equal(Fase.Semifinal, partida.Fase);
        Assert.Equal(StatusPartida.Agendada, partida.Status);
        Assert.Equal(2, partida.PlacarTimeA);
        Assert.Equal(1, partida.PlacarTimeB);
        Assert.Equal(1, partida.TimeVencedorId);
    }

    [Fact]
    public void Partida_NavigationProperties_ShouldBeNull_ByDefault()
    {
        // Act
        var partida = new Partida();

        // Assert
        Assert.Null(partida.Campeonato);
        Assert.Null(partida.TimeA);
        Assert.Null(partida.TimeB);
        Assert.Null(partida.TimeVencedor);
    }

    // ========== JOGADOR ==========

    [Fact]
    public void Jogador_ShouldHaveDefaultValues()
    {
        // Act
        var jogador = new Jogador();

        // Assert
        Assert.Equal(0, jogador.Id);
        // NickName é inicializado com null! (null forgiven pattern)
        Assert.Equal(default(FuncaoJogador), jogador.Funcao);
    }

    // ========== NOTICIA ==========

    [Fact]
    public void Noticia_ShouldHaveDefaultValues()
    {
        // Act
        var noticia = new Noticia();

        // Assert
        Assert.Equal(0, noticia.Id);
        Assert.Null(noticia.Titulo);
        Assert.Null(noticia.Conteudo);
    }

    // ========== PRODUTO ==========

    [Fact]
    public void Produto_ShouldHaveDefaultValues()
    {
        // Act
        var produto = new Produto();

        // Assert
        Assert.Equal(0, produto.Id);
        Assert.Null(produto.Nome);
        Assert.Equal(0m, produto.Preco);
    }

    // ========== CONQUISTA ==========

    [Fact]
    public void Conquista_ShouldHaveDefaultValues()
    {
        // Act
        var conquista = new Conquista();

        // Assert
        Assert.Equal(0, conquista.Id);
        Assert.Null(conquista.Titulo);
    }

    // ========== JOGO ==========

    [Fact]
    public void Jogo_ShouldHaveDefaultValues()
    {
        // Act
        var jogo = new Jogo();

        // Assert
        Assert.Equal(0, jogo.Id);
        Assert.Null(jogo.Nome);
    }

    // ========== INSCRICAO CAMPEONATO ==========

    [Fact]
    public void InscricaoCampeonato_ShouldHaveDefaultValues()
    {
        // Act
        var inscricao = new InscricaoCampeonato();

        // Assert
        Assert.Equal(0, inscricao.Id);
        Assert.Equal(0, inscricao.CampeonatoId);
        Assert.Null(inscricao.TimeId); // TimeId é nullable
    }

    // ========== TESTES DE RELACIONAMENTOS ==========

    [Fact]
    public void Time_ShouldAddJogadores()
    {
        // Arrange
        var time = new Time { Id = 1, Nome = "Test Team" };
        var jogador = new Jogador { Id = 1, NickName = "Player1", TimeId = 1 };

        // Act
        time.Jogadores.Add(jogador);

        // Assert
        Assert.Single(time.Jogadores);
        Assert.Contains(jogador, time.Jogadores);
    }

    [Fact]
    public void Campeonato_ShouldAddPartidas()
    {
        // Arrange
        var campeonato = new Campeonato { Id = 1, Titulo = "Test Championship" };
        var partida = new Partida { Id = 1, CampeonatoId = 1 };

        // Act
        campeonato.Partidas.Add(partida);

        // Assert
        Assert.Single(campeonato.Partidas);
        Assert.Contains(partida, campeonato.Partidas);
    }

    [Fact]
    public void User_ShouldAddNoticias()
    {
        // Arrange
        var user = new User { Id = 1, Nome = "Test User" };
        var noticia = new Noticia { Id = 1, Titulo = "Test News", AutorId = 1 };

        // Act
        user.Noticias!.Add(noticia);

        // Assert
        Assert.Single(user.Noticias);
        Assert.Contains(noticia, user.Noticias);
    }
}
