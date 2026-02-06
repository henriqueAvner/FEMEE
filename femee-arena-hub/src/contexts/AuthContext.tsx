import React, { createContext, useContext, useEffect, useState, useCallback } from 'react';
import { authService } from '@/services';
import type { AuthUser, LoginRequest, RegisterRequest, LoginResponse } from '@/types/api.types';
import { TipoUsuario } from '@/types/api.types';

// ============================================
// TYPES
// ============================================

interface AuthContextType {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (data: LoginRequest) => Promise<LoginResponse>;
  register: (data: RegisterRequest) => Promise<LoginResponse>;
  logout: () => void;
  isAdmin: boolean;
  isCapitao: boolean;
}

// ============================================
// CONTEXT
// ============================================

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// ============================================
// PROVIDER
// ============================================

interface AuthProviderProps {
  children: React.ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Carrega usuário do localStorage ao iniciar
  useEffect(() => {
    const storedUser = authService.getStoredUser();
    if (storedUser && authService.isAuthenticated()) {
      setUser(storedUser);
    }
    setIsLoading(false);
  }, []);

  // Login
  const login = useCallback(async (data: LoginRequest): Promise<LoginResponse> => {
    setIsLoading(true);
    try {
      const response = await authService.login(data);
      setUser({
        userId: response.userId,
        email: response.email,
        nome: response.nome,
        tipoUsuario: response.tipoUsuario,
      });
      return response;
    } finally {
      setIsLoading(false);
    }
  }, []);

  // Registro
  const register = useCallback(async (data: RegisterRequest): Promise<LoginResponse> => {
    setIsLoading(true);
    try {
      const response = await authService.register(data);
      setUser({
        userId: response.userId,
        email: response.email,
        nome: response.nome,
        tipoUsuario: response.tipoUsuario,
      });
      return response;
    } finally {
      setIsLoading(false);
    }
  }, []);

  // Logout
  const logout = useCallback(() => {
    authService.logout();
    setUser(null);
  }, []);

  // Computed values
  const isAuthenticated = !!user && authService.isAuthenticated();
  const isAdmin = user?.tipoUsuario === TipoUsuario.Administrador;
  const isCapitao = user?.tipoUsuario === TipoUsuario.Capitao;

  const value: AuthContextType = {
    user,
    isAuthenticated,
    isLoading,
    login,
    register,
    logout,
    isAdmin,
    isCapitao,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

// ============================================
// HOOK
// ============================================

export function useAuth(): AuthContextType {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}

export default AuthContext;
