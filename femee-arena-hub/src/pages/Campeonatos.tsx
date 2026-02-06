import Header from "@/components/layout/Header";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Calendar, Users, Trophy, MapPin } from "lucide-react";
import { useState } from "react";
import RegistrationDialog from "@/components/forms/RegistrationDialog";
import { useCampeonatos } from "@/hooks/api";
import { LoadingPage } from "@/components/ui/loading";
import { ErrorPage } from "@/components/ui/error-display";
import { EmptyState } from "@/components/ui/empty-state";
import { StatusCampeonato } from "@/types/api.types";

const Campeonatos = () => {
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [selectedCampeonatoId, setSelectedCampeonatoId] = useState<number | null>(null);
  const { data: campeonatos, isLoading, error, refetch } = useCampeonatos();

  const getStatusBadge = (status: StatusCampeonato) => {
    switch (status) {
      case StatusCampeonato.InscricoesAbertas:
        return <span className="px-2 py-1 text-xs font-semibold bg-primary/20 text-primary rounded">Inscrições Abertas</span>;
      case StatusCampeonato.EmAndamento:
        return <span className="px-2 py-1 text-xs font-semibold bg-accent/20 text-accent rounded">Em Andamento</span>;
      case StatusCampeonato.Finalizado:
        return <span className="px-2 py-1 text-xs font-semibold bg-muted text-muted-foreground rounded">Encerrado</span>;
      case StatusCampeonato.Cancelado:
        return <span className="px-2 py-1 text-xs font-semibold bg-destructive/20 text-destructive rounded">Cancelado</span>;
      default:
        return null;
    }
  };

  const handleInscrever = (campeonatoId: number) => {
    setSelectedCampeonatoId(campeonatoId);
    setIsDialogOpen(true);
  };

  if (isLoading) {
    return <LoadingPage message="Carregando campeonatos..." />;
  }

  if (error) {
    return (
      <ErrorPage
        title="Erro ao carregar campeonatos"
        message="Não foi possível carregar a lista de campeonatos. Tente novamente mais tarde."
        onRetry={refetch}
      />
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main className="container mx-auto px-4 py-8">
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-foreground mb-2">Campeonatos</h1>
          <p className="text-muted-foreground">Próximos torneios e competições da FEMEE</p>
        </div>

        {(!campeonatos || campeonatos.length === 0) ? (
          <EmptyState
            icon={Trophy}
            title="Nenhum campeonato encontrado"
            description="Ainda não há campeonatos cadastrados. Fique de olho para novidades!"
          />
        ) : (
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            {campeonatos.map((campeonato) => (
              <Card key={campeonato.id} className="p-6 hover:bg-secondary/30 transition-colors">
                <div className="flex justify-between items-start mb-4">
                  <div>
                    <h3 className="text-xl font-bold text-foreground mb-1">{campeonato.titulo}</h3>
                    <p className="text-sm text-muted-foreground">{campeonato.jogo?.nome || 'Não especificado'}</p>
                  </div>
                  {getStatusBadge(campeonato.status)}
                </div>

                <div className="space-y-3 mb-6">
                  <div className="flex items-center gap-2 text-sm">
                    <Calendar className="h-4 w-4 text-primary" />
                    <span className="text-foreground">
                      {new Date(campeonato.dataInicio).toLocaleDateString('pt-BR')}
                      {campeonato.dataFim && ` - ${new Date(campeonato.dataFim).toLocaleDateString('pt-BR')}`}
                    </span>
                  </div>
                  {campeonato.local && (
                    <div className="flex items-center gap-2 text-sm">
                      <MapPin className="h-4 w-4 text-primary" />
                      <span className="text-foreground">{campeonato.local}</span>
                    </div>
                  )}
                  <div className="flex items-center gap-2 text-sm">
                    <Users className="h-4 w-4 text-primary" />
                    <span className="text-foreground">
                      {campeonato.numeroInscritos ?? 0}/{campeonato.numeroVagas} times
                    </span>
                  </div>
                  {campeonato.premiacao && (
                    <div className="flex items-center gap-2 text-sm">
                      <Trophy className="h-4 w-4 text-gold" />
                      <span className="text-foreground font-semibold">R$ {campeonato.premiacao.toLocaleString('pt-BR')}</span>
                    </div>
                  )}
                </div>

                {campeonato.status === StatusCampeonato.InscricoesAbertas && (
                  <Button 
                    className="w-full esports-glow"
                    onClick={() => handleInscrever(campeonato.id)}
                  >
                    Inscrever-se
                  </Button>
                )}
                {campeonato.status === StatusCampeonato.Planejado && (
                  <Button variant="secondary" className="w-full" disabled>
                    Em Breve
                  </Button>
                )}
                {campeonato.status === StatusCampeonato.EmAndamento && (
                  <Button variant="secondary" className="w-full" disabled>
                    Em Andamento
                  </Button>
                )}
              </Card>
            ))}
          </div>
        )}
      </main>
      <RegistrationDialog 
        open={isDialogOpen} 
        onOpenChange={setIsDialogOpen}
        campeonatoId={selectedCampeonatoId}
      />
    </div>
  );
};

export default Campeonatos;
