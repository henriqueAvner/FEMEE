import { useState } from "react";
import { 
  Users, 
  Trophy, 
  Shield, 
  Settings, 
  BarChart3, 
  Activity,
  UserPlus,
  Calendar,
  AlertTriangle,
  CheckCircle2,
  Clock,
  TrendingUp,
  Gamepad2,
  Eye
} from "lucide-react";
import Header from "@/components/layout/Header";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { useAuth } from "@/contexts/AuthContext";
import { useNavigate } from "react-router-dom";

// Mock data
const dashboardStats = {
  totalUsuarios: 1250,
  usuariosAtivos: 890,
  totalTimes: 156,
  campeonatosAtivos: 8,
  inscricoesPendentes: 12,
  crescimentoMensal: 15.3,
};

const recentUsers = [
  { id: 1, nome: "João Silva", email: "joao@email.com", role: "Capitao", status: "Ativo", dataRegistro: "2024-01-15" },
  { id: 2, nome: "Maria Santos", email: "maria@email.com", role: "Membro", status: "Ativo", dataRegistro: "2024-01-14" },
  { id: 3, nome: "Pedro Oliveira", email: "pedro@email.com", role: "Membro", status: "Pendente", dataRegistro: "2024-01-14" },
  { id: 4, nome: "Ana Costa", email: "ana@email.com", role: "Capitao", status: "Ativo", dataRegistro: "2024-01-13" },
  { id: 5, nome: "Carlos Lima", email: "carlos@email.com", role: "Membro", status: "Ativo", dataRegistro: "2024-01-12" },
];

const pendingTeams = [
  { id: 1, nome: "Dragon Slayers", jogo: "League of Legends", capitao: "João Silva", dataSubmissao: "2024-01-15" },
  { id: 2, nome: "Phoenix Rising", jogo: "Valorant", capitao: "Maria Santos", dataSubmissao: "2024-01-14" },
  { id: 3, nome: "Thunder Squad", jogo: "CS2", capitao: "Pedro Costa", dataSubmissao: "2024-01-13" },
];

const activeChampionships = [
  { id: 1, nome: "Copa FEMEE 2024", jogo: "League of Legends", fase: "Quartas de Final", participantes: 32, status: "Em Andamento" },
  { id: 2, nome: "Valorant Masters", jogo: "Valorant", fase: "Grupos", participantes: 16, status: "Em Andamento" },
  { id: 3, nome: "CS2 Pro League", jogo: "CS2", fase: "Inscrições", participantes: 24, status: "Inscrições Abertas" },
];

