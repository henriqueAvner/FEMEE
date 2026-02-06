import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { campeonatosService } from '@/services';
import type { 
  CampeonatoResponse, 
  CreateCampeonatoRequest,
  StatusCampeonato 
} from '@/types/api.types';

// ============================================
// QUERY KEYS
// ============================================

export const campeonatosKeys = {
  all: ['campeonatos'] as const,
  lists: () => [...campeonatosKeys.all, 'list'] as const,
  list: () => [...campeonatosKeys.lists()] as const,
  byStatus: (status: StatusCampeonato) => [...campeonatosKeys.all, 'status', status] as const,
  ativos: () => [...campeonatosKeys.all, 'ativos'] as const,
  details: () => [...campeonatosKeys.all, 'detail'] as const,
  detail: (id: number) => [...campeonatosKeys.details(), id] as const,
};

// ============================================
// QUERIES
// ============================================

/**
 * Hook para buscar todos os campeonatos
 */
export function useCampeonatos() {
  return useQuery({
    queryKey: campeonatosKeys.list(),
    queryFn: () => campeonatosService.getAll(),
  });
}

/**
 * Hook para buscar campeonato por ID
 */
export function useCampeonato(id: number) {
  return useQuery({
    queryKey: campeonatosKeys.detail(id),
    queryFn: () => campeonatosService.getById(id),
    enabled: !!id && id > 0,
  });
}

/**
 * Hook para buscar campeonatos por status
 */
export function useCampeonatosByStatus(status: StatusCampeonato) {
  return useQuery({
    queryKey: campeonatosKeys.byStatus(status),
    queryFn: () => campeonatosService.getByStatus(status),
  });
}

/**
 * Hook para buscar campeonatos ativos
 */
export function useCampeonatosAtivos() {
  return useQuery({
    queryKey: campeonatosKeys.ativos(),
    queryFn: () => campeonatosService.getAtivos(),
  });
}

// ============================================
// MUTATIONS
// ============================================

/**
 * Hook para criar campeonato
 */
export function useCreateCampeonato() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateCampeonatoRequest) => campeonatosService.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: campeonatosKeys.lists() });
      queryClient.invalidateQueries({ queryKey: campeonatosKeys.ativos() });
    },
  });
}

/**
 * Hook para atualizar campeonato
 */
export function useUpdateCampeonato() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: Partial<CreateCampeonatoRequest> }) =>
      campeonatosService.update(id, data),
    onSuccess: (updatedCampeonato) => {
      queryClient.setQueryData(campeonatosKeys.detail(updatedCampeonato.id), updatedCampeonato);
      queryClient.invalidateQueries({ queryKey: campeonatosKeys.lists() });
      queryClient.invalidateQueries({ queryKey: campeonatosKeys.ativos() });
    },
  });
}

/**
 * Hook para deletar campeonato
 */
export function useDeleteCampeonato() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => campeonatosService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: campeonatosKeys.all });
    },
  });
}
