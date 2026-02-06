using FEMEE.Domain.Enums;

namespace FEMEE.UnitTests.Domain;

/// <summary>
/// Testes para os Enums do Domain.
/// Garantem que os valores dos enums estão corretos e documentados.
/// </summary>
public class EnumsTests
{
    // ========== STATUS PARTIDA ==========

    [Fact]
    public void StatusPartida_ShouldHaveCorrectValues()
    {
        Assert.Equal(1, (int)StatusPartida.Agendada);
        Assert.Equal(2, (int)StatusPartida.EmAndamento);
        Assert.Equal(3, (int)StatusPartida.Finalizada);
        Assert.Equal(4, (int)StatusPartida.Cancelada);
        Assert.Equal(5, (int)StatusPartida.Adiada);
    }

    [Fact]
    public void StatusPartida_ShouldHave5Values()
    {
        var values = Enum.GetValues<StatusPartida>();
        Assert.Equal(5, values.Length);
    }

    // ========== STATUS CAMPEONATO ==========

    [Fact]
    public void StatusCampeonato_ShouldHaveCorrectValues()
    {
        Assert.Equal(0, (int)StatusCampeonato.UpComing);
        Assert.Equal(1, (int)StatusCampeonato.Open);
        Assert.Equal(2, (int)StatusCampeonato.InProgress);
        Assert.Equal(3, (int)StatusCampeonato.Closed);
        Assert.Equal(4, (int)StatusCampeonato.Cancelled);
    }

    [Fact]
    public void StatusCampeonato_ShouldHave5Values()
    {
        var values = Enum.GetValues<StatusCampeonato>();
        Assert.Equal(5, values.Length);
    }

    // ========== TIPO USUARIO ==========

    [Fact]
    public void TipoUsuario_ShouldHaveCorrectValues()
    {
        Assert.Equal(1, (int)TipoUsuario.Administrador);
        Assert.Equal(2, (int)TipoUsuario.Capitao);
        Assert.Equal(3, (int)TipoUsuario.Jogador);
        Assert.Equal(4, (int)TipoUsuario.Visitante);
        Assert.Equal(5, (int)TipoUsuario.Moderador);
    }

    [Fact]
    public void TipoUsuario_ShouldHave5Values()
    {
        var values = Enum.GetValues<TipoUsuario>();
        Assert.Equal(5, values.Length);
    }

    // ========== FUNCAO JOGADOR ==========

    [Fact]
    public void FuncaoJogador_ShouldHaveCorrectValues()
    {
        Assert.Equal(0, (int)FuncaoJogador.Nenhum);
        Assert.Equal(1, (int)FuncaoJogador.Jogador);
        Assert.Equal(2, (int)FuncaoJogador.Capitao);
    }

    [Fact]
    public void FuncaoJogador_ShouldHave3Values()
    {
        var values = Enum.GetValues<FuncaoJogador>();
        Assert.Equal(3, values.Length);
    }

    // ========== FUNCAO JOGADOR LOL ==========

    [Fact]
    public void FuncaoJogadorLol_ShouldHaveCorrectValues()
    {
        Assert.Equal(1, (int)FuncaoJogadorLol.Top);
        Assert.Equal(2, (int)FuncaoJogadorLol.Jungle);
        Assert.Equal(3, (int)FuncaoJogadorLol.Mid);
        Assert.Equal(4, (int)FuncaoJogadorLol.Adc);
        Assert.Equal(5, (int)FuncaoJogadorLol.Suporte);
    }

    [Fact]
    public void FuncaoJogadorLol_ShouldHave5Values()
    {
        var values = Enum.GetValues<FuncaoJogadorLol>();
        Assert.Equal(5, values.Length);
    }

    // ========== FUNCAO JOGADOR CS ==========

    [Fact]
    public void FuncaoJogadorCs_ShouldHaveCorrectValues()
    {
        Assert.Equal(1, (int)FuncaoJogadorCs.EntryFragger);
        Assert.Equal(2, (int)FuncaoJogadorCs.Support);
        Assert.Equal(3, (int)FuncaoJogadorCs.Lurker);
        Assert.Equal(4, (int)FuncaoJogadorCs.AWPer);
        Assert.Equal(5, (int)FuncaoJogadorCs.InGameLeader);
    }

    [Fact]
    public void FuncaoJogadorCs_ShouldHave5Values()
    {
        var values = Enum.GetValues<FuncaoJogadorCs>();
        Assert.Equal(5, values.Length);
    }

    // ========== FASE ==========

    [Fact]
    public void Fase_ShouldHaveCorrectValues()
    {
        Assert.Equal(1, (int)Fase.Grupos);
        Assert.Equal(2, (int)Fase.OitavasDeFinal);
        Assert.Equal(3, (int)Fase.QuartasDeFinal);
        Assert.Equal(4, (int)Fase.Semifinal);
        Assert.Equal(5, (int)Fase.Final);
    }

    [Fact]
    public void Fase_ShouldHave5Values()
    {
        var values = Enum.GetValues<Fase>();
        Assert.Equal(5, values.Length);
    }

    // ========== TESTES DE PARSING ==========

    [Theory]
    [InlineData("Agendada", StatusPartida.Agendada)]
    [InlineData("EmAndamento", StatusPartida.EmAndamento)]
    [InlineData("Finalizada", StatusPartida.Finalizada)]
    public void StatusPartida_ShouldParseFromString(string value, StatusPartida expected)
    {
        var result = Enum.Parse<StatusPartida>(value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("UpComing", StatusCampeonato.UpComing)]
    [InlineData("Open", StatusCampeonato.Open)]
    [InlineData("InProgress", StatusCampeonato.InProgress)]
    public void StatusCampeonato_ShouldParseFromString(string value, StatusCampeonato expected)
    {
        var result = Enum.Parse<StatusCampeonato>(value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Administrador", TipoUsuario.Administrador)]
    [InlineData("Jogador", TipoUsuario.Jogador)]
    [InlineData("Visitante", TipoUsuario.Visitante)]
    public void TipoUsuario_ShouldParseFromString(string value, TipoUsuario expected)
    {
        var result = Enum.Parse<TipoUsuario>(value);
        Assert.Equal(expected, result);
    }

    // ========== TESTES DE CONVERSÃO ==========

    [Fact]
    public void AllEnums_ShouldConvertToAndFromInt()
    {
        // StatusPartida
        Assert.Equal(StatusPartida.Agendada, (StatusPartida)1);
        Assert.Equal(1, (int)StatusPartida.Agendada);

        // StatusCampeonato
        Assert.Equal(StatusCampeonato.Open, (StatusCampeonato)1);
        Assert.Equal(1, (int)StatusCampeonato.Open);

        // TipoUsuario
        Assert.Equal(TipoUsuario.Administrador, (TipoUsuario)1);
        Assert.Equal(1, (int)TipoUsuario.Administrador);

        // Fase
        Assert.Equal(Fase.Grupos, (Fase)1);
        Assert.Equal(1, (int)Fase.Grupos);
    }
}
