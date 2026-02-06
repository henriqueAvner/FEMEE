using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEMEE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_JOGOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SLUG = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IMAGEM_URL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ATIVO = table.Column<bool>(type: "bit", nullable: false),
                    CATEGORIA_JOGO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_JOGOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_PRODUTOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ImagemUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Estoque = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PRODUTOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_TIMES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIME_NOME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TITULOS_TIME = table.Column<int>(type: "int", nullable: false),
                    SLUG = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LOGO_URL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DATA_FUNDACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VITORIAS = table.Column<int>(type: "int", nullable: false),
                    DERROTAS = table.Column<int>(type: "int", nullable: false),
                    EMPATES = table.Column<int>(type: "int", nullable: false),
                    PONTOS = table.Column<int>(type: "int", nullable: false),
                    POSICAO_RANKING = table.Column<int>(type: "int", nullable: false),
                    POSICAO_ANTERIOR = table.Column<int>(type: "int", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_TIMES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_USERS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USUARIO = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SENHA = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TELEFONE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TIPO_USUARIO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_USERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_CAMPEONATO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JogoId = table.Column<int>(type: "int", nullable: false),
                    TITULO = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DATA_INICIO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATA_FIM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BANNER_CAMPEONATO_URL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DATA_LIMITE_INSCRICAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOCAL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CIDADE = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ESTADO = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    PREMIACAO = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NUMERO_VAGAS = table.Column<int>(type: "int", nullable: false),
                    NUMERO_INSCRITOS = table.Column<int>(type: "int", nullable: false),
                    STATUS_CAMPEONATO = table.Column<int>(type: "int", nullable: false),
                    FASE_CAMPEONATO = table.Column<int>(type: "int", nullable: false),
                    REGULAMENTO_URL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CAMPEONATO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_CAMPEONATO_T_JOGOS_JogoId",
                        column: x => x.JogoId,
                        principalTable: "T_JOGOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_JOGADORES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NICKNAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FOTO_URL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FUNCAO = table.Column<int>(type: "int", nullable: false),
                    DATA_ENTRADA_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATA_SAIDA_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    TIME_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_JOGADORES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JOGADOR_TIME",
                        column: x => x.TIME_ID,
                        principalTable: "T_TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_T_JOGADORES_T_USERS_Id",
                        column: x => x.Id,
                        principalTable: "T_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_NOTICIAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AUTOR_ID = table.Column<int>(type: "int", nullable: false),
                    TITULO = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    SLUG = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    RESUMO = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CONTEUDO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CATEGORIA = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NOTICIA_URL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DATA_PUBLICACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NUMERO_COMENTARIOS = table.Column<int>(type: "int", nullable: false),
                    VISUALIZACOES = table.Column<int>(type: "int", nullable: false),
                    PUBLICADA = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_NOTICIAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NOTICIA_USER",
                        column: x => x.AUTOR_ID,
                        principalTable: "T_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_CONQUISTAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIME_ID = table.Column<int>(type: "int", nullable: false),
                    CAMPEONATO_ID = table.Column<int>(type: "int", nullable: true),
                    TITULO = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    POSICAO = table.Column<int>(type: "int", nullable: false),
                    DATA_CONQUISTA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ICONE_TITULO = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CONQUISTAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CONQUISTA_CAMPEONATO",
                        column: x => x.CAMPEONATO_ID,
                        principalTable: "T_CAMPEONATO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_T_CONQUISTAS_T_TIMES_TIME_ID",
                        column: x => x.TIME_ID,
                        principalTable: "T_TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_INSCRICAO_CAMPEONATOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CAMPEONATO_ID = table.Column<int>(type: "int", nullable: false),
                    TIME_ID = table.Column<int>(type: "int", nullable: true),
                    CAPITAO_ID = table.Column<int>(type: "int", nullable: true),
                    TELEFONE_CONTATO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EMAIL_CONTATO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NOME_CAPITAO = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NOME_TIME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DATA_INSCRICAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STATUS_INSCRICAO = table.Column<int>(type: "int", nullable: false),
                    OBSERVACOES = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_INSCRICAO_CAMPEONATOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CONQUISTA_TIME",
                        column: x => x.TIME_ID,
                        principalTable: "T_TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INSCRICAO_CAMPEONATO",
                        column: x => x.CAMPEONATO_ID,
                        principalTable: "T_CAMPEONATO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INSCRICAO_CAPITAO",
                        column: x => x.CAPITAO_ID,
                        principalTable: "T_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_PARTIDAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampeonatoId = table.Column<int>(type: "int", nullable: false),
                    TIME_A_ID = table.Column<int>(type: "int", nullable: false),
                    TIME_B_ID = table.Column<int>(type: "int", nullable: false),
                    TIME_VENCEDOR_ID = table.Column<int>(type: "int", nullable: true),
                    DATA_HORA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOCAL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FASE = table.Column<int>(type: "int", nullable: false),
                    PLACAR_TIME_A = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PLACAR_TIME_B = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    STATUS_PARTIDA = table.Column<int>(type: "int", nullable: false),
                    TRANSMISSAO_URL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PARTIDAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PARTIDA_CAMPEONATO",
                        column: x => x.CampeonatoId,
                        principalTable: "T_CAMPEONATO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PARTIDA_TIMEA",
                        column: x => x.TIME_A_ID,
                        principalTable: "T_TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PARTIDA_TIMEB",
                        column: x => x.TIME_B_ID,
                        principalTable: "T_TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_PARTIDAS_T_TIMES_TIME_VENCEDOR_ID",
                        column: x => x.TIME_VENCEDOR_ID,
                        principalTable: "T_TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAMPEONATO_JOGOID",
                table: "T_CAMPEONATO",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_CAMPEONATO_STATUS",
                table: "T_CAMPEONATO",
                column: "STATUS_CAMPEONATO");

            migrationBuilder.CreateIndex(
                name: "IX_T_CONQUISTAS_CAMPEONATO_ID",
                table: "T_CONQUISTAS",
                column: "CAMPEONATO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CONQUISTAS_TIME_ID",
                table: "T_CONQUISTAS",
                column: "TIME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INSCRICAO_CAMPEONATO_STATUS",
                table: "T_INSCRICAO_CAMPEONATOS",
                column: "STATUS_INSCRICAO");

            migrationBuilder.CreateIndex(
                name: "IX_INSCRICAOCAMPEONATO_CAMPEONATOID_TIMEID_UNIQUE",
                table: "T_INSCRICAO_CAMPEONATOS",
                columns: new[] { "CAMPEONATO_ID", "TIME_ID" },
                unique: true,
                filter: "[TIME_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_T_INSCRICAO_CAMPEONATOS_CAPITAO_ID",
                table: "T_INSCRICAO_CAMPEONATOS",
                column: "CAPITAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_INSCRICAO_CAMPEONATOS_TIME_ID",
                table: "T_INSCRICAO_CAMPEONATOS",
                column: "TIME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_JOGADOR_NICKNAME_UNIQUE",
                table: "T_JOGADORES",
                column: "NICKNAME",
                unique: true,
                filter: "[NICKNAME] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JOGADOR_TIMEID_STATUS",
                table: "T_JOGADORES",
                columns: new[] { "TIME_ID", "STATUS" });

            migrationBuilder.CreateIndex(
                name: "IX_JOGO_SLUG_UNIQUE",
                table: "T_JOGOS",
                column: "SLUG",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NOTICIA_DATAPUBLICACAO",
                table: "T_NOTICIAS",
                column: "DATA_PUBLICACAO");

            migrationBuilder.CreateIndex(
                name: "IX_NOTICIA_PUBLICADA",
                table: "T_NOTICIAS",
                column: "PUBLICADA");

            migrationBuilder.CreateIndex(
                name: "IX_NOTICIA_SLUG_UNIQUE",
                table: "T_NOTICIAS",
                column: "SLUG",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_NOTICIAS_AUTOR_ID",
                table: "T_NOTICIAS",
                column: "AUTOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PARTIDA_CAMPEONATOID",
                table: "T_PARTIDAS",
                column: "CampeonatoId");

            migrationBuilder.CreateIndex(
                name: "IX_PARTIDA_CAMPEONATOID_DATAHORA",
                table: "T_PARTIDAS",
                columns: new[] { "CampeonatoId", "DATA_HORA" });

            migrationBuilder.CreateIndex(
                name: "IX_PARTIDA_STATUS",
                table: "T_PARTIDAS",
                column: "STATUS_PARTIDA");

            migrationBuilder.CreateIndex(
                name: "IX_T_PARTIDAS_TIME_A_ID",
                table: "T_PARTIDAS",
                column: "TIME_A_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_PARTIDAS_TIME_B_ID",
                table: "T_PARTIDAS",
                column: "TIME_B_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_PARTIDAS_TIME_VENCEDOR_ID",
                table: "T_PARTIDAS",
                column: "TIME_VENCEDOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_ATIVO",
                table: "T_PRODUTOS",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_CATEGORIA",
                table: "T_PRODUTOS",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_TIME_PONTOS",
                table: "T_TIMES",
                column: "PONTOS");

            migrationBuilder.CreateIndex(
                name: "IX_TIME_SLUG_UNIQUE",
                table: "T_TIMES",
                column: "SLUG",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_EMAIL_UNIQUE",
                table: "T_USERS",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_CONQUISTAS");

            migrationBuilder.DropTable(
                name: "T_INSCRICAO_CAMPEONATOS");

            migrationBuilder.DropTable(
                name: "T_JOGADORES");

            migrationBuilder.DropTable(
                name: "T_NOTICIAS");

            migrationBuilder.DropTable(
                name: "T_PARTIDAS");

            migrationBuilder.DropTable(
                name: "T_PRODUTOS");

            migrationBuilder.DropTable(
                name: "T_USERS");

            migrationBuilder.DropTable(
                name: "T_CAMPEONATO");

            migrationBuilder.DropTable(
                name: "T_TIMES");

            migrationBuilder.DropTable(
                name: "T_JOGOS");
        }
    }
}
