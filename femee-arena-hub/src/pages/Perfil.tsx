import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { User, Mail, Phone, Shield, Trophy, Users, Calendar, Camera, Loader2 } from "lucide-react";
import Header from "@/components/layout/Header";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Separator } from "@/components/ui/separator";
import { useToast } from "@/hooks/use-toast";
import { useAuth } from "@/contexts/AuthContext";
import { TipoUsuario } from "@/types/api.types";

const perfilSchema = z.object({
  nome: z.string().min(3, "Nome deve ter no mínimo 3 caracteres"),
  email: z.string().email("E-mail inválido"),
  telefone: z.string().optional(),
});

const senhaSchema = z.object({
  senhaAtual: z.string().min(1, "Senha atual é obrigatória"),
  novaSenha: z
    .string()
    .min(8, "Nova senha deve ter no mínimo 8 caracteres")
    .regex(/[A-Z]/, "Deve conter pelo menos uma letra maiúscula")
    .regex(/[a-z]/, "Deve conter pelo menos uma letra minúscula")
    .regex(/[0-9]/, "Deve conter pelo menos um número")
    .regex(/[^A-Za-z0-9]/, "Deve conter pelo menos um caractere especial"),
  confirmarSenha: z.string(),
}).refine((data) => data.novaSenha === data.confirmarSenha, {
  message: "As senhas não coincidem",
  path: ["confirmarSenha"],
});

type PerfilFormData = z.infer<typeof perfilSchema>;
type SenhaFormData = z.infer<typeof senhaSchema>;

