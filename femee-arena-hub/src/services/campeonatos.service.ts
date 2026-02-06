import api from './api';
import type {
  CampeonatoResponse,
  CreateCampeonatoRequest,
  StatusCampeonato,
} from '@/types/api.types';

export const campeonatosService = {
  /**
   * Obtém todos os campeonatos
   */
  async getAll(): Promise<CampeonatoResponse[]> {
    const response = await api.get<CampeonatoResponse[]>('/campeonatos');
    return response.data;
  },

  /**
   * Obtém um campeonato pelo ID
   */
  async getById(id: number): Promise<CampeonatoResponse> {
    const response = await api.get<CampeonatoResponse>(`/campeonatos/${id}`);
    return response.data;
  },

  /**
   * Obtém campeonatos por status
   */
  async getByStatus(status: StatusCampeonato): Promise<CampeonatoResponse[]> {
    const response = await api.get<CampeonatoResponse[]>(`/campeonatos/status/${status}`);
    return response.data;
  },

  /**
   * Obtém campeonatos ativos (em andamento ou agendados)
   */
  async getAtivos(): Promise<CampeonatoResponse[]> {
    const response = await api.get<CampeonatoResponse[]>('/campeonatos/ativos');
    return response.data;
  },

  /**
   * Cria um novo campeonato (requer autenticação admin)
   */
  async create(data: CreateCampeonatoRequest): Promise<CampeonatoResponse> {
    const response = await api.post<CampeonatoResponse>('/campeonatos', data);
    return response.data;
  },

  /**
   * Atualiza um campeonato (requer autenticação admin)
   */
  async update(id: number, data: Partial<CreateCampeonatoRequest>): Promise<CampeonatoResponse> {
    const response = await api.put<CampeonatoResponse>(`/campeonatos/${id}`, data);
    return response.data;
  },

  /**
   * Deleta um campeonato (requer autenticação admin)
   */
  async delete(id: number): Promise<void> {
    await api.delete(`/campeonatos/${id}`);
  },
};

export default campeonatosService;
