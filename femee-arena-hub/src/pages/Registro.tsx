import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Trophy, Eye, EyeOff, Loader2, CheckCircle2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { useRegister } from "@/hooks/api";
import { toast } from "@/components/ui/use-toast";

const registerSchema = z.object({
  nome: z.string().min(3, "O nome deve ter pelo menos 3 caracteres"),
  email: z.string().email("Email inválido"),
  telefone: z.string().optional(),
  senha: z
    .string()
    .min(8, "A senha deve ter pelo menos 8 caracteres")
    .regex(/[A-Z]/, "A senha deve ter pelo menos uma letra maiúscula")
    .regex(/[a-z]/, "A senha deve ter pelo menos uma letra minúscula")
    .regex(/[0-9]/, "A senha deve ter pelo menos um número"),
  confirmacaoSenha: z.string(),
}).refine((data) => data.senha === data.confirmacaoSenha, {
  message: "As senhas não coincidem",
  path: ["confirmacaoSenha"],
});

type RegisterFormData = z.infer<typeof registerSchema>;

const Registro = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const registerMutation = useRegister();
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  const senha = watch("senha", "");

  const passwordRequirements = [
    { label: "Pelo menos 8 caracteres", met: senha.length >= 8 },
    { label: "Uma letra maiúscula", met: /[A-Z]/.test(senha) },
    { label: "Uma letra minúscula", met: /[a-z]/.test(senha) },
    { label: "Um número", met: /[0-9]/.test(senha) },
  ];

  const onSubmit = async (data: RegisterFormData) => {
    try {
      await registerMutation.mutateAsync({
        nome: data.nome,
        email: data.email,
        senha: data.senha,
        confirmacaoSenha: data.confirmacaoSenha,
        telefone: data.telefone,
      });
      toast({
        title: "Conta criada com sucesso!",
        description: "Faça login para acessar a plataforma.",
      });
      navigate("/login");
    } catch (error) {
      const apiError = error as { response?: { data?: { message?: string } } };
      toast({
        title: "Erro ao criar conta",
        description: apiError.response?.data?.message || "Não foi possível criar sua conta. Tente novamente.",
        variant: "destructive",
      });
    }
  };

  return (
    <div className="min-h-screen bg-background flex items-center justify-center p-4">
      <div className="w-full max-w-md space-y-8">
        {/* Logo */}
        <div className="flex flex-col items-center text-center">
          <Link to="/" className="flex items-center gap-2 mb-4">
            <Trophy className="h-12 w-12 text-primary" />
            <div className="flex flex-col items-start">
              <h1 className="text-3xl font-bold text-primary">FEMEE</h1>
              <p className="text-xs text-muted-foreground">Federação Mineira de Esportes Eletrônicos</p>
            </div>
          </Link>
        </div>

        {/* Register Card */}
        <Card className="border-border">
          <CardHeader className="space-y-1">
            <CardTitle className="text-2xl text-center">Criar Conta</CardTitle>
            <CardDescription className="text-center">
              Crie sua conta para participar dos campeonatos
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="nome">Nome Completo</Label>
                <Input
                  id="nome"
                  type="text"
                  placeholder="Seu nome completo"
                  {...register("nome")}
                  disabled={registerMutation.isPending}
                />
                {errors.nome && (
                  <p className="text-sm text-destructive">{errors.nome.message}</p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <Input
                  id="email"
                  type="email"
                  placeholder="seu@email.com"
                  {...register("email")}
                  disabled={registerMutation.isPending}
                />
                {errors.email && (
                  <p className="text-sm text-destructive">{errors.email.message}</p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="telefone">Telefone (opcional)</Label>
                <Input
                  id="telefone"
                  type="tel"
                  placeholder="(31) 99999-9999"
                  {...register("telefone")}
                  disabled={registerMutation.isPending}
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="senha">Senha</Label>
                <div className="relative">
                  <Input
                    id="senha"
                    type={showPassword ? "text" : "password"}
                    placeholder="••••••••"
                    {...register("senha")}
                    disabled={registerMutation.isPending}
                  />
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    className="absolute right-0 top-0 h-full px-3 hover:bg-transparent"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? (
                      <EyeOff className="h-4 w-4 text-muted-foreground" />
                    ) : (
                      <Eye className="h-4 w-4 text-muted-foreground" />
                    )}
                  </Button>
                </div>
                {errors.senha && (
                  <p className="text-sm text-destructive">{errors.senha.message}</p>
                )}
                
                {/* Password Requirements */}
                {senha && (
                  <div className="mt-2 space-y-1">
                    {passwordRequirements.map((req, index) => (
                      <div key={index} className="flex items-center gap-2 text-xs">
                        <CheckCircle2
                          className={`h-3 w-3 ${
                            req.met ? "text-primary" : "text-muted-foreground"
                          }`}
                        />
                        <span className={req.met ? "text-foreground" : "text-muted-foreground"}>
                          {req.label}
                        </span>
                      </div>
                    ))}
                  </div>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="confirmacaoSenha">Confirmar Senha</Label>
                <div className="relative">
                  <Input
                    id="confirmacaoSenha"
                    type={showConfirmPassword ? "text" : "password"}
                    placeholder="••••••••"
                    {...register("confirmacaoSenha")}
                    disabled={registerMutation.isPending}
                  />
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    className="absolute right-0 top-0 h-full px-3 hover:bg-transparent"
                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                  >
                    {showConfirmPassword ? (
                      <EyeOff className="h-4 w-4 text-muted-foreground" />
                    ) : (
                      <Eye className="h-4 w-4 text-muted-foreground" />
                    )}
                  </Button>
                </div>
                {errors.confirmacaoSenha && (
                  <p className="text-sm text-destructive">{errors.confirmacaoSenha.message}</p>
                )}
              </div>

              <Button type="submit" className="w-full esports-glow" disabled={registerMutation.isPending}>
                {registerMutation.isPending ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    Criando conta...
                  </>
                ) : (
                  "Criar Conta"
                )}
              </Button>
            </form>
          </CardContent>
          <CardFooter className="flex flex-col space-y-4">
            <p className="text-xs text-muted-foreground text-center">
              Ao criar uma conta, você concorda com os{" "}
              <Link to="/termos" className="text-primary hover:underline">
                Termos de Uso
              </Link>{" "}
              e{" "}
              <Link to="/privacidade" className="text-primary hover:underline">
                Política de Privacidade
              </Link>
              .
            </p>
            <div className="relative w-full">
              <div className="absolute inset-0 flex items-center">
                <span className="w-full border-t border-border" />
              </div>
              <div className="relative flex justify-center text-xs uppercase">
                <span className="bg-card px-2 text-muted-foreground">ou</span>
              </div>
            </div>
            <p className="text-sm text-muted-foreground text-center">
              Já tem uma conta?{" "}
              <Link to="/login" className="text-primary font-medium hover:underline">
                Fazer login
              </Link>
            </p>
          </CardFooter>
        </Card>

        {/* Back to Home */}
        <p className="text-center text-sm text-muted-foreground">
          <Link to="/" className="hover:text-primary transition-colors">
            ← Voltar para a página inicial
          </Link>
        </p>
      </div>
    </div>
  );
};

export default Registro;
