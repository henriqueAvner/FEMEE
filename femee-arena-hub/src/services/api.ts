import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios';
import type { ApiError } from '@/types/api.types';

// Cria instância do Axios com configurações base
export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 30000, // 30 segundos
});

// ============================================
// REQUEST INTERCEPTOR
// ============================================

api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // Adiciona token de autenticação se existir
    const token = localStorage.getItem('femee_token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// ============================================
// RESPONSE INTERCEPTOR
// ============================================

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiError>) => {
    // Tratamento de erro 401 (não autorizado)
    if (error.response?.status === 401) {
      // Limpa token e redireciona para login
      localStorage.removeItem('femee_token');
      localStorage.removeItem('femee_user');
      
      // Só redireciona se não estiver na página de login
      if (!window.location.pathname.includes('/login')) {
        window.location.href = '/login';
      }
    }

    // Tratamento de erro 403 (proibido)
    if (error.response?.status === 403) {
      console.error('Acesso negado: você não tem permissão para esta ação');
    }

    // Tratamento de erro 500 (erro interno)
    if (error.response?.status === 500) {
      console.error('Erro interno do servidor');
    }

    return Promise.reject(error);
  }
);

// ============================================
// HELPERS
// ============================================

/**
 * Extrai mensagem de erro da resposta da API
 */
export function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError(error)) {
    const apiError = error.response?.data as ApiError | undefined;
    return apiError?.message || error.message || 'Erro desconhecido';
  }
  if (error instanceof Error) {
    return error.message;
  }
  return 'Erro desconhecido';
}

/**
 * Verifica se o usuário está autenticado
 */
export function isAuthenticated(): boolean {
  return !!localStorage.getItem('femee_token');
}

/**
 * Obtém o usuário atual do localStorage
 */
export function getCurrentUser() {
  const userJson = localStorage.getItem('femee_user');
  if (!userJson) return null;
  
  try {
    return JSON.parse(userJson);
  } catch {
    return null;
  }
}

export default api;
