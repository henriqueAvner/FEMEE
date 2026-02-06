import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { inscricoesService } from '@/services';
import type { CreateInscricaoCampeonatoRequest, StatusInscricao } from '@/types/api.types';
import { campeonatosKeys } from './useCampeonatos';

// ============================================
// QUERY KEYS
// ============================================

export const inscricoesKeys = {
  all: ['inscricoes'] as const,
  byCampeonato: (campeonatoId: number) => [...inscricoesKeys.all, 'campeonato', campeonatoId] as const,
  byTime: (timeId: number) => [...inscricoesKeys.all, 'time', timeId] as const,
  byStatus: (status: StatusInscricao) => [...inscricoesKeys.all, 'status', status] as const,
  detail: (id: number) => [...inscricoesKeys.all, 'detail', id] as const,
};

// ============================================
// QUERIES
// ============================================

/**
 * Hook para buscar inscrições de um campeonato
 */
export function useInscricoesByCampeonato(campeonatoId: number) {
  return useQuery({
    queryKey: inscricoesKeys.byCampeonato(campeonatoId),
    queryFn: () => inscricoesService.getByCampeonato(campeonatoId),
    enabled: !!campeonatoId && campeonatoId > 0,
  });
}

/**
 * Hook para buscar inscrições de um time
 */
export function useInscricoesByTime(timeId: number) {
  return useQuery({
    queryKey: inscricoesKeys.byTime(timeId),
    queryFn: () => inscricoesService.getByTime(timeId),
    enabled: !!timeId && timeId > 0,
  });
}

/**
 * Hook para buscar inscrição por ID
 */
export function useInscricao(id: number) {
  return useQuery({
    queryKey: inscricoesKeys.detail(id),
    queryFn: () => inscricoesService.getById(id),
    enabled: !!id && id > 0,
  });
}

// ============================================
// MUTATIONS
// ============================================

/**
 * Hook para criar inscrição em campeonato
 */
export function useCreateInscricao() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateInscricaoCampeonatoRequest) => inscricoesService.create(data),
    onSuccess: (inscricao) => {
      // Invalida caches relacionados
      queryClient.invalidateQueries({ 
        queryKey: inscricoesKeys.byCampeonato(inscricao.campeonatoId) 
      });
      queryClient.invalidateQueries({ 
        queryKey: inscricoesKeys.byTime(inscricao.timeId) 
      });
      // Atualiza contagem de inscritos no campeonato
      queryClient.invalidateQueries({ 
        queryKey: campeonatosKeys.detail(inscricao.campeonatoId) 
      });
    },
  });
}

/**
 * Hook para aprovar inscrição (admin)
 */
export function useApproveInscricao() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => inscricoesService.approve(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: inscricoesKeys.all });
    },
  });
}

/**
 * Hook para rejeitar inscrição (admin)
 */
export function useRejectInscricao() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => inscricoesService.reject(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: inscricoesKeys.all });
    },
  });
}

/**
 * Hook para deletar inscrição (admin)
 */
export function useDeleteInscricao() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => inscricoesService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: inscricoesKeys.all });
    },
  });
}