const Perfil = () => {
  const navigate = useNavigate();
  const { toast } = useToast();
  const { user, isAuthenticated } = useAuth();
  const [isEditingProfile, setIsEditingProfile] = useState(false);
  const [isSavingProfile, setIsSavingProfile] = useState(false);
  const [isSavingPassword, setIsSavingPassword] = useState(false);

  const perfilForm = useForm<PerfilFormData>({
    resolver: zodResolver(perfilSchema),
    defaultValues: {
      nome: user?.nome || "",
      email: user?.email || "",
      telefone: "",
    },
  });

  const senhaForm = useForm<SenhaFormData>({
    resolver: zodResolver(senhaSchema),
    defaultValues: {
      senhaAtual: "",
      novaSenha: "",
      confirmarSenha: "",
    },
  });

  const handleSavePerfil = async (data: PerfilFormData) => {
    setIsSavingProfile(true);
    try {
      // TODO: Integrar com API de atualização de perfil
      await new Promise((resolve) => setTimeout(resolve, 1000));
      
      toast({
        title: "Perfil atualizado",
        description: "Suas informações foram atualizadas com sucesso.",
      });
      setIsEditingProfile(false);
    } catch (error) {
      toast({
        title: "Erro ao atualizar",
        description: "Não foi possível atualizar seu perfil. Tente novamente.",
        variant: "destructive",
      });
    } finally {
      setIsSavingProfile(false);
    }
  };

  const handleChangePassword = async (data: SenhaFormData) => {
    setIsSavingPassword(true);
    try {
      // TODO: Integrar com API de alteração de senha
      await new Promise((resolve) => setTimeout(resolve, 1000));
      
      toast({
        title: "Senha alterada",
        description: "Sua senha foi alterada com sucesso.",
      });
      senhaForm.reset();
    } catch (error) {
      toast({
        title: "Erro ao alterar senha",
        description: "Não foi possível alterar sua senha. Verifique a senha atual.",
        variant: "destructive",
      });
    } finally {
      setIsSavingPassword(false);
    }
  };

  // Mock data for user stats
  const userStats = {
    campeonatosParticipados: 12,
    timesGerenciados: 2,
    membroDesde: "Janeiro 2024",
  };

  const getRoleBadge = (tipoUsuario?: TipoUsuario) => {
    switch (tipoUsuario) {
      case TipoUsuario.Administrador:
        return <Badge variant="destructive">Administrador</Badge>;
      case TipoUsuario.Capitao:
        return <Badge variant="default">Capitão</Badge>;
      case TipoUsuario.Moderador:
        return <Badge className="bg-blue-500 hover:bg-blue-600">Moderador</Badge>;
      default:
        return <Badge variant="secondary">Membro</Badge>;
    }
  };

  const getInitials = (name?: string) => {
    if (!name) return "U";
    return name
      .split(" ")
      .map((n) => n[0])
      .join("")
      .toUpperCase()
      .slice(0, 2);
  };

  if (!isAuthenticated) {
    navigate("/login");
    return null;
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-femee-darker via-background to-femee-darker">
      <Header />

      <main className="container mx-auto px-4 py-8">
        <div className="max-w-4xl mx-auto">
          {/* Profile Header */}
          <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20 mb-6">
            <CardContent className="pt-6">
              <div className="flex flex-col md:flex-row items-center md:items-start gap-6">
                <div className="relative">
                  <Avatar className="w-24 h-24 border-4 border-femee-purple">
                    <AvatarImage src={undefined} />
                    <AvatarFallback className="text-2xl bg-femee-purple/20">
                      {getInitials(user?.nome)}
                    </AvatarFallback>
                  </Avatar>
                  <Button
                    size="icon"
                    variant="secondary"
                    className="absolute -bottom-2 -right-2 rounded-full w-8 h-8"
                    title="Alterar foto"
                  >
                    <Camera className="h-4 w-4" />
                  </Button>
                </div>
                <div className="flex-1 text-center md:text-left">
                  <div className="flex flex-col md:flex-row md:items-center gap-2 mb-2">
                    <h1 className="text-2xl font-bold">{user?.nome || "Usuário"}</h1>
                    {getRoleBadge(user?.tipoUsuario)}
                  </div>
                  <p className="text-muted-foreground">{user?.email}</p>
                  <div className="flex flex-wrap justify-center md:justify-start gap-4 mt-4">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <Trophy className="h-4 w-4 text-femee-orange" />
                      <span>{userStats.campeonatosParticipados} campeonatos</span>
                    </div>
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <Users className="h-4 w-4 text-femee-purple" />
                      <span>{userStats.timesGerenciados} times</span>
                    </div>
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <Calendar className="h-4 w-4" />
                      <span>Desde {userStats.membroDesde}</span>
                    </div>
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>

          {/* Tabs */}
          <Tabs defaultValue="perfil" className="space-y-6">
            <TabsList className="grid w-full grid-cols-2 max-w-md mx-auto">
              <TabsTrigger value="perfil">Informações</TabsTrigger>
              <TabsTrigger value="seguranca">Segurança</TabsTrigger>
            </TabsList>

            {/* Profile Tab */}
            <TabsContent value="perfil">
              <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div>
                      <CardTitle className="flex items-center gap-2">
                        <User className="h-5 w-5" />
                        Informações Pessoais
                      </CardTitle>
                      <CardDescription>
                        Gerencie suas informações de perfil
                      </CardDescription>
                    </div>
                    {!isEditingProfile && (
                      <Button
                        variant="outline"
                        onClick={() => setIsEditingProfile(true)}
                      >
                        Editar
                      </Button>
                    )}
                  </div>
                </CardHeader>
                <CardContent>
                  <form onSubmit={perfilForm.handleSubmit(handleSavePerfil)}>
                    <div className="space-y-4">
                      <div className="space-y-2">
                        <Label htmlFor="nome">Nome completo</Label>
                        <div className="relative">
                          <User className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                          <Input
                            id="nome"
                            className="pl-10"
                            disabled={!isEditingProfile}
                            {...perfilForm.register("nome")}
                          />
                        </div>
                        {perfilForm.formState.errors.nome && (
                          <p className="text-sm text-destructive">
                            {perfilForm.formState.errors.nome.message}
                          </p>
                        )}
                      </div>

                      <div className="space-y-2">
                        <Label htmlFor="email">E-mail</Label>
                        <div className="relative">
                          <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                          <Input
                            id="email"
                            type="email"
                            className="pl-10"
                            disabled={!isEditingProfile}
                            {...perfilForm.register("email")}
                          />
                        </div>
                        {perfilForm.formState.errors.email && (
                          <p className="text-sm text-destructive">
                            {perfilForm.formState.errors.email.message}
                          </p>
                        )}
                      </div>

                      <div className="space-y-2">
                        <Label htmlFor="telefone">Telefone (opcional)</Label>
                        <div className="relative">
                          <Phone className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                          <Input
                            id="telefone"
                            className="pl-10"
                            placeholder="(00) 00000-0000"
                            disabled={!isEditingProfile}
                            {...perfilForm.register("telefone")}
                          />
                        </div>
                      </div>

                      {isEditingProfile && (
                        <div className="flex gap-2 pt-4">
                          <Button
                            type="submit"
                            className="bg-femee-purple hover:bg-femee-purple/90"
                            disabled={isSavingProfile}
                          >
                            {isSavingProfile ? (
                              <>
                                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                Salvando...
                              </>
                            ) : (
                              "Salvar alterações"
                            )}
                          </Button>
                          <Button
                            type="button"
                            variant="outline"
                            onClick={() => {
                              setIsEditingProfile(false);
                              perfilForm.reset();
                            }}
                          >
                            Cancelar
                          </Button>
                        </div>
                      )}
                    </div>
                  </form>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Security Tab */}
            <TabsContent value="seguranca">
              <Card className="bg-card/50 backdrop-blur-sm border-femee-purple/20">
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <Shield className="h-5 w-5" />
                    Alterar Senha
                  </CardTitle>
                  <CardDescription>
                    Mantenha sua conta segura com uma senha forte
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <form onSubmit={senhaForm.handleSubmit(handleChangePassword)}>
                    <div className="space-y-4">
                      <div className="space-y-2">
                        <Label htmlFor="senhaAtual">Senha atual</Label>
                        <Input
                          id="senhaAtual"
                          type="password"
                          {...senhaForm.register("senhaAtual")}
                        />
                        {senhaForm.formState.errors.senhaAtual && (
                          <p className="text-sm text-destructive">
                            {senhaForm.formState.errors.senhaAtual.message}
                          </p>
                        )}
                      </div>

                      <Separator />

                      <div className="space-y-2">
                        <Label htmlFor="novaSenha">Nova senha</Label>
                        <Input
                          id="novaSenha"
                          type="password"
                          {...senhaForm.register("novaSenha")}
                        />
                        {senhaForm.formState.errors.novaSenha && (
                          <p className="text-sm text-destructive">
                            {senhaForm.formState.errors.novaSenha.message}
                          </p>
                        )}
                      </div>

                      <div className="space-y-2">
                        <Label htmlFor="confirmarSenha">Confirmar nova senha</Label>
                        <Input
                          id="confirmarSenha"
                          type="password"
                          {...senhaForm.register("confirmarSenha")}
                        />
                        {senhaForm.formState.errors.confirmarSenha && (
                          <p className="text-sm text-destructive">
                            {senhaForm.formState.errors.confirmarSenha.message}
                          </p>
                        )}
                      </div>

                      <div className="bg-muted/50 p-4 rounded-lg">
                        <p className="text-sm text-muted-foreground mb-2">
                          A senha deve conter:
                        </p>
                        <ul className="text-sm text-muted-foreground space-y-1">
                          <li>• Mínimo 8 caracteres</li>
                          <li>• Uma letra maiúscula</li>
                          <li>• Uma letra minúscula</li>
                          <li>• Um número</li>
                          <li>• Um caractere especial</li>
                        </ul>
                      </div>

                      <Button
                        type="submit"
                        className="bg-femee-purple hover:bg-femee-purple/90"
                        disabled={isSavingPassword}
                      >
                        {isSavingPassword ? (
                          <>
                            <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                            Alterando...
                          </>
                        ) : (
                          "Alterar senha"
                        )}
                      </Button>
                    </div>
                  </form>
                </CardContent>
              </Card>
            </TabsContent>
          </Tabs>
        </div>
      </main>
    </div>
  );
};

export default Perfil;
