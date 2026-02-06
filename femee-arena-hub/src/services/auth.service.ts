import api from './api';
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  AuthUser,
} from '@/types/api.types';

const AUTH_TOKEN_KEY = 'femee_token';
const AUTH_USER_KEY = 'femee_user';

export const authService = {
  /**
   * Realiza login do usuário
   */
  async login(data: LoginRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>('/auth/login', data);
    
    // Salva token e dados do usuário
    if (response.data.token) {
      localStorage.setItem(AUTH_TOKEN_KEY, response.data.token);
      localStorage.setItem(AUTH_USER_KEY, JSON.stringify({
        userId: response.data.userId,
        email: response.data.email,
        nome: response.data.nome,
        tipoUsuario: response.data.tipoUsuario,
      }));
    }
    
    return response.data;
  },

  /**
   * Registra novo usuário
   */
  async register(data: RegisterRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>('/auth/register', data);
    
    // Salva token e dados do usuário após registro
    if (response.data.token) {
      localStorage.setItem(AUTH_TOKEN_KEY, response.data.token);
      localStorage.setItem(AUTH_USER_KEY, JSON.stringify({
        userId: response.data.userId,
        email: response.data.email,
        nome: response.data.nome,
        tipoUsuario: response.data.tipoUsuario,
      }));
    }
    
    return response.data;
  },

  /**
   * Obtém dados do usuário autenticado
   */
  async getMe(): Promise<AuthUser> {
    const response = await api.get<AuthUser>('/auth/me');
    return response.data;
  },

  /**
   * Realiza logout do usuário
   */
  logout(): void {
    localStorage.removeItem(AUTH_TOKEN_KEY);
    localStorage.removeItem(AUTH_USER_KEY);
  },

  /**
   * Verifica se o usuário está autenticado
   */
  isAuthenticated(): boolean {
    return !!localStorage.getItem(AUTH_TOKEN_KEY);
  },

  /**
   * Obtém o token atual
   */
  getToken(): string | null {
    return localStorage.getItem(AUTH_TOKEN_KEY);
  },

  /**
   * Obtém dados do usuário do localStorage
   */
  getStoredUser(): AuthUser | null {
    const userJson = localStorage.getItem(AUTH_USER_KEY);
    if (!userJson) return null;
    
    try {
      return JSON.parse(userJson);
    } catch {
      return null;
    }
  },
};

export default authService;
