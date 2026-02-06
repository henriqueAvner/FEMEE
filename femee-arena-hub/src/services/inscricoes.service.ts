import api from './api';
import type {
  InscricaoCampeonatoResponse,
  CreateInscricaoCampeonatoRequest,
  StatusInscricao,
} from '@/types/api.types';

export const inscricoesService = {
  /**
   * Cria uma nova inscrição em campeonato
   */
  async create(data: CreateInscricaoCampeonatoRequest): Promise<InscricaoCampeonatoResponse> {
    const response = await api.post<InscricaoCampeonatoResponse>('/inscricoes-campeonato', data);
    return response.data;
  },

  /**
   * Obtém inscrição pelo ID
   */
  async getById(id: number): Promise<InscricaoCampeonatoResponse> {
    const response = await api.get<InscricaoCampeonatoResponse>(`/inscricoes-campeonato/${id}`);
    return response.data;
  },

  /**
   * Obtém inscrições de um campeonato
   */
  async getByCampeonato(campeonatoId: number): Promise<InscricaoCampeonatoResponse[]> {
    const response = await api.get<InscricaoCampeonatoResponse[]>(
      `/inscricoes-campeonato/campeonato/${campeonatoId}`
    );
    return response.data;
  },

  /**
   * Obtém inscrições de um time
   */
  async getByTime(timeId: number): Promise<InscricaoCampeonatoResponse[]> {
    const response = await api.get<InscricaoCampeonatoResponse[]>(
      `/inscricoes-campeonato/time/${timeId}`
    );
    return response.data;
  },

  /**
   * Obtém inscrições por status (admin only)
   */
  async getByStatus(status: StatusInscricao): Promise<InscricaoCampeonatoResponse[]> {
    const response = await api.get<InscricaoCampeonatoResponse[]>(
      `/inscricoes-campeonato/status/${status}`
    );
    return response.data;
  },

  /**
   * Aprova uma inscrição (admin only)
   */
  async approve(id: number): Promise<InscricaoCampeonatoResponse> {
    const response = await api.post<InscricaoCampeonatoResponse>(
      `/inscricoes-campeonato/${id}/aprovar`
    );
    return response.data;
  },

  /**
   * Rejeita uma inscrição (admin only)
   */
  async reject(id: number): Promise<InscricaoCampeonatoResponse> {
    const response = await api.post<InscricaoCampeonatoResponse>(
      `/inscricoes-campeonato/${id}/rejeitar`
    );
    return response.data;
  },

  /**
   * Deleta uma inscrição (admin only)
   */
  async delete(id: number): Promise<void> {
    await api.delete(`/inscricoes-campeonato/${id}`);
  },
};

export default inscricoesService;
