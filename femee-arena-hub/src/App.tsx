import { lazy, Suspense } from "react";
import { Toaster } from "@/components/ui/toaster";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "@/contexts/AuthContext";
import { GuestRoute, ProtectedRoute, AdminRoute } from "@/components/auth/ProtectedRoute";
import { LoadingPage } from "@/components/ui/loading";

// Lazy loaded pages para melhor performance
const Index = lazy(() => import("./pages/Index"));
const Times = lazy(() => import("./pages/Times"));
const TeamDetail = lazy(() => import("./pages/TeamDetail"));
const Ranking = lazy(() => import("./pages/Ranking"));
const Campeonatos = lazy(() => import("./pages/Campeonatos"));
const Login = lazy(() => import("./pages/Login"));
const Registro = lazy(() => import("./pages/Registro"));
const Perfil = lazy(() => import("./pages/Perfil"));
const Admin = lazy(() => import("./pages/Admin"));
const NotFound = lazy(() => import("./pages/NotFound"));

// Configuração otimizada do QueryClient
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 5 * 60 * 1000, // 5 minutos - dados considerados frescos
      retry: 1, // Apenas 1 retry em caso de erro
      refetchOnWindowFocus: false, // Não refetch ao focar janela
    },
  },
});

const App = () => (
  <QueryClientProvider client={queryClient}>
    <AuthProvider>
      <TooltipProvider>
        <Toaster />
        <Sonner />
        <BrowserRouter>
          <Suspense fallback={<LoadingPage />}>
            <Routes>
              {/* Public Routes */}
              <Route path="/" element={<Index />} />
              <Route path="/times" element={<Times />} />
              <Route path="/times/:teamId" element={<TeamDetail />} />
              <Route path="/ranking" element={<Ranking />} />
              <Route path="/campeonatos" element={<Campeonatos />} />
              
              {/* Auth Routes - Only for guests */}
              <Route path="/login" element={
                <GuestRoute>
                  <Login />
                </GuestRoute>
              } />
              <Route path="/registro" element={
                <GuestRoute>
                  <Registro />
                </GuestRoute>
              } />
              
              {/* Protected Routes - Require authentication */}
              <Route path="/perfil" element={
                <ProtectedRoute>
                  <Perfil />
                </ProtectedRoute>
              } />
              
              {/* Admin Routes - Require admin role */}
              <Route path="/admin" element={
                <AdminRoute>
                  <Admin />
                </AdminRoute>
              } />
              
              {/* ADD ALL CUSTOM ROUTES ABOVE THE CATCH-ALL "*" ROUTE */}
              <Route path="*" element={<NotFound />} />
            </Routes>
          </Suspense>
        </BrowserRouter>
      </TooltipProvider>
    </AuthProvider>
  </QueryClientProvider>
);

export default App;
