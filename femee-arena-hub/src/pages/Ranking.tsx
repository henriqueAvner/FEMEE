import Header from "@/components/layout/Header";
import { Card } from "@/components/ui/card";
import { Trophy, TrendingUp, TrendingDown, Minus } from "lucide-react";
import { Link } from "react-router-dom";
import { useTimesRanking } from "@/hooks/api";
import { LoadingPage } from "@/components/ui/loading";
import { ErrorPage } from "@/components/ui/error-display";
import { EmptyState } from "@/components/ui/empty-state";

const getTrendIcon = (current: number, previous: number) => {
  if (current < previous) return <TrendingUp className="h-4 w-4 text-primary" />;
  if (current > previous) return <TrendingDown className="h-4 w-4 text-destructive" />;
  return <Minus className="h-4 w-4 text-muted-foreground" />;
};

const Ranking = () => {
  const { data: times, isLoading, error, refetch } = useTimesRanking();

  if (isLoading) {
    return <LoadingPage message="Carregando ranking..." />;
  }

  if (error) {
    return (
      <ErrorPage
        title="Erro ao carregar ranking"
        message="Não foi possível carregar o ranking de times. Tente novamente mais tarde."
        onRetry={refetch}
      />
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main className="container mx-auto px-4 py-8">
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-foreground mb-2 flex items-center gap-3">
            <Trophy className="h-10 w-10 text-gold" />
            Ranking de Times
          </h1>
          <p className="text-muted-foreground">Classificação geral dos times da FEMEE</p>
        </div>

        {(!times || times.length === 0) ? (
          <EmptyState
            icon={Trophy}
            title="Nenhum time no ranking"
            description="Ainda não há times classificados no ranking."
          />
        ) : (
          <Card className="overflow-hidden">
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead className="bg-secondary/50 border-b border-border">
                  <tr>
                    <th className="px-4 py-3 text-left text-sm font-semibold text-foreground">#</th>
                    <th className="px-4 py-3 text-left text-sm font-semibold text-foreground">Time</th>
                    <th className="px-4 py-3 text-center text-sm font-semibold text-foreground">V</th>
                    <th className="px-4 py-3 text-center text-sm font-semibold text-foreground">D</th>
                    <th className="px-4 py-3 text-center text-sm font-semibold text-foreground">Pontos</th>
                    <th className="px-4 py-3 text-center text-sm font-semibold text-foreground">Tendência</th>
                  </tr>
                </thead>
                <tbody>
                  {times.map((team, index) => {
                    const rank = index + 1;
                    const wins = team.vitorias ?? 0;
                    const losses = team.derrotas ?? 0;
                    const points = team.pontos ?? (wins * 3);
                    // Posição anterior simulada (em produção viria do backend)
                    const previousRank = rank;

                    return (
                      <tr 
                        key={team.id}
                        className="border-b border-border hover:bg-secondary/30 transition-colors"
                      >
                        <td className="px-4 py-4">
                          <div className={`w-8 h-8 rounded-full flex items-center justify-center font-bold text-sm ${
                            rank === 1 ? 'bg-gold text-gold-foreground' :
                            rank === 2 ? 'bg-muted text-foreground' :
                            rank === 3 ? 'bg-accent/30 text-accent' :
                            'bg-muted/50 text-muted-foreground'
                          }`}>
                            {rank}
                          </div>
                        </td>
                        <td className="px-4 py-4">
                          <Link 
                            to={`/times/${team.slug}`}
                            className="font-semibold text-foreground hover:text-primary transition-colors"
                          >
                            {team.nome}
                          </Link>
                        </td>
                        <td className="px-4 py-4 text-center text-primary font-semibold">{wins}</td>
                        <td className="px-4 py-4 text-center text-muted-foreground">{losses}</td>
                        <td className="px-4 py-4 text-center font-bold text-foreground">{points}</td>
                        <td className="px-4 py-4 text-center">
                          <div className="flex justify-center">
                            {getTrendIcon(rank, previousRank)}
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </Card>
        )}
      </main>
    </div>
  );
};

export default Ranking;
