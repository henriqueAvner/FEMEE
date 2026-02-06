import { Trophy, Menu, X, LogIn, UserPlus } from "lucide-react";
import { Button } from "../ui/button";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import RegistrationDialog from "../forms/RegistrationDialog";
import { useAuth } from "@/contexts/AuthContext";
import { useIsMobile } from "@/hooks/use-mobile";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "../ui/dropdown-menu";
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "../ui/sheet";

const Header = () => {
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const { isAuthenticated, user, logout, isAdmin } = useAuth();
  const isMobile = useIsMobile();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  const navLinks = [
    { href: "/", label: "Notícias" },
    { href: "/campeonatos", label: "Campeonatos" },
    { href: "/times", label: "Times" },
    { href: "/ranking", label: "Ranking" },
  ];

  const NavLinks = ({ onClick }: { onClick?: () => void }) => (
    <>
      {navLinks.map((link) => (
        <Link
          key={link.href}
          to={link.href}
          className="text-sm font-medium text-foreground hover:text-primary transition-colors"
          onClick={onClick}
        >
          {link.label}
        </Link>
      ))}
    </>
  );

  const AuthButtons = ({ mobile = false }: { mobile?: boolean }) => {
    if (isAuthenticated && user) {
      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="outline" size={mobile ? "default" : "sm"} className={mobile ? "w-full" : ""}>
              {user.nome.split(' ')[0]}
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuItem className="text-muted-foreground">
              {user.email}
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            {isAdmin && (
              <>
                <DropdownMenuItem asChild>
                  <Link to="/admin">Painel Admin</Link>
                </DropdownMenuItem>
                <DropdownMenuSeparator />
              </>
            )}
            <DropdownMenuItem asChild>
              <Link to="/perfil">Meu Perfil</Link>
            </DropdownMenuItem>
            <DropdownMenuItem onClick={handleLogout} className="text-destructive">
              Sair
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      );
    }

    return (
      <div className={`flex ${mobile ? "flex-col w-full" : ""} gap-2`}>
        <Link to="/login">
          <Button variant="ghost" size={mobile ? "default" : "sm"} className={mobile ? "w-full justify-start" : ""}>
            <LogIn className="h-4 w-4 mr-2" />
            Entrar
          </Button>
        </Link>
        <Link to="/registro">
          <Button size={mobile ? "default" : "sm"} className={`esports-glow ${mobile ? "w-full" : ""}`}>
            <UserPlus className="h-4 w-4 mr-2" />
            Criar Conta
          </Button>
        </Link>
      </div>
    );
  };

  return (
    <header className="sticky top-0 z-50 w-full border-b border-border bg-card/95 backdrop-blur supports-[backdrop-filter]:bg-card/80">
      <div className="container flex h-16 items-center justify-between px-4">
        <Link to="/" className="flex items-center gap-2">
          <Trophy className="h-8 w-8 text-primary" />
          <div className="flex flex-col">
            <h1 className="text-xl font-bold text-primary">FEMEE</h1>
            <p className="text-xs text-muted-foreground hidden sm:block">Federação Mineira de Esportes Eletrônicos</p>
          </div>
        </Link>
        
        {/* Desktop Navigation */}
        <nav className="hidden md:flex items-center gap-6">
          <NavLinks />
          <AuthButtons />
        </nav>

        {/* Mobile Navigation */}
        <div className="md:hidden">
          <Sheet open={isMobileMenuOpen} onOpenChange={setIsMobileMenuOpen}>
            <SheetTrigger asChild>
              <Button variant="ghost" size="icon">
                <Menu className="h-6 w-6" />
                <span className="sr-only">Abrir menu</span>
              </Button>
            </SheetTrigger>
            <SheetContent side="right" className="w-[300px] sm:w-[400px]">
              <SheetHeader>
                <SheetTitle className="flex items-center gap-2">
                  <Trophy className="h-6 w-6 text-primary" />
                  <span className="text-primary">FEMEE</span>
                </SheetTitle>
              </SheetHeader>
              <nav className="flex flex-col gap-4 mt-8">
                <NavLinks onClick={() => setIsMobileMenuOpen(false)} />
                <div className="border-t border-border my-4" />
                <AuthButtons mobile />
              </nav>
            </SheetContent>
          </Sheet>
        </div>
      </div>
      <RegistrationDialog open={isDialogOpen} onOpenChange={setIsDialogOpen} />
    </header>
  );
};

export default Header;
