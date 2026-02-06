import Header from "@/components/layout/Header";
import { Card } from "@/components/ui/card";
import { Link } from "react-router-dom";
import { Trophy } from "lucide-react";
import { useTimes } from "@/hooks/api";
import { LoadingPage } from "@/components/ui/loading";
import { ErrorPage } from "@/components/ui/error-display";
import { EmptyState } from "@/components/ui/empty-state";

const Times = () => {
  const { data: times, isLoading, error, refetch } = useTimes();

  if (isLoading) {
    return <LoadingPage message="Carregando times..." />;
  }

  if (error) {
    return (
      <ErrorPage
        title="Erro ao carregar times"
        message="Não foi possível carregar a lista de times. Tente novamente mais tarde."
        onRetry={refetch}
      />
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main className="container mx-auto px-4 py-8">
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-foreground mb-2">Times</h1>
          <p className="text-muted-foreground">Conheça os times participantes da FEMEE</p>
        </div>

        {(!times || times.length === 0) ? (
          <EmptyState
            icon={Trophy}
            title="Nenhum time encontrado"
            description="Ainda não há times cadastrados na plataforma."
          />
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {times.map((team) => {
              const wins = team.vitorias ?? 0;
              const losses = team.derrotas ?? 0;
              const total = wins + losses;
              const winRate = total > 0 ? ((wins / total) * 100).toFixed(0) : '0';

              return (
                <Link key={team.id} to={`/times/${team.slug}`}>
                  <Card className="p-6 hover:bg-secondary/50 transition-colors cursor-pointer">
                    <div className="flex items-center gap-4 mb-4">
                      {team.logoUrl ? (
                        <img 
                          src={team.logoUrl} 
                          alt={team.nome} 
                          className="w-16 h-16 rounded-full object-cover"
                        />
                      ) : (
                        <div className="w-16 h-16 rounded-full bg-primary/20 flex items-center justify-center">
                          <Trophy className="h-8 w-8 text-primary" />
                        </div>
                      )}
                      <div>
                        <h3 className="text-xl font-bold text-foreground">{team.nome}</h3>
                        <p className="text-sm text-muted-foreground">
                          {wins}V - {losses}D
                        </p>
                      </div>
                    </div>
                    <div className="flex gap-4 text-sm">
                      <div>
                        <p className="text-muted-foreground">Vitórias</p>
                        <p className="text-lg font-bold text-primary">{wins}</p>
                      </div>
                      <div>
                        <p className="text-muted-foreground">Derrotas</p>
                        <p className="text-lg font-bold text-foreground">{losses}</p>
                      </div>
                      <div>
                        <p className="text-muted-foreground">Taxa</p>
                        <p className="text-lg font-bold text-foreground">{winRate}%</p>
                      </div>
                    </div>
                  </Card>
                </Link>
              );
            })}
          </div>
        )}
      </main>
    </div>
  );
};

export default Times;
