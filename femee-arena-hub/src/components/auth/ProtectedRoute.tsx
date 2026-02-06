import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { LoadingPage } from "@/components/ui/loading";
import { TipoUsuario } from "@/types/api.types";

interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRoles?: TipoUsuario[];
  redirectTo?: string;
}

/**
 * Componente para proteger rotas que requerem autenticação.
 * 
 * @param children - Componentes filhos a serem renderizados se autorizado
 * @param requiredRoles - Lista de roles permitidos (opcional)
 * @param redirectTo - Rota para redirecionar se não autenticado (default: /login)
 */
export const ProtectedRoute = ({ 
  children, 
  requiredRoles,
  redirectTo = "/login" 
}: ProtectedRouteProps) => {
  const { isAuthenticated, user, isLoading } = useAuth();
  const location = useLocation();

  // Mostra loading enquanto verifica autenticação
  if (isLoading) {
    return <LoadingPage message="Verificando autenticação..." />;
  }

  // Redireciona para login se não autenticado
  if (!isAuthenticated || !user) {
    return <Navigate to={redirectTo} state={{ from: location }} replace />;
  }

  // Verifica se o usuário tem a role necessária
  if (requiredRoles && requiredRoles.length > 0) {
    const hasRequiredRole = requiredRoles.includes(user.tipoUsuario);
    
    if (!hasRequiredRole) {
      return <Navigate to="/" replace />;
    }
  }

  return <>{children}</>;
};

/**
 * Componente para rotas que só devem ser acessíveis por usuários não autenticados.
 * Ex: Login, Registro
 */
interface GuestRouteProps {
  children: React.ReactNode;
  redirectTo?: string;
}

export const GuestRoute = ({ 
  children, 
  redirectTo = "/" 
}: GuestRouteProps) => {
  const { isAuthenticated, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return <LoadingPage message="Carregando..." />;
  }

  // Se já está autenticado, redireciona
  if (isAuthenticated) {
    const from = (location.state as { from?: { pathname: string } })?.from?.pathname || redirectTo;
    return <Navigate to={from} replace />;
  }

  return <>{children}</>;
};

/**
 * Componente para rotas que requerem role de Admin
 */
export const AdminRoute = ({ children }: { children: React.ReactNode }) => {
  return (
    <ProtectedRoute requiredRoles={[TipoUsuario.Administrador]}>
      {children}
    </ProtectedRoute>
  );
};

/**
 * Componente para rotas que requerem role de Capitão ou Admin
 */
export const CapitaoRoute = ({ children }: { children: React.ReactNode }) => {
  return (
    <ProtectedRoute requiredRoles={[TipoUsuario.Administrador, TipoUsuario.Capitao]}>
      {children}
    </ProtectedRoute>
  );
};

export default ProtectedRoute;
