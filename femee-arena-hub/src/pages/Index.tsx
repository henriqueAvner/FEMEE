import Header from "@/components/layout/Header";
import NewsCard from "@/components/features/NewsCard";
import TeamRanking from "@/components/features/TeamRanking";
import MerchandiseAd from "@/components/features/MerchandiseAd";
import ChampionshipCard from "@/components/features/ChampionshipCard";
import heroBanner from "@/assets/hero-banner.jpg";
import { Button } from "@/components/ui/button";
import { Newspaper, Trophy } from "lucide-react";
import { useNoticiasRecentes, useCampeonatosAtivos } from "@/hooks/api";
import { LoadingSpinner } from "@/components/ui/loading";
import { ErrorDisplay } from "@/components/ui/error-display";
import { EmptyState } from "@/components/ui/empty-state";
import { Link } from "react-router-dom";
import { StatusCampeonato } from "@/types/api.types";

const Index = () => {
  const { 
    data: noticias, 
    isLoading: isLoadingNoticias, 
    error: errorNoticias,
    refetch: refetchNoticias 
  } = useNoticiasRecentes(4);
  
  const { 
    data: campeonatos, 
    isLoading: isLoadingCampeonatos, 
    error: errorCampeonatos,
    refetch: refetchCampeonatos 
  } = useCampeonatosAtivos();

  // Mapear status do backend para o formato esperado pelo ChampionshipCard
  const mapStatus = (status: StatusCampeonato): "registration-open" | "upcoming" | "ongoing" => {
    switch (status) {
      case StatusCampeonato.InscricoesAbertas:
        return "registration-open";
      case StatusCampeonato.EmAndamento:
        return "ongoing";
      default:
        return "upcoming";
    }
  };

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      {/* Hero Section */}
      <section className="relative h-[500px] overflow-hidden">
        <div className="absolute inset-0">
          <img 
            src={heroBanner}
            alt="FEMEE Championship Arena"
            className="w-full h-full object-cover"
          />
          <div className="absolute inset-0 bg-gradient-to-t from-background via-background/60 to-transparent" />
        </div>
        
        <div className="relative container h-full flex items-end pb-12 px-4">
          <div className="max-w-3xl space-y-4">
            <h1 className="text-5xl md:text-6xl font-bold text-foreground">
              Esportes Eletrônicos de <span className="text-primary">Minas Gerais</span>
            </h1>
            <p className="text-xl text-muted-foreground">
              A casa dos maiores campeonatos de esports do estado
            </p>
            <div className="flex gap-3 pt-4">
              <Link to="/campeonatos">
                <Button size="lg" className="esports-glow">
                  Ver Campeonatos
                </Button>
              </Link>
              <Button size="lg" variant="secondary">
                Sobre a FEMEE
              </Button>
            </div>
          </div>
        </div>
      </section>

      {/* Main Content */}
      <div className="container py-12 px-4">
        <div className="grid grid-cols-1 lg:grid-cols-12 gap-8">
          {/* Left Sidebar - Team Ranking */}
          <aside className="lg:col-span-3 space-y-6">
            <TeamRanking />
          </aside>

          {/* Main Content Area */}
          <main className="lg:col-span-6 space-y-12">
            {/* Latest News Section */}
            <section id="noticias">
              <div className="flex items-center gap-2 mb-6">
                <Newspaper className="h-6 w-6 text-primary" />
                <h2 className="text-3xl font-bold text-foreground">Últimas Notícias</h2>
              </div>
              
              {isLoadingNoticias && (
                <div className="flex justify-center py-12">
                  <LoadingSpinner size="lg" />
                </div>
              )}

              {errorNoticias && (
                <ErrorDisplay
                  title="Erro ao carregar notícias"
                  message="Não foi possível carregar as notícias."
                  onRetry={refetchNoticias}
                />
              )}

              {!isLoadingNoticias && !errorNoticias && (!noticias || noticias.length === 0) && (
                <EmptyState
                  icon={Newspaper}
                  title="Nenhuma notícia"
                  description="Ainda não há notícias publicadas."
                />
              )}

              {!isLoadingNoticias && !errorNoticias && noticias && noticias.length > 0 && (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  {noticias.map((noticia) => (
                    <NewsCard 
                      key={noticia.id}
                      title={noticia.titulo}
                      excerpt={noticia.resumo || noticia.conteudo.substring(0, 150) + '...'}
                      date={new Date(noticia.dataPublicacao).toLocaleDateString('pt-BR', {
                        day: '2-digit',
                        month: 'short',
                        year: 'numeric'
                      })}
                      comments={0}
                      category={noticia.categoria || 'Notícias'}
                      image={noticia.imagemUrl || noticia.imagemCapa}
                    />
                  ))}
                </div>
              )}
            </section>

            {/* Upcoming Championships Section */}
            <section id="campeonatos">
              <div className="flex items-center gap-2 mb-6">
                <Trophy className="h-6 w-6 text-gold" />
                <h2 className="text-3xl font-bold text-foreground">Próximos Campeonatos</h2>
              </div>
              
              {isLoadingCampeonatos && (
                <div className="flex justify-center py-12">
                  <LoadingSpinner size="lg" />
                </div>
              )}

              {errorCampeonatos && (
                <ErrorDisplay
                  title="Erro ao carregar campeonatos"
                  message="Não foi possível carregar os campeonatos."
                  onRetry={refetchCampeonatos}
                />
              )}

              {!isLoadingCampeonatos && !errorCampeonatos && (!campeonatos || campeonatos.length === 0) && (
                <EmptyState
                  icon={Trophy}
                  title="Nenhum campeonato"
                  description="Ainda não há campeonatos ativos."
                />
              )}

              {!isLoadingCampeonatos && !errorCampeonatos && campeonatos && campeonatos.length > 0 && (
                <div className="space-y-6">
                  {campeonatos.slice(0, 3).map((campeonato) => (
                    <ChampionshipCard 
                      key={campeonato.id}
                      title={campeonato.titulo}
                      game={campeonato.jogo?.nome || 'Não especificado'}
                      date={`${new Date(campeonato.dataInicio).toLocaleDateString('pt-BR')}${
                        campeonato.dataFim ? ` - ${new Date(campeonato.dataFim).toLocaleDateString('pt-BR')}` : ''
                      }`}
                      registrationDeadline={campeonato.dataLimiteInscricao 
                        ? new Date(campeonato.dataLimiteInscricao).toLocaleDateString('pt-BR')
                        : undefined
                      }
                      prize={`R$ ${campeonato.premiacao.toLocaleString('pt-BR')}`}
                      teams={campeonato.numeroInscritos ?? 0}
                      status={mapStatus(campeonato.status)}
                    />
                  ))}
                </div>
              )}
            </section>
          </main>

          {/* Right Sidebar - Merchandise */}
          <aside className="lg:col-span-3 space-y-6">
            <MerchandiseAd />
          </aside>
        </div>
      </div>

      {/* Footer */}
      <footer className="border-t border-border bg-card mt-20">
        <div className="container py-12 px-4">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            <div className="space-y-4">
              <h3 className="text-lg font-bold text-foreground">FEMEE</h3>
              <p className="text-sm text-muted-foreground">
                Federação Mineira de Esportes Eletrônicos
              </p>
            </div>
            <div>
              <h4 className="font-semibold text-foreground mb-3">Campeonatos</h4>
              <ul className="space-y-2 text-sm text-muted-foreground">
                <li><a href="#" className="hover:text-primary transition-colors">Valorant</a></li>
                <li><a href="#" className="hover:text-primary transition-colors">League of Legends</a></li>
                <li><a href="#" className="hover:text-primary transition-colors">CS2</a></li>
              </ul>
            </div>
            <div>
              <h4 className="font-semibold text-foreground mb-3">Institucional</h4>
              <ul className="space-y-2 text-sm text-muted-foreground">
                <li><a href="#" className="hover:text-primary transition-colors">Sobre</a></li>
                <li><a href="#" className="hover:text-primary transition-colors">Contato</a></li>
                <li><a href="#" className="hover:text-primary transition-colors">Regulamento</a></li>
              </ul>
            </div>
            <div>
              <h4 className="font-semibold text-foreground mb-3">Redes Sociais</h4>
              <ul className="space-y-2 text-sm text-muted-foreground">
                <li><a href="#" className="hover:text-primary transition-colors">Instagram</a></li>
                <li><a href="#" className="hover:text-primary transition-colors">Twitter</a></li>
                <li><a href="#" className="hover:text-primary transition-colors">Discord</a></li>
              </ul>
            </div>
          </div>
          <div className="mt-8 pt-8 border-t border-border text-center text-sm text-muted-foreground">
            <p>© 2025 FEMEE - Todos os direitos reservados</p>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default Index;
