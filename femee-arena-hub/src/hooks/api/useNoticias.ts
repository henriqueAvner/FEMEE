import { useQuery } from '@tanstack/react-query';
import { noticiasService } from '@/services';
import type { PaginationParams } from '@/types/api.types';

// ============================================
// QUERY KEYS
// ============================================

export const noticiasKeys = {
  all: ['noticias'] as const,
  lists: () => [...noticiasKeys.all, 'list'] as const,
  list: (params?: PaginationParams) => [...noticiasKeys.lists(), params] as const,
  recentes: (limit: number) => [...noticiasKeys.all, 'recentes', limit] as const,
  byCategoria: (categoria: string, params?: PaginationParams) => 
    [...noticiasKeys.all, 'categoria', categoria, params] as const,
  details: () => [...noticiasKeys.all, 'detail'] as const,
  detail: (id: number) => [...noticiasKeys.details(), id] as const,
  slug: (slug: string) => [...noticiasKeys.all, 'slug', slug] as const,
};

// ============================================
// QUERIES
// ============================================

/**
 * Hook para buscar notícias paginadas
 */
export function useNoticias(params?: PaginationParams) {
  return useQuery({
    queryKey: noticiasKeys.list(params),
    queryFn: () => noticiasService.getAll(params),
  });
}

/**
 * Hook para buscar notícias recentes (para homepage)
 */
export function useNoticiasRecentes(limit: number = 5) {
  return useQuery({
    queryKey: noticiasKeys.recentes(limit),
    queryFn: () => noticiasService.getRecentes(limit),
  });
}

/**
 * Hook para buscar notícia por ID
 */
export function useNoticia(id: number) {
  return useQuery({
    queryKey: noticiasKeys.detail(id),
    queryFn: () => noticiasService.getById(id),
    enabled: !!id && id > 0,
  });
}

/**
 * Hook para buscar notícia por slug
 */
export function useNoticiaBySlug(slug: string) {
  return useQuery({
    queryKey: noticiasKeys.slug(slug),
    queryFn: () => noticiasService.getBySlug(slug),
    enabled: !!slug && slug.length > 0,
  });
}

/**
 * Hook para buscar notícias por categoria
 */
export function useNoticiasByCategoria(categoria: string, params?: PaginationParams) {
  return useQuery({
    queryKey: noticiasKeys.byCategoria(categoria, params),
    queryFn: () => noticiasService.getByCategoria(categoria, params),
    enabled: !!categoria && categoria.length > 0,
  });
}
