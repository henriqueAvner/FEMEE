using FEMEE.Application.DTOs.Jogador;
using FEMEE.Application.Validators.Jogador;
using FEMEE.Domain.Enums;
using FluentValidation.Results;

namespace FEMEE.UnitTests.Validators;

/// <summary>
/// Testes para CreateJogadorDtoValidator.
/// </summary>
public class CreateJogadorDtoValidatorTests
{
    private readonly CreateJogadorDtoValidator _validator;

    public CreateJogadorDtoValidatorTests()
    {
        _validator = new CreateJogadorDtoValidator();
    }

    #region UserId Tests

    [Fact]
    public void UserId_WhenZero_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 0, NickName = "ValidNick", Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UserId");
    }

    [Fact]
    public void UserId_WhenNegative_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = -1, NickName = "ValidNick", Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UserId");
    }

    [Fact]
    public void UserId_WhenPositive_ShouldNotHaveError()
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 1, NickName = "ValidNick", Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "UserId");
    }

    #endregion

    #region NickName Tests

    [Theory]
    [InlineData("")]
    public void NickName_WhenEmpty_ShouldHaveError(string nickname)
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 1, NickName = nickname, Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.Contains(result.Errors, e => e.PropertyName == "NickName");
    }

    [Theory]
    [InlineData("ab")] // muito curto
    [InlineData("a")] // muito curto
    public void NickName_WhenTooShort_ShouldHaveError(string nickname)
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 1, NickName = nickname, Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.Contains(result.Errors, e => e.PropertyName == "NickName");
    }

    [Fact]
    public void NickName_WhenTooLong_ShouldHaveError()
    {
        // Arrange
        var longNickname = new string('a', 51);
        var dto = new CreateJogadorDto { UserId = 1, NickName = longNickname, Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.Contains(result.Errors, e => e.PropertyName == "NickName");
    }

    [Theory]
    [InlineData("Player One")] // espaço
    [InlineData("Player@123")] // @
    [InlineData("Player.123")] // .
    public void NickName_WhenHasInvalidChars_ShouldHaveError(string nickname)
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 1, NickName = nickname, Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.Contains(result.Errors, e => e.PropertyName == "NickName");
    }

    [Theory]
    [InlineData("Player123")]
    [InlineData("Player_123")]
    [InlineData("Player-123")]
    [InlineData("abc")]
    public void NickName_WhenValid_ShouldNotHaveError(string nickname)
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 1, NickName = nickname, Funcao = FuncaoJogador.Jogador };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "NickName");
    }

    #endregion

    #region Funcao Tests

    [Fact]
    public void Funcao_WhenValid_ShouldNotHaveError()
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 1, NickName = "ValidNick", Funcao = FuncaoJogador.Capitao };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "Funcao");
    }

    [Fact]
    public void Funcao_WhenInvalid_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateJogadorDto { UserId = 1, NickName = "ValidNick", Funcao = (FuncaoJogador)999 };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.Contains(result.Errors, e => e.PropertyName == "Funcao");
    }

    #endregion

    #region Full DTO Tests

    [Fact]
    public void ValidDto_ShouldPassValidation()
    {
        // Arrange
        var dto = new CreateJogadorDto
        {
            UserId = 1,
            NickName = "ValidPlayer",
            Funcao = FuncaoJogador.Jogador,
            TimeId = null
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    #endregion
}
