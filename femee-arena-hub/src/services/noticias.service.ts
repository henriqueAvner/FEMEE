import api from './api';
import type {
  NoticiaResponse,
  PagedResult,
  PaginationParams,
} from '@/types/api.types';

export const noticiasService = {
  /**
   * Obtém todas as notícias paginadas
   */
  async getAll(params?: PaginationParams): Promise<PagedResult<NoticiaResponse>> {
    const response = await api.get<PagedResult<NoticiaResponse>>('/noticias', {
      params: {
        page: params?.page || 1,
        pageSize: params?.pageSize || 10,
      },
    });
    return response.data;
  },

  /**
   * Obtém uma notícia pelo ID
   */
  async getById(id: number): Promise<NoticiaResponse> {
    const response = await api.get<NoticiaResponse>(`/noticias/${id}`);
    return response.data;
  },

  /**
   * Obtém uma notícia pelo slug
   */
  async getBySlug(slug: string): Promise<NoticiaResponse> {
    const response = await api.get<NoticiaResponse>(`/noticias/slug/${slug}`);
    return response.data;
  },

  /**
   * Obtém notícias por categoria
   */
  async getByCategoria(categoria: string, params?: PaginationParams): Promise<PagedResult<NoticiaResponse>> {
    const response = await api.get<PagedResult<NoticiaResponse>>(`/noticias/categoria/${categoria}`, {
      params,
    });
    return response.data;
  },

  /**
   * Obtém notícias recentes (para homepage)
   */
  async getRecentes(limit: number = 5): Promise<NoticiaResponse[]> {
    const response = await api.get<PagedResult<NoticiaResponse>>('/noticias', {
      params: { page: 1, pageSize: limit },
    });
    return response.data.items;
  },
};

export default noticiasService;
