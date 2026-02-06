import { Calendar, MessageSquare } from "lucide-react";
import { Card } from "../ui/card";

interface NewsCardProps {
  title: string;
  excerpt: string;
  date: string;
  comments: number;
  image?: string;
  category: string;
}

const NewsCard = ({ title, excerpt, date, comments, image, category }: NewsCardProps) => {
  return (
    <Card className="overflow-hidden card-hover cursor-pointer bg-card border-border">
      {image && (
        <div className="aspect-video overflow-hidden">
          <img 
            src={image} 
            alt={title}
            className="w-full h-full object-cover transition-transform duration-300 hover:scale-105"
          />
        </div>
      )}
      <div className="p-4 space-y-3">
        <div className="flex items-center gap-2">
          <span className="text-xs font-semibold text-primary uppercase tracking-wide">
            {category}
          </span>
        </div>
        <h3 className="text-lg font-bold text-foreground leading-tight line-clamp-2 hover:text-primary transition-colors">
          {title}
        </h3>
        <p className="text-sm text-muted-foreground line-clamp-2">
          {excerpt}
        </p>
        <div className="flex items-center justify-between text-xs text-muted-foreground pt-2 border-t border-border">
          <div className="flex items-center gap-1">
            <Calendar className="h-3 w-3" />
            <span>{date}</span>
          </div>
          <div className="flex items-center gap-1">
            <MessageSquare className="h-3 w-3" />
            <span>{comments} comentários</span>
          </div>
        </div>
      </div>
    </Card>
  );
};

export default NewsCard;
