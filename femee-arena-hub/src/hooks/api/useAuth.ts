import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { authService } from '@/services';
import type { LoginRequest, RegisterRequest, AuthUser } from '@/types/api.types';

// ============================================
// QUERY KEYS
// ============================================

export const authKeys = {
  user: ['auth', 'user'] as const,
  me: ['auth', 'me'] as const,
};

// ============================================
// QUERIES
// ============================================

/**
 * Hook para obter dados do usuário autenticado
 */
export function useCurrentUser() {
  return useQuery({
    queryKey: authKeys.me,
    queryFn: () => authService.getMe(),
    enabled: authService.isAuthenticated(),
    staleTime: 10 * 60 * 1000, // 10 minutos
    retry: false,
  });
}

/**
 * Hook para verificar se está autenticado
 */
export function useIsAuthenticated(): boolean {
  return authService.isAuthenticated();
}

/**
 * Hook para obter usuário do localStorage (sem fetch)
 */
export function useStoredUser(): AuthUser | null {
  return authService.getStoredUser();
}

// ============================================
// MUTATIONS
// ============================================

/**
 * Hook para realizar login
 */
export function useLogin() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: LoginRequest) => authService.login(data),
    onSuccess: (response) => {
      // Atualiza cache do usuário
      queryClient.setQueryData(authKeys.me, {
        userId: response.userId,
        email: response.email,
        nome: response.nome,
        tipoUsuario: response.tipoUsuario,
      });
    },
  });
}

/**
 * Hook para realizar registro
 */
export function useRegister() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: RegisterRequest) => authService.register(data),
    onSuccess: (response) => {
      // Atualiza cache do usuário
      queryClient.setQueryData(authKeys.me, {
        userId: response.userId,
        email: response.email,
        nome: response.nome,
        tipoUsuario: response.tipoUsuario,
      });
    },
  });
}

/**
 * Hook para realizar logout
 */
export function useLogout() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async () => {
      authService.logout();
    },
    onSuccess: () => {
      // Limpa todo o cache de autenticação
      queryClient.removeQueries({ queryKey: authKeys.me });
      queryClient.removeQueries({ queryKey: authKeys.user });
      // Invalida queries que podem depender de autenticação
      queryClient.invalidateQueries();
    },
  });
}
