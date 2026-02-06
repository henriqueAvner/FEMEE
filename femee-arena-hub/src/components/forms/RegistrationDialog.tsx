import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "../ui/dialog";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { useState } from "react";
import { toast } from "../ui/use-toast";
import { useCreateInscricao } from "@/hooks/api";
import { useAuth } from "@/contexts/AuthContext";
import { LoadingSpinner } from "../ui/loading";

interface RegistrationDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  campeonatoId?: number | null;
}

const RegistrationDialog = ({ open, onOpenChange, campeonatoId }: RegistrationDialogProps) => {
  const { isAuthenticated, user } = useAuth();
  const createInscricao = useCreateInscricao();
  
  const [formData, setFormData] = useState({
    timeId: "",
    observacoes: "",
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!campeonatoId) {
      toast({
        title: "Erro",
        description: "Nenhum campeonato selecionado.",
        variant: "destructive",
      });
      return;
    }

    if (!isAuthenticated) {
      toast({
        title: "Autenticação necessária",
        description: "Você precisa estar logado para inscrever um time.",
        variant: "destructive",
      });
      return;
    }

    const timeId = parseInt(formData.timeId, 10);
    if (isNaN(timeId)) {
      toast({
        title: "Erro",
        description: "Selecione um time válido.",
        variant: "destructive",
      });
      return;
    }

    try {
      await createInscricao.mutateAsync({
        campeonatoId,
        timeId,
        observacoes: formData.observacoes || undefined,
      });
      
      toast({
        title: "Inscrição enviada!",
        description: "Sua inscrição foi enviada com sucesso. Aguarde a confirmação.",
      });
      
      onOpenChange(false);
      setFormData({ timeId: "", observacoes: "" });
    } catch (error) {
      const apiError = error as { response?: { data?: { message?: string } } };
      toast({
        title: "Erro ao inscrever",
        description: apiError.response?.data?.message || "Não foi possível completar a inscrição. Tente novamente.",
        variant: "destructive",
      });
    }
  };

  // Formulário simplificado para usuários não autenticados
  const renderGuestForm = () => (
    <div className="space-y-4 mt-4">
      <p className="text-sm text-muted-foreground">
        Para inscrever seu time em campeonatos da FEMEE, você precisa:
      </p>
      <ol className="list-decimal list-inside space-y-2 text-sm text-muted-foreground">
        <li>Criar uma conta ou fazer login</li>
        <li>Cadastrar seu time na plataforma</li>
        <li>Voltar aqui e selecionar seu time para inscrição</li>
      </ol>
      <div className="flex gap-2 pt-4">
        <Button 
          variant="outline" 
          className="flex-1"
          onClick={() => {
            onOpenChange(false);
            // TODO: Navegar para página de login
          }}
        >
          Fazer Login
        </Button>
        <Button 
          className="flex-1"
          onClick={() => {
            onOpenChange(false);
            // TODO: Navegar para página de cadastro
          }}
        >
          Criar Conta
        </Button>
      </div>
    </div>
  );

  // Formulário para usuários autenticados
  const renderAuthenticatedForm = () => (
    <form onSubmit={handleSubmit} className="space-y-4 mt-4">
      <div className="space-y-2">
        <Label htmlFor="timeId">ID do Time</Label>
        <Input
          id="timeId"
          type="number"
          placeholder="Digite o ID do seu time"
          value={formData.timeId}
          onChange={(e) => setFormData({ ...formData, timeId: e.target.value })}
          required
        />
        <p className="text-xs text-muted-foreground">
          Você pode encontrar o ID do seu time na página do time.
        </p>
      </div>
      <div className="space-y-2">
        <Label htmlFor="observacoes">Observações (opcional)</Label>
        <Input
          id="observacoes"
          placeholder="Alguma observação sobre a inscrição?"
          value={formData.observacoes}
          onChange={(e) => setFormData({ ...formData, observacoes: e.target.value })}
        />
      </div>
      <Button 
        type="submit" 
        className="w-full"
        disabled={createInscricao.isPending}
      >
        {createInscricao.isPending ? (
          <>
            <LoadingSpinner size="sm" className="mr-2" />
            Enviando...
          </>
        ) : (
          'Enviar Inscrição'
        )}
      </Button>
    </form>
  );

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle className="text-primary">Inscrever Time</DialogTitle>
          <DialogDescription>
            {isAuthenticated 
              ? "Preencha os dados abaixo para inscrever seu time neste campeonato."
              : "Crie uma conta ou faça login para inscrever seu time nos campeonatos da FEMEE."
            }
          </DialogDescription>
        </DialogHeader>
        {isAuthenticated ? renderAuthenticatedForm() : renderGuestForm()}
      </DialogContent>
    </Dialog>
  );
};

export default RegistrationDialog;
