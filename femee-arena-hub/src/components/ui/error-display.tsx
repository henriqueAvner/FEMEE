import { AlertCircle, RefreshCw } from "lucide-react";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";

interface ErrorDisplayProps {
  title?: string;
  message?: string;
  onRetry?: () => void;
  className?: string;
  compact?: boolean;
}

export function ErrorDisplay({ 
  title = "Erro ao carregar dados",
  message = "Ocorreu um erro inesperado. Tente novamente.",
  onRetry,
  className,
  compact = false
}: ErrorDisplayProps) {
  return (
    <div className={cn(
      "flex flex-col items-center justify-center gap-4 rounded-lg border border-destructive/20 bg-destructive/5 text-center",
      compact ? "p-4" : "p-8",
      className
    )}>
      <AlertCircle className={cn("text-destructive", compact ? "h-8 w-8" : "h-12 w-12")} />
      <div className="space-y-1">
        <h3 className={cn("font-semibold text-destructive", compact && "text-sm")}>{title}</h3>
        <p className={cn("text-muted-foreground", compact ? "text-xs" : "text-sm")}>{message}</p>
      </div>
      {onRetry && (
        <Button variant="outline" onClick={onRetry} size={compact ? "sm" : "default"} className="gap-2">
          <RefreshCw className="h-4 w-4" />
          Tentar novamente
        </Button>
      )}
    </div>
  );
}

interface ErrorPageProps {
  title?: string;
  message?: string;
  onRetry?: () => void;
}

export function ErrorPage({ title, message, onRetry }: ErrorPageProps) {
  return (
    <div className="flex min-h-[400px] items-center justify-center p-4">
      <ErrorDisplay title={title} message={message} onRetry={onRetry} />
    </div>
  );
}
