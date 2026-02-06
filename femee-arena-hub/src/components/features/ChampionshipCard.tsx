import { Calendar, Users, Award } from "lucide-react";
import { Card } from "../ui/card";
import { Button } from "../ui/button";

interface ChampionshipCardProps {
  title: string;
  game: string;
  date: string;
  registrationDeadline: string;
  prize: string;
  teams: number;
  status: "upcoming" | "registration-open" | "ongoing";
}

const ChampionshipCard = ({ 
  title, 
  game, 
  date, 
  registrationDeadline, 
  prize, 
  teams,
  status 
}: ChampionshipCardProps) => {
  const statusConfig = {
    "upcoming": { label: "Em Breve", color: "text-muted-foreground bg-muted" },
    "registration-open": { label: "Inscrições Abertas", color: "text-primary bg-primary/10 esports-glow" },
    "ongoing": { label: "Em Andamento", color: "text-accent bg-accent/10" },
  };

  return (
    <Card className="bg-card border-border p-5 space-y-4 card-hover">
      <div className="space-y-2">
        <div className="flex items-start justify-between gap-3">
          <div className="flex-1">
            <h3 className="text-xl font-bold text-foreground mb-1">
              {title}
            </h3>
            <p className="text-sm font-semibold text-primary uppercase tracking-wide">
              {game}
            </p>
          </div>
          <span className={`px-3 py-1 rounded-full text-xs font-semibold whitespace-nowrap ${statusConfig[status].color}`}>
            {statusConfig[status].label}
          </span>
        </div>
      </div>

      <div className="grid grid-cols-2 gap-4 py-3 border-y border-border">
        <div className="flex items-center gap-2">
          <Calendar className="h-4 w-4 text-primary" />
          <div>
            <p className="text-xs text-muted-foreground">Data do Evento</p>
            <p className="text-sm font-semibold text-foreground">{date}</p>
          </div>
        </div>
        <div className="flex items-center gap-2">
          <Users className="h-4 w-4 text-primary" />
          <div>
            <p className="text-xs text-muted-foreground">Times</p>
            <p className="text-sm font-semibold text-foreground">{teams} inscritos</p>
          </div>
        </div>
      </div>

      <div className="space-y-3">
        <div className="flex items-center gap-2">
          <Award className="h-5 w-5 text-gold" />
          <div>
            <p className="text-xs text-muted-foreground">Premiação Total</p>
            <p className="text-lg font-bold gold-text">{prize}</p>
          </div>
        </div>

        {status === "registration-open" && (
          <div className="bg-secondary/50 p-3 rounded-lg">
            <p className="text-xs text-muted-foreground mb-1">Inscrições até</p>
            <p className="text-sm font-semibold text-foreground">{registrationDeadline}</p>
          </div>
        )}

        <Button 
          className="w-full esports-glow" 
          variant={status === "registration-open" ? "default" : "secondary"}
        >
          {status === "registration-open" ? "Inscrever Agora" : "Ver Detalhes"}
        </Button>
      </div>
    </Card>
  );
};

export default ChampionshipCard;
