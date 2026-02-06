import Header from "@/components/layout/Header";
import { Card } from "@/components/ui/card";
import { useParams, Link } from "react-router-dom";
import { Trophy, Users, Calendar, Award } from "lucide-react";
import { useTimeBySlug } from "@/hooks/api";
import { LoadingPage } from "@/components/ui/loading";
import { ErrorPage } from "@/components/ui/error-display";
import { EmptyState } from "@/components/ui/empty-state";

const TeamDetail = () => {
  const { teamId } = useParams();
  const { data: team, isLoading, error, refetch } = useTimeBySlug(teamId || '');

  if (isLoading) {
    return <LoadingPage message="Carregando informações do time..." />;
  }

  if (error) {
    return (
      <ErrorPage
        title="Erro ao carregar time"
        message="Não foi possível carregar as informações do time. Tente novamente mais tarde."
        onRetry={refetch}
      />
    );
  }

  if (!team) {
    return (
      <div className="min-h-screen bg-background">
        <Header />
        <main className="container mx-auto px-4 py-8">
          <Link to="/times" className="text-primary hover:underline mb-4 inline-block">
            ← Voltar para Times
          </Link>
          <EmptyState
            icon={Trophy}
            title="Time não encontrado"
            description="O time que você está procurando não existe ou foi removido."
            action={{
              label: "Ver todos os times",
              href: "/times"
            }}
          />
        </main>
      </div>
    );
  }

  const wins = team.vitorias ?? 0;
  const losses = team.derrotas ?? 0;
  const total = wins + losses;
  const winRate = total > 0 ? ((wins / total) * 100).toFixed(0) : '0';
  const titles = team.conquistas?.filter(c => c.posicao === 1).length ?? 0;

  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main className="container mx-auto px-4 py-8">
        <Link to="/times" className="text-primary hover:underline mb-4 inline-block">
          ← Voltar para Times
        </Link>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2 space-y-6">
            <Card className="p-6">
              <div className="flex items-center gap-4 mb-6">
                {team.logoUrl ? (
                  <img 
                    src={team.logoUrl} 
                    alt={team.nome} 
                    className="w-24 h-24 rounded-full object-cover"
                  />
                ) : (
                  <div className="w-24 h-24 rounded-full bg-primary/20 flex items-center justify-center">
                    <Trophy className="h-12 w-12 text-primary" />
                  </div>
                )}
                <div>
                  <h1 className="text-3xl font-bold text-foreground">{team.nome}</h1>
                  <p className="text-muted-foreground">
                    {team.descricao || "Time Profissional de eSports"}
                  </p>
                </div>
              </div>

              <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
                <div className="text-center p-4 bg-secondary/50 rounded-lg">
                  <p className="text-3xl font-bold text-primary">{wins}</p>
                  <p className="text-sm text-muted-foreground">Vitórias</p>
                </div>
                <div className="text-center p-4 bg-secondary/50 rounded-lg">
                  <p className="text-3xl font-bold text-foreground">{losses}</p>
                  <p className="text-sm text-muted-foreground">Derrotas</p>
                </div>
                <div className="text-center p-4 bg-secondary/50 rounded-lg">
                  <p className="text-3xl font-bold text-primary">{winRate}%</p>
                  <p className="text-sm text-muted-foreground">Taxa</p>
                </div>
                <div className="text-center p-4 bg-secondary/50 rounded-lg">
                  <p className="text-3xl font-bold text-gold">{titles}</p>
                  <p className="text-sm text-muted-foreground">Títulos</p>
                </div>
              </div>
            </Card>

            <Card className="p-6">
              <h2 className="text-xl font-bold text-foreground mb-4 flex items-center gap-2">
                <Users className="h-5 w-5 text-primary" />
                Elenco
              </h2>
              {team.jogadores && team.jogadores.length > 0 ? (
                <div className="space-y-3">
                  {team.jogadores.map((jogador) => (
                    <div key={jogador.id} className="flex items-center gap-3 p-3 bg-secondary/50 rounded-lg">
                      <div className="w-10 h-10 rounded-full bg-primary/20 flex items-center justify-center text-primary font-bold">
                        {jogador.nickname.charAt(0).toUpperCase()}
                      </div>
                      <div>
                        <p className="font-semibold text-foreground">{jogador.nickname}</p>
                        <p className="text-sm text-muted-foreground">
                          Função: {jogador.funcao || 'Player'}
                        </p>
                      </div>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-muted-foreground text-center py-4">
                  Nenhum jogador cadastrado
                </p>
              )}
            </Card>
          </div>

          <div className="space-y-6">
            <Card className="p-6">
              <h2 className="text-xl font-bold text-foreground mb-4 flex items-center gap-2">
                <Award className="h-5 w-5 text-gold" />
                Conquistas
              </h2>
              {team.conquistas && team.conquistas.length > 0 ? (
                <div className="space-y-3">
                  {team.conquistas.map((conquista, index) => (
                    <div 
                      key={conquista.id || index} 
                      className={`p-3 rounded-lg border ${
                        conquista.posicao === 1 
                          ? 'bg-gold/10 border-gold/20' 
                          : 'bg-secondary/50 border-transparent'
                      }`}
                    >
                      <p className="font-semibold text-foreground">
                        {conquista.posicao === 1 ? '🏆' : conquista.posicao === 2 ? '🥈' : '🥉'} {conquista.titulo}
                      </p>
                      <p className="text-sm text-muted-foreground">{conquista.campeonato}</p>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-muted-foreground text-center py-4">
                  Nenhuma conquista registrada
                </p>
              )}
            </Card>

            <Card className="p-6">
              <h2 className="text-xl font-bold text-foreground mb-4 flex items-center gap-2">
                <Calendar className="h-5 w-5 text-primary" />
                Próximas Partidas
              </h2>
              {team.proximasPartidas && team.proximasPartidas.length > 0 ? (
                <div className="space-y-3">
                  {team.proximasPartidas.map((partida, index) => (
                    <div key={partida.id || index} className="p-3 bg-secondary/50 rounded-lg">
                      <p className="font-semibold text-foreground">vs {partida.adversario}</p>
                      <p className="text-sm text-muted-foreground">
                        {new Date(partida.dataHora).toLocaleDateString('pt-BR')} - {new Date(partida.dataHora).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' })}
                      </p>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-muted-foreground text-center py-4">
                  Nenhuma partida agendada
                </p>
              )}
            </Card>
          </div>
        </div>
      </main>
    </div>
  );
};

export default TeamDetail;
