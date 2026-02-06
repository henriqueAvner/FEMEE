import { Inbox, LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";
import { Link } from "react-router-dom";
import { Button } from "./button";

interface EmptyStateAction {
  label: string;
  href?: string;
  onClick?: () => void;
}

interface EmptyStateProps {
  icon?: LucideIcon | React.ReactNode;
  title?: string;
  description?: string;
  action?: EmptyStateAction | React.ReactNode;
  className?: string;
}

export function EmptyState({
  icon: IconProp,
  title = "Nenhum item encontrado",
  description = "Não há dados para exibir no momento.",
  action,
  className,
}: EmptyStateProps) {
  const renderIcon = () => {
    if (!IconProp) {
      return <Inbox className="h-12 w-12 text-muted-foreground" />;
    }
    
    // Se for um componente Lucide (função)
    if (typeof IconProp === 'function') {
      const LucideIconComponent = IconProp as LucideIcon;
      return <LucideIconComponent className="h-12 w-12 text-muted-foreground" />;
    }
    
    // Se for um ReactNode
    return IconProp;
  };

  const renderAction = (): React.ReactNode => {
    if (!action) return null;
    
    // Se for um objeto EmptyStateAction
    if (typeof action === 'object' && action !== null && 'label' in action) {
      const actionConfig = action as EmptyStateAction;
      if (actionConfig.href) {
        return (
          <Link to={actionConfig.href}>
            <Button variant="outline">{actionConfig.label}</Button>
          </Link>
        );
      }
      if (actionConfig.onClick) {
        return (
          <Button variant="outline" onClick={actionConfig.onClick}>
            {actionConfig.label}
          </Button>
        );
      }
      return null;
    }
    
    // Se for um ReactNode
    return action as React.ReactNode;
  };

  return (
    <div className={cn(
      "flex flex-col items-center justify-center gap-4 rounded-lg border border-dashed p-8 text-center",
      className
    )}>
      {renderIcon()}
      <div className="space-y-1">
        <h3 className="font-semibold">{title}</h3>
        <p className="text-sm text-muted-foreground">{description}</p>
      </div>
      {renderAction()}
    </div>
  );
}
