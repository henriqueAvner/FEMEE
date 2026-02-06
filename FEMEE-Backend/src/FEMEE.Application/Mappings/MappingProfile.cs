// FEMEE.Application/Mappings/MappingProfile.cs

using AutoMapper;
using FEMEE.Application.DTOs.Campeonato;
using FEMEE.Application.DTOs.Conquista;
using FEMEE.Application.DTOs.InscricaoCampeonato;
using FEMEE.Application.DTOs.Jogador;
using FEMEE.Application.DTOs.Jogo;
using FEMEE.Application.DTOs.Noticia;
using FEMEE.Application.DTOs.Partida;
using FEMEE.Application.DTOs.Produto;
using FEMEE.Application.DTOs.Time;
using FEMEE.Application.DTOs.User;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Store;

namespace FEMEE.Application.Mappings
{
    /// <summary>
    /// Perfil de mapeamento AutoMapper.
    /// Define como converter entidades para DTOs e vice-versa.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Construtor que configura todos os mapeamentos.
        /// </summary>
        public MappingProfile()
        {
            // ===== USER =====
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // ===== TIME =====
            CreateMap<Time, TimeResponseDto>().ReverseMap();
            CreateMap<CreateTimeDto, Time>();
            CreateMap<UpdateTimeDto, Time>();

            // ===== JOGADOR =====
            // Jogador usa composição com User (não herança)
            CreateMap<Jogador, JogadorResponseDto>()
                .ForMember(dest => dest.NomeCompleto, opt => opt.MapFrom(src => src.User != null ? src.User.Nome : string.Empty))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<CreateJogadorDto, Jogador>();
            CreateMap<UpdateJogadorDto, Jogador>();

            // ===== CAMPEONATO =====
            CreateMap<Campeonato, CampeonatoResponseDto>()
                .ForMember(dest => dest.Jogo, opt => opt.MapFrom(src => src.Jogo));
            CreateMap<CreateCampeonatoDto, Campeonato>();
            CreateMap<UpdateCampeonatoDto, Campeonato>();

            // ===== PARTIDA =====
            CreateMap<Partida, PartidaResponseDto>()
                .ForMember(dest => dest.TimeA, opt => opt.MapFrom(src => src.TimeA))
                .ForMember(dest => dest.TimeB, opt => opt.MapFrom(src => src.TimeB))
                .ForMember(dest => dest.TimeVencedor, opt => opt.MapFrom(src => src.TimeVencedor));
            CreateMap<CreatePartidaDto, Partida>();
            CreateMap<UpdatePartidaDto, Partida>();

            // ===== NOTICIA =====
            CreateMap<Noticia, NoticiaResponseDto>()
                .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.Autor));
            CreateMap<CreateNoticiaDto, Noticia>();
            CreateMap<UpdateNoticiaDto, Noticia>();

            // ===== PRODUTO =====
            CreateMap<Produto, ProdutoResponseDto>().ReverseMap();
            CreateMap<CreateProdutoDto, Produto>();
            CreateMap<UpdateProdutoDto, Produto>();

            // ===== JOGO =====
            CreateMap<Jogo, JogoResponseDto>().ReverseMap();
            CreateMap<CreateJogoDto, Jogo>();
            CreateMap<UpdateJogoDto, Jogo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ===== CONQUISTA =====
            CreateMap<Conquista, ConquistaResponseDto>().ReverseMap();
            CreateMap<CreateConquistaDto, Conquista>();
            CreateMap<UpdateConquistaDto, Conquista>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ===== INSCRICAO CAMPEONATO =====
            CreateMap<InscricaoCampeonato, InscricaoCampeonatoResponseDto>().ReverseMap();
            CreateMap<CreateInscricaoCampeonatoDto, InscricaoCampeonato>();
            CreateMap<UpdateInscricaoCampeonatoDto, InscricaoCampeonato>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