const Admin = () => {
  const { user, isAuthenticated, isAdmin } = useAuth();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState("dashboard");

  // Check authentication and admin status
  if (!isAuthenticated || !isAdmin) {
    navigate("/");
    return null;
  }

  const StatCard = ({ 
    title, 
    value, 
    icon: Icon, 
    description, 
    trend 
  }: { 
    title: string; 
    value: string | number; 
    icon: React.ElementType; 
    description?: string;
    trend?: { value: number; positive: boolean };
  }) => (
    <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
      <CardContent className="pt-6">
        <div className="flex items-start justify-between">
          <div>
            <p className="text-sm text-muted-foreground">{title}</p>
            <p className="text-2xl font-bold mt-1">{value}</p>
            {description && (
              <p className="text-xs text-muted-foreground mt-1">{description}</p>
            )}
            {trend && (
              <div className={`flex items-center gap-1 mt-1 text-xs ${trend.positive ? 'text-green-500' : 'text-red-500'}`}>
                <TrendingUp className={`h-3 w-3 ${!trend.positive && 'rotate-180'}`} />
                <span>{trend.positive ? '+' : ''}{trend.value}%</span>
              </div>
            )}
          </div>
          <div className="p-3 rounded-full bg-femee-purple/10">
            <Icon className="h-5 w-5 text-femee-purple" />
          </div>
        </div>
      </CardContent>
    </Card>
  );

  const getRoleBadge = (role: string) => {
    switch (role) {
      case "Admin":
        return <Badge variant="destructive">Admin</Badge>;
      case "Capitao":
        return <Badge variant="default">Capitão</Badge>;
      default:
        return <Badge variant="secondary">Membro</Badge>;
    }
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Ativo":
        return <Badge className="bg-green-500/10 text-green-500 hover:bg-green-500/20">Ativo</Badge>;
      case "Pendente":
        return <Badge className="bg-yellow-500/10 text-yellow-500 hover:bg-yellow-500/20">Pendente</Badge>;
      case "Inativo":
        return <Badge variant="secondary">Inativo</Badge>;
      default:
        return <Badge variant="outline">{status}</Badge>;
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-femee-darker via-background to-femee-darker">
      <Header />

      <main className="container mx-auto px-4 py-8">
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold">Painel Administrativo</h1>
            <p className="text-muted-foreground">
              Bem-vindo, {user?.nome}. Gerencie a plataforma FEMEE.
            </p>
          </div>
          <Badge variant="destructive" className="text-sm">
            <Shield className="h-4 w-4 mr-1" />
            Admin
          </Badge>
        </div>

        <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-6">
          <TabsList className="grid w-full grid-cols-4 max-w-2xl">
            <TabsTrigger value="dashboard" className="gap-2">
              <BarChart3 className="h-4 w-4" />
              Dashboard
            </TabsTrigger>
            <TabsTrigger value="usuarios" className="gap-2">
              <Users className="h-4 w-4" />
              Usuários
            </TabsTrigger>
            <TabsTrigger value="times" className="gap-2">
              <Gamepad2 className="h-4 w-4" />
              Times
            </TabsTrigger>
            <TabsTrigger value="campeonatos" className="gap-2">
              <Trophy className="h-4 w-4" />
              Campeonatos
            </TabsTrigger>
          </TabsList>

          {/* Dashboard Tab */}
          <TabsContent value="dashboard" className="space-y-6">
            {/* Stats Grid */}
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
              <StatCard
                title="Total de Usuários"
                value={dashboardStats.totalUsuarios}
                icon={Users}
                trend={{ value: dashboardStats.crescimentoMensal, positive: true }}
              />
              <StatCard
                title="Usuários Ativos"
                value={dashboardStats.usuariosAtivos}
                icon={Activity}
                description="Últimos 30 dias"
              />
              <StatCard
                title="Times Cadastrados"
                value={dashboardStats.totalTimes}
                icon={Gamepad2}
              />
              <StatCard
                title="Campeonatos Ativos"
                value={dashboardStats.campeonatosAtivos}
                icon={Trophy}
              />
            </div>

            {/* Alerts */}
            {dashboardStats.inscricoesPendentes > 0 && (
              <Card className="bg-yellow-500/10 border-yellow-500/30">
                <CardContent className="pt-6">
                  <div className="flex items-center gap-3">
                    <AlertTriangle className="h-5 w-5 text-yellow-500" />
                    <div>
                      <p className="font-medium">Atenção: {dashboardStats.inscricoesPendentes} inscrições pendentes</p>
                      <p className="text-sm text-muted-foreground">
                        Existem times aguardando aprovação para campeonatos.
                      </p>
                    </div>
                    <Button size="sm" className="ml-auto">
                      Revisar
                    </Button>
                  </div>
                </CardContent>
              </Card>
            )}

            {/* Recent Activity */}
            <div className="grid gap-6 md:grid-cols-2">
              <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <UserPlus className="h-5 w-5" />
                    Últimos Registros
                  </CardTitle>
                  <CardDescription>Usuários registrados recentemente</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    {recentUsers.slice(0, 4).map((user) => (
                      <div key={user.id} className="flex items-center justify-between p-2 rounded-lg bg-muted/30">
                        <div>
                          <p className="font-medium">{user.nome}</p>
                          <p className="text-sm text-muted-foreground">{user.email}</p>
                        </div>
                        {getStatusBadge(user.status)}
                      </div>
                    ))}
                  </div>
                </CardContent>
              </Card>

              <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <Clock className="h-5 w-5" />
                    Times Pendentes
                  </CardTitle>
                  <CardDescription>Aguardando aprovação</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    {pendingTeams.map((team) => (
                      <div key={team.id} className="flex items-center justify-between p-2 rounded-lg bg-muted/30">
                        <div>
                          <p className="font-medium">{team.nome}</p>
                          <p className="text-sm text-muted-foreground">{team.jogo}</p>
                        </div>
                        <div className="flex gap-2">
                          <Button size="sm" variant="ghost" className="text-green-500 hover:text-green-600 hover:bg-green-500/10">
                            <CheckCircle2 className="h-4 w-4" />
                          </Button>
                          <Button size="sm" variant="ghost" className="text-red-500 hover:text-red-600 hover:bg-red-500/10">
                            <AlertTriangle className="h-4 w-4" />
                          </Button>
                        </div>
                      </div>
                    ))}
                  </div>
                </CardContent>
              </Card>
            </div>
          </TabsContent>

          {/* Users Tab */}
          <TabsContent value="usuarios">
            <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle>Gerenciamento de Usuários</CardTitle>
                    <CardDescription>
                      Visualize e gerencie todos os usuários da plataforma
                    </CardDescription>
                  </div>
                  <Button className="bg-femee-purple hover:bg-femee-purple/90">
                    <UserPlus className="h-4 w-4 mr-2" />
                    Adicionar Usuário
                  </Button>
                </div>
              </CardHeader>
              <CardContent>
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Nome</TableHead>
                      <TableHead>E-mail</TableHead>
                      <TableHead>Função</TableHead>
                      <TableHead>Status</TableHead>
                      <TableHead>Data de Registro</TableHead>
                      <TableHead className="text-right">Ações</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {recentUsers.map((user) => (
                      <TableRow key={user.id}>
                        <TableCell className="font-medium">{user.nome}</TableCell>
                        <TableCell>{user.email}</TableCell>
                        <TableCell>{getRoleBadge(user.role)}</TableCell>
                        <TableCell>{getStatusBadge(user.status)}</TableCell>
                        <TableCell>{new Date(user.dataRegistro).toLocaleDateString('pt-BR')}</TableCell>
                        <TableCell className="text-right">
                          <Button variant="ghost" size="sm">
                            <Eye className="h-4 w-4" />
                          </Button>
                          <Button variant="ghost" size="sm">
                            <Settings className="h-4 w-4" />
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </CardContent>
            </Card>
          </TabsContent>

          {/* Teams Tab */}
          <TabsContent value="times">
            <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle>Gerenciamento de Times</CardTitle>
                    <CardDescription>
                      Visualize e gerencie todos os times cadastrados
                    </CardDescription>
                  </div>
                  <Button className="bg-femee-purple hover:bg-femee-purple/90">
                    <Gamepad2 className="h-4 w-4 mr-2" />
                    Criar Time
                  </Button>
                </div>
              </CardHeader>
              <CardContent>
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Nome do Time</TableHead>
                      <TableHead>Jogo</TableHead>
                      <TableHead>Capitão</TableHead>
                      <TableHead>Data de Submissão</TableHead>
                      <TableHead className="text-right">Ações</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {pendingTeams.map((team) => (
                      <TableRow key={team.id}>
                        <TableCell className="font-medium">{team.nome}</TableCell>
                        <TableCell>{team.jogo}</TableCell>
                        <TableCell>{team.capitao}</TableCell>
                        <TableCell>{new Date(team.dataSubmissao).toLocaleDateString('pt-BR')}</TableCell>
                        <TableCell className="text-right">
                          <Button variant="ghost" size="sm" className="text-green-500 hover:text-green-600">
                            <CheckCircle2 className="h-4 w-4 mr-1" />
                            Aprovar
                          </Button>
                          <Button variant="ghost" size="sm" className="text-red-500 hover:text-red-600">
                            Recusar
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </CardContent>
            </Card>
          </TabsContent>

          {/* Championships Tab */}
          <TabsContent value="campeonatos">
            <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle>Gerenciamento de Campeonatos</CardTitle>
                    <CardDescription>
                      Visualize e gerencie todos os campeonatos
                    </CardDescription>
                  </div>
                  <Button className="bg-femee-purple hover:bg-femee-purple/90">
                    <Trophy className="h-4 w-4 mr-2" />
                    Criar Campeonato
                  </Button>
                </div>
              </CardHeader>
              <CardContent>
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Nome</TableHead>
                      <TableHead>Jogo</TableHead>
                      <TableHead>Fase</TableHead>
                      <TableHead>Participantes</TableHead>
                      <TableHead>Status</TableHead>
                      <TableHead className="text-right">Ações</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {activeChampionships.map((championship) => (
                      <TableRow key={championship.id}>
                        <TableCell className="font-medium">{championship.nome}</TableCell>
                        <TableCell>{championship.jogo}</TableCell>
                        <TableCell>{championship.fase}</TableCell>
                        <TableCell>{championship.participantes}</TableCell>
                        <TableCell>
                          <Badge variant={championship.status === "Em Andamento" ? "default" : "secondary"}>
                            {championship.status}
                          </Badge>
                        </TableCell>
                        <TableCell className="text-right">
                          <Button variant="ghost" size="sm">
                            <Eye className="h-4 w-4 mr-1" />
                            Ver
                          </Button>
                          <Button variant="ghost" size="sm">
                            <Settings className="h-4 w-4 mr-1" />
                            Editar
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
      </main>
    </div>
  );
};

export default Admin;
