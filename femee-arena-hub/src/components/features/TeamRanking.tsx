import { Trophy, TrendingUp } from "lucide-react";
import { Card } from "../ui/card";
import { Link } from "react-router-dom";
import { useTimesRanking } from "@/hooks/api";
import { LoadingSpinner } from "../ui/loading";
import { ErrorDisplay } from "../ui/error-display";

const TeamRanking = () => {
  const { data: times, isLoading, error, refetch } = useTimesRanking();

  // Pegar apenas os top 5 para a sidebar
  const topTeams = times?.slice(0, 5) ?? [];

  return (
    <Card className="bg-card border-border p-4 space-y-4">
      <Link to="/ranking" className="flex items-center gap-2 pb-3 border-b border-border hover:opacity-80 transition-opacity">
        <Trophy className="h-5 w-5 text-gold" />
        <h2 className="text-lg font-bold text-foreground">Ranking de Times</h2>
      </Link>
      
      {isLoading && (
        <div className="flex justify-center py-8">
          <LoadingSpinner size="md" />
        </div>
      )}

      {error && (
        <ErrorDisplay 
          title="Erro ao carregar ranking"
          message="Não foi possível carregar o ranking de times."
          onRetry={refetch}
          compact
        />
      )}

      {!isLoading && !error && topTeams.length === 0 && (
        <p className="text-sm text-muted-foreground text-center py-4">
          Nenhum time encontrado
        </p>
      )}

      {!isLoading && !error && topTeams.length > 0 && (
        <div className="space-y-3">
          {topTeams.map((team, index) => {
            const rank = index + 1;
            return (
              <Link 
                key={team.id}
                to={`/times/${team.slug}`}
                className="flex items-center gap-3 p-3 rounded-lg bg-secondary/50 hover:bg-secondary transition-colors cursor-pointer"
              >
                <div className={`flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center font-bold text-sm ${
                  rank === 1 ? 'bg-gold text-gold-foreground' :
                  rank === 2 ? 'bg-muted text-foreground' :
                  rank === 3 ? 'bg-accent/30 text-accent' :
                  'bg-muted/50 text-muted-foreground'
                }`}>
                  {rank}
                </div>
                
                <div className="flex-1 min-w-0">
                  <h3 className="font-semibold text-sm text-foreground truncate">
                    {team.nome}
                  </h3>
                  <p className="text-xs text-muted-foreground">
                    {team.vitorias ?? 0} vitórias
                  </p>
                </div>
                
                {rank <= 3 && (
                  <TrendingUp className="h-4 w-4 text-primary flex-shrink-0" />
                )}
              </Link>
            );
          })}
        </div>
      )}
      
      <Link to="/ranking" className="w-full block py-2 text-sm font-medium text-primary hover:text-primary/80 transition-colors text-center">
        Ver ranking completo →
      </Link>
    </Card>
  );
};

export default TeamRanking;
