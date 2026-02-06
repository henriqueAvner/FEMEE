using AutoMapper;
using FEMEE.Application.DTOs.Campeonato;
using FEMEE.Application.DTOs.Jogador;
using FEMEE.Application.DTOs.Noticia;
using FEMEE.Application.DTOs.Partida;
using FEMEE.Application.DTOs.Produto;
using FEMEE.Application.DTOs.Time;
using FEMEE.Application.DTOs.User;
using FEMEE.Application.Mappings;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Store;
using FEMEE.Domain.Enums;

namespace FEMEE.UnitTests.Mappings
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;

        public MappingProfileTests()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = _config.CreateMapper();
        }

        #region Configuration Tests

        [Fact]
        public void Configuration_CanCreateMapper()
        {
            // Assert - validates that mapper can be created successfully
            var mapper = _config.CreateMapper();
            Assert.NotNull(mapper);
        }

        #endregion

        #region User Mapping Tests

        [Fact]
        public void Map_User_To_UserResponseDto_MapsCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Nome = "Test User",
                Email = "test@email.com",
                Telefone = "11999999999",
                TipoUsuario = TipoUsuario.Jogador,
                DataCriacao = DateTime.UtcNow
            };

            // Act
            var dto = _mapper.Map<UserResponseDto>(user);

            // Assert
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Nome, dto.Nome);
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.Telefone, dto.Telefone);
            Assert.Equal(user.TipoUsuario, dto.TipoUsuario);
        }

        [Fact]
        public void Map_CreateUserDto_To_User_MapsCorrectly()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Nome = "New User",
                Email = "new@email.com",
                Senha = "password123",
                Telefone = "11999999999"
            };

            // Act
            var user = _mapper.Map<User>(dto);

            // Assert
            Assert.Equal(dto.Nome, user.Nome);
            Assert.Equal(dto.Email, user.Email);
            Assert.Equal(dto.Senha, user.Senha);
            Assert.Equal(dto.Telefone, user.Telefone);
        }

        #endregion

        #region Time Mapping Tests

        [Fact]
        public void Map_Time_To_TimeResponseDto_MapsCorrectly()
        {
            // Arrange
            var time = new Time
            {
                Id = 1,
                Nome = "Test Team",
                Slug = "test-team",
                Vitorias = 10,
                Derrotas = 5,
                Empates = 2,
                Pontos = 32
            };

            // Act
            var dto = _mapper.Map<TimeResponseDto>(time);

            // Assert
            Assert.Equal(time.Id, dto.Id);
            Assert.Equal(time.Nome, dto.Nome);
            Assert.Equal(time.Slug, dto.Slug);
            Assert.Equal(time.Vitorias, dto.Vitorias);
            Assert.Equal(time.Derrotas, dto.Derrotas);
            Assert.Equal(time.Empates, dto.Empates);
        }

        [Fact]
        public void Map_CreateTimeDto_To_Time_MapsCorrectly()
        {
            // Arrange
            var dto = new CreateTimeDto
            {
                Nome = "New Team",
                Slug = "new-team",
                Descricao = "Description"
            };

            // Act
            var time = _mapper.Map<Time>(dto);

            // Assert
            Assert.Equal(dto.Nome, time.Nome);
            Assert.Equal(dto.Slug, time.Slug);
            Assert.Equal(dto.Descricao, time.Descricao);
        }

        #endregion

        #region Jogador Mapping Tests

        [Fact]
        public void Map_Jogador_To_JogadorResponseDto_MapsCorrectly()
        {
            // Arrange
            var jogador = new Jogador
            {
                Id = 1,
                NickName = "TestPlayer",
                Funcao = FuncaoJogador.Jogador,
                Status = StatusJogador.Ativo,
                User = new User
                {
                    Id = 1,
                    Nome = "Player Name",
                    Email = "player@email.com"
                }
            };

            // Act
            var dto = _mapper.Map<JogadorResponseDto>(jogador);

            // Assert
            Assert.Equal(jogador.Id, dto.Id);
            Assert.Equal(jogador.NickName, dto.NickName);
            Assert.Equal(jogador.Funcao, dto.Funcao);
            Assert.Equal(jogador.Status, dto.Status);
            Assert.Equal(jogador.User.Nome, dto.NomeCompleto);
            Assert.NotNull(dto.User);
        }

        [Fact]
        public void Map_CreateJogadorDto_To_Jogador_MapsCorrectly()
        {
            // Arrange
            var dto = new CreateJogadorDto
            {
                NickName = "NewPlayer",
                Funcao = FuncaoJogador.Capitao,
                UserId = 1
            };

            // Act
            var jogador = _mapper.Map<Jogador>(dto);

            // Assert
            Assert.Equal(dto.NickName, jogador.NickName);
            Assert.Equal(dto.Funcao, jogador.Funcao);
            Assert.Equal(dto.UserId, jogador.UserId);
        }

        #endregion

        #region Campeonato Mapping Tests

        [Fact]
        public void Map_Campeonato_To_CampeonatoResponseDto_MapsCorrectly()
        {
            // Arrange
            var campeonato = new Campeonato
            {
                Id = 1,
                Titulo = "Test Championship",
                Descricao = "Description",
                DataInicio = DateTime.UtcNow.AddDays(7),
                DataFim = DateTime.UtcNow.AddDays(30),
                Status = StatusCampeonato.Open,
                Local = "Online"
            };

            // Act
            var dto = _mapper.Map<CampeonatoResponseDto>(campeonato);

            // Assert
            Assert.Equal(campeonato.Id, dto.Id);
            Assert.Equal(campeonato.Titulo, dto.Titulo);
            Assert.Equal(campeonato.Descricao, dto.Descricao);
            Assert.Equal(campeonato.Status, dto.Status);
        }

        [Fact]
        public void Map_CreateCampeonatoDto_To_Campeonato_MapsCorrectly()
        {
            // Arrange
            var dto = new CreateCampeonatoDto
            {
                Titulo = "New Championship",
                Descricao = "Description",
                JogoId = 1,
                DataInicio = DateTime.UtcNow.AddDays(7),
                DataFim = DateTime.UtcNow.AddDays(30)
            };

            // Act
            var campeonato = _mapper.Map<Campeonato>(dto);

            // Assert
            Assert.Equal(dto.Titulo, campeonato.Titulo);
            Assert.Equal(dto.JogoId, campeonato.JogoId);
        }

        #endregion

        #region Partida Mapping Tests

        [Fact]
        public void Map_Partida_To_PartidaResponseDto_MapsCorrectly()
        {
            // Arrange
            var partida = new Partida
            {
                Id = 1,
                CampeonatoId = 1,
                DataHora = DateTime.UtcNow.AddDays(1),
                Status = StatusPartida.Agendada,
                TimeA = new Time { Id = 1, Nome = "Team A", Slug = "team-a" },
                TimeB = new Time { Id = 2, Nome = "Team B", Slug = "team-b" }
            };

            // Act
            var dto = _mapper.Map<PartidaResponseDto>(partida);

            // Assert
            Assert.Equal(partida.Id, dto.Id);
            Assert.Equal(partida.CampeonatoId, dto.CampeonatoId);
            Assert.Equal(partida.Status, dto.Status);
            Assert.NotNull(dto.TimeA);
            Assert.NotNull(dto.TimeB);
        }

        #endregion

        #region Noticia Mapping Tests

        [Fact]
        public void Map_Noticia_To_NoticiaResponseDto_MapsCorrectly()
        {
            // Arrange
            var noticia = new Noticia
            {
                Id = 1,
                Titulo = "Test News",
                Conteudo = "Content here",
                DataPublicacao = DateTime.UtcNow,
                Autor = new User { Id = 1, Nome = "Author", Email = "author@email.com" }
            };

            // Act
            var dto = _mapper.Map<NoticiaResponseDto>(noticia);

            // Assert
            Assert.Equal(noticia.Id, dto.Id);
            Assert.Equal(noticia.Titulo, dto.Titulo);
            Assert.Equal(noticia.Conteudo, dto.Conteudo);
            Assert.NotNull(dto.Autor);
        }

        #endregion

        #region Produto Mapping Tests

        [Fact]
        public void Map_Produto_To_ProdutoResponseDto_MapsCorrectly()
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = "Test Product",
                Descricao = "Description",
                Preco = 99.99m,
                Ativo = true
            };

            // Act
            var dto = _mapper.Map<ProdutoResponseDto>(produto);

            // Assert
            Assert.Equal(produto.Id, dto.Id);
            Assert.Equal(produto.Nome, dto.Nome);
            Assert.Equal(produto.Preco, dto.Preco);
        }

        [Fact]
        public void Map_CreateProdutoDto_To_Produto_MapsCorrectly()
        {
            // Arrange
            var dto = new CreateProdutoDto
            {
                Nome = "New Product",
                Descricao = "Description",
                Preco = 49.99m
            };

            // Act
            var produto = _mapper.Map<Produto>(dto);

            // Assert
            Assert.Equal(dto.Nome, produto.Nome);
            Assert.Equal(dto.Descricao, produto.Descricao);
            Assert.Equal(dto.Preco, produto.Preco);
        }

        #endregion

        #region Null Handling Tests

        [Fact]
        public void Map_NullUser_ReturnsNull()
        {
            // Arrange
            User? user = null;

            // Act
            var dto = _mapper.Map<UserResponseDto>(user);

            // Assert
            Assert.Null(dto);
        }

        [Fact]
        public void Map_Jogador_WithNullUser_MapsWithEmptyNomeCompleto()
        {
            // Arrange
            var jogador = new Jogador
            {
                Id = 1,
                NickName = "TestPlayer",
                User = null!
            };

            // Act
            var dto = _mapper.Map<JogadorResponseDto>(jogador);

            // Assert
            Assert.Equal(string.Empty, dto.NomeCompleto);
        }

        #endregion
    }
}
