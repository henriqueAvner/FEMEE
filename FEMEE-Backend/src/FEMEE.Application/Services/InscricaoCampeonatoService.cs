using AutoMapper;
using FEMEE.Application.DTOs.InscricaoCampeonato;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de inscrições de campeonato.
    /// </summary>
    public class InscricaoCampeonatoService : IInscricaoCampeonatoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InscricaoCampeonatoService> _logger;

        public InscricaoCampeonatoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InscricaoCampeonatoService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<InscricaoCampeonatoResponseDto> GetInscricaoByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetInscricaoById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando inscrição com ID: {InscricaoId}", id);
                    var inscricao = await _unitOfWork.InscricoesCampeonato.GetByIdAsync(id);

                    if (inscricao == null)
                    {
                        _logger.LogWarning("Inscrição não encontrada: {InscricaoId}", id);
                        throw new KeyNotFoundException($"Inscrição com ID {id} não encontrada");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Inscrição encontrada em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    return _mapper.Map<InscricaoCampeonatoResponseDto>(inscricao);
                }
                catch (Exception ex) when (ex is not KeyNotFoundException)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar inscrição em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetAllInscricoesAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllInscricoes"))
            {
                _logger.LogInformation("Buscando todas as inscrições");
                var inscricoes = await _unitOfWork.InscricoesCampeonato.GetAllAsync();
                _logger.LogInformation("Total de inscrições encontradas: {Count}", inscricoes.Count());
                return _mapper.Map<IEnumerable<InscricaoCampeonatoResponseDto>>(inscricoes);
            }
        }

        public async Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetInscricoesByCampeonatoIdAsync(int campeonatoId)
        {
            using (StructuredLogging.BeginOperationScope("GetInscricoesByCampeonatoId", campeonatoId))
            {
                _logger.LogInformation("Buscando inscrições do campeonato: {CampeonatoId}", campeonatoId);
                var inscricoes = await _unitOfWork.InscricoesCampeonato.GetAllAsync();
                var inscricoesCampeonato = inscricoes.Where(i => i.CampeonatoId == campeonatoId);
                _logger.LogInformation("Inscrições encontradas: {Count}", inscricoesCampeonato.Count());
                return _mapper.Map<IEnumerable<InscricaoCampeonatoResponseDto>>(inscricoesCampeonato);
            }
        }

        public async Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetInscricoesByTimeIdAsync(int timeId)
        {
            using (StructuredLogging.BeginOperationScope("GetInscricoesByTimeId", timeId))
            {
                _logger.LogInformation("Buscando inscrições do time: {TimeId}", timeId);
                var inscricoes = await _unitOfWork.InscricoesCampeonato.GetAllAsync();
                var inscricoesTime = inscricoes.Where(i => i.TimeId == timeId);
                _logger.LogInformation("Inscrições encontradas: {Count}", inscricoesTime.Count());
                return _mapper.Map<IEnumerable<InscricaoCampeonatoResponseDto>>(inscricoesTime);
            }
        }

        public async Task<IEnumerable<InscricaoCampeonatoResponseDto>> GetInscricoesByStatusAsync(StatusInscricao status)
        {
            using (StructuredLogging.BeginOperationScope("GetInscricoesByStatus"))
            {
                _logger.LogInformation("Buscando inscrições com status: {Status}", status);
                var inscricoes = await _unitOfWork.InscricoesCampeonato.GetAllAsync();
                var inscricoesStatus = inscricoes.Where(i => i.StatusInscricao == status);
                _logger.LogInformation("Inscrições encontradas: {Count}", inscricoesStatus.Count());
                return _mapper.Map<IEnumerable<InscricaoCampeonatoResponseDto>>(inscricoesStatus);
            }
        }

        public async Task<InscricaoCampeonatoResponseDto> CreateInscricaoAsync(CreateInscricaoCampeonatoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateInscricao"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Criando nova inscrição para campeonato: {CampeonatoId}", dto.CampeonatoId);

                    // Verificar se campeonato existe
                    var campeonato = await _unitOfWork.Campeonatos.GetByIdAsync(dto.CampeonatoId);
                    if (campeonato == null)
                    {
                        throw new InvalidOperationException($"Campeonato com ID {dto.CampeonatoId} não encontrado");
                    }

                    // Verificar se campeonato aceita inscrições
                    if (campeonato.Status != StatusCampeonato.Open)
                    {
                        throw new InvalidOperationException("Campeonato não está aberto para inscrições");
                    }

                    // Verificar data limite
                    if (DateTime.UtcNow > campeonato.DataLimiteInscricao)
                    {
                        throw new InvalidOperationException("Prazo de inscrição encerrado");
                    }

                    // Verificar vagas
                    if (campeonato.NumeroInscritos >= campeonato.NumeroVagas)
                    {
                        throw new InvalidOperationException("Não há vagas disponíveis");
                    }

                    var inscricao = _mapper.Map<InscricaoCampeonato>(dto);
                    inscricao.DataInscricao = DateTime.UtcNow;
                    inscricao.StatusInscricao = StatusInscricao.Pendente;

                    await _unitOfWork.InscricoesCampeonato.AddAsync(inscricao);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Inscrição criada com ID {InscricaoId} em {ElapsedMilliseconds}ms",
                        inscricao.Id, stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<InscricaoCampeonatoResponseDto>(inscricao);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar inscrição em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<InscricaoCampeonatoResponseDto> UpdateInscricaoAsync(int id, UpdateInscricaoCampeonatoDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateInscricao", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando inscrição com ID: {InscricaoId}", id);
                    var inscricao = await _unitOfWork.InscricoesCampeonato.GetByIdAsync(id);

                    if (inscricao == null)
                    {
                        _logger.LogWarning("Inscrição não encontrada para atualização: {InscricaoId}", id);
                        throw new KeyNotFoundException($"Inscrição com ID {id} não encontrada");
                    }

                    // Atualizar propriedades
                    if (dto.StatusInscricao.HasValue) inscricao.StatusInscricao = dto.StatusInscricao.Value;
                    if (dto.TelefoneContato != null) inscricao.TelefoneContato = dto.TelefoneContato;
                    if (dto.EmailContato != null) inscricao.EmailContato = dto.EmailContato;
                    if (dto.NomeCapitao != null) inscricao.NomeCapitao = dto.NomeCapitao;
                    if (dto.NomeTime != null) inscricao.NomeTime = dto.NomeTime;
                    if (dto.Observacoes != null) inscricao.Observacoes = dto.Observacoes;

                    await _unitOfWork.InscricoesCampeonato.UpdateAsync(inscricao);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Inscrição atualizada em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<InscricaoCampeonatoResponseDto>(inscricao);
                }
                catch (Exception ex) when (ex is not KeyNotFoundException)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar inscrição em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public async Task<InscricaoCampeonatoResponseDto> AprovarInscricaoAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("AprovarInscricao", id))
            {
                _logger.LogInformation("Aprovando inscrição: {InscricaoId}", id);
                var inscricao = await _unitOfWork.InscricoesCampeonato.GetByIdAsync(id);

                if (inscricao == null)
                {
                    throw new KeyNotFoundException($"Inscrição com ID {id} não encontrada");
                }

                if (inscricao.StatusInscricao != StatusInscricao.Pendente)
                {
                    throw new InvalidOperationException("Apenas inscrições pendentes podem ser aprovadas");
                }

                inscricao.StatusInscricao = StatusInscricao.Aprovada;
                await _unitOfWork.InscricoesCampeonato.UpdateAsync(inscricao);

                // Incrementar número de inscritos no campeonato
                var campeonato = await _unitOfWork.Campeonatos.GetByIdAsync(inscricao.CampeonatoId);
                if (campeonato != null)
                {
                    campeonato.NumeroInscritos++;
                    await _unitOfWork.Campeonatos.UpdateAsync(campeonato);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Inscrição aprovada: {InscricaoId}", id);
                return _mapper.Map<InscricaoCampeonatoResponseDto>(inscricao);
            }
        }

        public async Task<InscricaoCampeonatoResponseDto> RejeitarInscricaoAsync(int id, string? motivo = null)
        {
            using (StructuredLogging.BeginOperationScope("RejeitarInscricao", id))
            {
                _logger.LogInformation("Rejeitando inscrição: {InscricaoId}", id);
                var inscricao = await _unitOfWork.InscricoesCampeonato.GetByIdAsync(id);

                if (inscricao == null)
                {
                    throw new KeyNotFoundException($"Inscrição com ID {id} não encontrada");
                }

                if (inscricao.StatusInscricao != StatusInscricao.Pendente)
                {
                    throw new InvalidOperationException("Apenas inscrições pendentes podem ser rejeitadas");
                }

                inscricao.StatusInscricao = StatusInscricao.Rejeitada;
                if (motivo != null)
                {
                    inscricao.Observacoes = motivo;
                }

                await _unitOfWork.InscricoesCampeonato.UpdateAsync(inscricao);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Inscrição rejeitada: {InscricaoId}", id);
                return _mapper.Map<InscricaoCampeonatoResponseDto>(inscricao);
            }
        }

        public async Task DeleteInscricaoAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteInscricao", id))
            {
                _logger.LogInformation("Deletando inscrição com ID: {InscricaoId}", id);
                var inscricao = await _unitOfWork.InscricoesCampeonato.GetByIdAsync(id);

                if (inscricao == null)
                {
                    _logger.LogWarning("Inscrição não encontrada para deleção: {InscricaoId}", id);
                    throw new KeyNotFoundException($"Inscrição com ID {id} não encontrada");
                }

                await _unitOfWork.InscricoesCampeonato.DeleteAsync(inscricao.Id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Inscrição deletada com sucesso: {InscricaoId}", id);
            }
        }
    }
}
