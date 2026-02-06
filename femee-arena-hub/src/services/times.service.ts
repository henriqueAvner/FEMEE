import api from './api';
import type {
  TimeResponse,
  CreateTimeRequest,
  UpdateTimeRequest,
  PagedResult,
  PaginationParams,
} from '@/types/api.types';

export const timesService = {
  /**
   * Obtém todos os times (com paginação opcional)
   */
  async getAll(params?: PaginationParams): Promise<TimeResponse[]> {
    const response = await api.get<TimeResponse[]>('/times', { params });
    return response.data;
  },

  /**
   * Obtém todos os times paginados
   */
  async getAllPaged(params: PaginationParams): Promise<PagedResult<TimeResponse>> {
    const response = await api.get<PagedResult<TimeResponse>>('/times/paged', { params });
    return response.data;
  },

  /**
   * Obtém um time pelo ID
   */
  async getById(id: number): Promise<TimeResponse> {
    const response = await api.get<TimeResponse>(`/times/${id}`);
    return response.data;
  },

  /**
   * Obtém um time pelo slug
   */
  async getBySlug(slug: string): Promise<TimeResponse> {
    const response = await api.get<TimeResponse>(`/times/slug/${slug}`);
    return response.data;
  },

  /**
   * Obtém times ordenados por ranking
   */
  async getRanking(top?: number): Promise<TimeResponse[]> {
    const response = await api.get<TimeResponse[]>('/times/ranking', {
      params: top ? { top } : undefined,
    });
    return response.data;
  },

  /**
   * Cria um novo time (requer autenticação admin)
   */
  async create(data: CreateTimeRequest): Promise<TimeResponse> {
    const response = await api.post<TimeResponse>('/times', data);
    return response.data;
  },

  /**
   * Atualiza um time (requer autenticação admin/capitão)
   */
  async update(id: number, data: UpdateTimeRequest): Promise<TimeResponse> {
    const response = await api.put<TimeResponse>(`/times/${id}`, data);
    return response.data;
  },

  /**
   * Deleta um time (requer autenticação admin)
   */
  async delete(id: number): Promise<void> {
    await api.delete(`/times/${id}`);
  },
};

export default timesService;
