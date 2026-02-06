using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Store;
using Microsoft.EntityFrameworkCore;

namespace FEMEE.Infrastructure.Data.Context
{
    public class FemeeDbContext(DbContextOptions<FemeeDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Time> Times {get; set;}
        public DbSet<Jogador> Jogadores {get; set;}
        public DbSet<Campeonato> Campeonatos {get; set;}
        public DbSet<Jogo> Jogos {get; set;}
        public DbSet<Partida> Partidas {get; set;}
        public DbSet<InscricaoCampeonato> InscricoesCampeonatos {get; set;}
        public DbSet<Noticia> Noticias {get; set;}
        public DbSet<Conquista> Conquistas {get; set;}
        public DbSet<Produto> Produtos {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUser(modelBuilder);
            ConfigureTime(modelBuilder);
            ConfigureJogador(modelBuilder);
            ConfigureCampeonato(modelBuilder);
            ConfigureJogo(modelBuilder);
            ConfigurePartida(modelBuilder);
            ConfigureInscricaoCampeonato(modelBuilder);
            ConfigureNoticia(modelBuilder);
            ConfigureConquista(modelBuilder);
            ConfigureProduto(modelBuilder);
        }
        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("IX_USER_EMAIL_UNIQUE");

                user.HasMany(e => e.Noticias)
                .WithOne(n => n.Autor)
                .HasForeignKey(n => n.AutorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_NOTICIA_USER");

                user.HasMany(e => e.InscricoesCampeonatos)
                .WithOne(i => i.Capitao)
                .HasForeignKey(i => i.CapitaoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_INSCRICAO_CAPITAO");

                user.ToTable("T_USERS");

            });
        }
        private static void ConfigureTime(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Time>(time =>
            {
                time.HasIndex(e => e.Slug)
                .IsUnique()
                .HasDatabaseName("IX_TIME_SLUG_UNIQUE");

                time.HasIndex(e => e.Pontos)
                .HasDatabaseName("IX_TIME_PONTOS");

                time.HasMany(e => e.Jogadores)
                .WithOne(j => j.Time)
                .HasForeignKey(j => j.TimeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_JOGADOR_TIME");

                time.HasMany(e => e.InscricoesCampeonatos)
                .WithOne(c => c.Time)
                .HasForeignKey(c => c.TimeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CONQUISTA_TIME");

                time.HasMany(e => e.PartidasCasaTeam)
                .WithOne(p => p.TimeA)
                .HasForeignKey(p => p.TimeAId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PARTIDA_TIMEA");

                time.HasMany(e => e.PartidasVisitanteTeam)
                .WithOne(p => p.TimeB)
                .HasForeignKey(p => p.TimeBId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PARTIDA_TIMEB");
                
                time.ToTable("T_TIMES");
            });
        }
        private static void ConfigureJogador(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jogador>(jogador =>
            {
                // Relacionamento com User (composição)
                jogador.HasOne(j => j.User)
                    .WithMany()
                    .HasForeignKey(j => j.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_JOGADOR_USER");

                // Índice único para nickname
                jogador.HasIndex(e => e.NickName)
                    .IsUnique()
                    .HasDatabaseName("IX_JOGADOR_NICKNAME_UNIQUE");

                // Índice para UserId (1 jogador por user)
                jogador.HasIndex(e => e.UserId)
                    .IsUnique()
                    .HasDatabaseName("IX_JOGADOR_USERID_UNIQUE");

                // Índice composto para queries por time
                jogador.HasIndex(e => new { e.TimeId, e.Status })
                    .HasDatabaseName("IX_JOGADOR_TIMEID_STATUS");

                jogador.HasOne(j => j.Time)
                .WithMany(t => t.Jogadores)
                .HasForeignKey(j => j.TimeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_JOGADOR_TIME");

                jogador.ToTable("T_JOGADORES");
            });
        }

        private static void ConfigureCampeonato(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<Campeonato>(campeonato =>
            {

                // Precisão para valores monetários
                campeonato.Property(e => e.Premiacao)
                    .HasPrecision(18, 2);

                campeonato.Property(e => e.RegulamentoUrl)
                    .HasMaxLength(512);

                // Índice para queries de campeonatos ativos
                campeonato.HasIndex(e => e.Status)
                    .HasDatabaseName("IX_CAMPEONATO_STATUS");

                // Índice para queries por jogo
                campeonato.HasIndex(e => e.JogoId)
                    .HasDatabaseName("IX_CAMPEONATO_JOGOID");

                // Relacionamentos
                campeonato.HasMany(e => e.Partidas)
                    .WithOne(p => p.Campeonato)
                    .HasForeignKey(p => p.CampeonatoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PARTIDA_CAMPEONATO");

                campeonato.HasMany(e => e.InscricoesCampeonatos)
                    .WithOne(i => i.Campeonato)
                    .HasForeignKey(i => i.CampeonatoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_INSCRICAO_CAMPEONATO");

                campeonato.HasMany(e => e.Conquistas)
                    .WithOne(c => c.Campeonato)
                    .HasForeignKey(c => c.CampeonatoId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_CONQUISTA_CAMPEONATO");

                campeonato.ToTable("T_CAMPEONATO");
            });
        }

        private static void ConfigureJogo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jogo>(jogo =>
            {
                // Índice único para slug
                jogo.HasIndex(e => e.Slug)
                    .IsUnique()
                    .HasDatabaseName("IX_JOGO_SLUG_UNIQUE");

                jogo.ToTable("T_JOGOS");
            });
        }
        private static void ConfigurePartida(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partida>(partida =>
            {

                partida.Property(e => e.PlacarTimeA)
                    .HasDefaultValue(0);

                partida.Property(e => e.PlacarTimeB)
                    .HasDefaultValue(0);

                // Índice para queries por campeonato
                partida.HasIndex(e => e.CampeonatoId)
                    .HasDatabaseName("IX_PARTIDA_CAMPEONATOID");

                // Índice para queries por status
                partida.HasIndex(e => e.Status)
                    .HasDatabaseName("IX_PARTIDA_STATUS");

                // Índice composto para queries de data
                partida.HasIndex(e => new { e.CampeonatoId, e.DataHora })
                    .HasDatabaseName("IX_PARTIDA_CAMPEONATOID_DATAHORA");

                // TimeVencedor opcional (partida pode não ter vencedor ainda)
                partida.HasOne(p => p.TimeVencedor)
                    .WithMany()
                    .HasForeignKey(p => p.TimeVencedorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                partida.ToTable("T_PARTIDAS");
            });
        }
         private static void ConfigureInscricaoCampeonato(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InscricaoCampeonato>(icampeonato =>
            {

                // Índice único composto: um time só pode se inscrever uma vez por campeonato
                icampeonato.HasIndex(e => new { e.CampeonatoId, e.TimeId })
                    .IsUnique()
                    .HasDatabaseName("IX_INSCRICAOCAMPEONATO_CAMPEONATOID_TIMEID_UNIQUE");

                // Índice para queries por status
                icampeonato.HasIndex(e => e.StatusInscricao)
                    .HasDatabaseName("IX_INSCRICAO_CAMPEONATO_STATUS");

                icampeonato.ToTable("T_INSCRICAO_CAMPEONATOS");
            });
        }
        private static void ConfigureNoticia(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Noticia>(noticia =>
            {
 
                // Índice único para slug
                noticia.HasIndex(e => e.Slug)
                    .IsUnique()
                    .HasDatabaseName("IX_NOTICIA_SLUG_UNIQUE");

                noticia.Property(e => e.Publicada)
                    .HasDefaultValue(false);

                // Índice para queries de notícias publicadas
                noticia.HasIndex(e => e.Publicada)
                    .HasDatabaseName("IX_NOTICIA_PUBLICADA");

                // Índice para ordenar por data
                noticia.HasIndex(e => e.DataPublicacao)
                    .HasDatabaseName("IX_NOTICIA_DATAPUBLICACAO");

                noticia.ToTable("T_NOTICIAS");
            });
        }
         private static void ConfigureConquista(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conquista>(conquista =>
            {
                conquista.ToTable("T_CONQUISTAS");
            });
        }

        private static void ConfigureProduto(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(1000);

                entity.Property(e => e.Categoria)
                    .HasMaxLength(128);

                entity.Property(e => e.ImagemUrl)
                    .HasMaxLength(512);

                // Precisão para preço
                entity.Property(e => e.Preco)
                    .HasPrecision(18, 2);

                // Valor padrão para estoque
                entity.Property(e => e.Estoque)
                    .HasDefaultValue(0);

                // Índice para queries de produtos ativos
                entity.HasIndex(e => e.Ativo)
                    .HasDatabaseName("IX_PRODUTO_ATIVO");

                // Índice para queries por categoria
                entity.HasIndex(e => e.Categoria)
                    .HasDatabaseName("IX_PRODUTO_CATEGORIA");

                entity.ToTable("T_PRODUTOS");
            });
        }

    }

    
    
    
}