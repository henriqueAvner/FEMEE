// ============================================
// ENUMS - Alinhados com FEMEE.Domain.Enums
// ============================================

export enum TipoUsuario {
  Administrador = 1,
  Capitao = 2,
  Jogador = 3,
  Visitante = 4,
  Moderador = 5,
}

export enum StatusCampeonato {
  Planejado = 0,        // UpComing
  InscricoesAbertas = 1, // Open
  EmAndamento = 2,       // InProgress
  Finalizado = 3,        // Closed
  Cancelado = 4,         // Cancelled
}

export enum StatusInscricao {
  Pendente = 1,
  Aprovada = 2,
  Rejeitada = 3,
}

// ============================================
// AUTH DTOs
// ============================================

export interface LoginRequest {
  email: string;
  senha: string;
}

export interface RegisterRequest {
  nome: string;
  email: string;
  senha: string;
  confirmacaoSenha: string;
  telefone?: string;
  tipoUsuario?: TipoUsuario;
}

export interface LoginResponse {
  token: string;
  userId: number;
  email: string;
  nome: string;
  tipoUsuario: TipoUsuario;
  expiresAt: string;
}

export interface AuthUser {
  userId: number;
  email: string;
  nome: string;
  tipoUsuario: TipoUsuario;
}

// ============================================
// USER DTOs
// ============================================

export interface UserResponse {
  id: number;
  nome: string;
  email: string;
  telefone?: string;
  tipoUsuario: TipoUsuario;
  dataCriacao: string;
}

// ============================================
// TIME DTOs
// ============================================

export interface TimeResponse {
  id: number;
  nome: string;
  slug: string;
  logoUrl?: string;
  descricao?: string;
  vitorias: number;
  derrotas: number;
  empates: number;
  pontos: number;
  posicaoRanking: number;
  dataCriacao: string;
  // Campos opcionais para detalhes expandidos
  jogadores?: JogadorResponse[];
  conquistas?: ConquistaResponse[];
  proximasPartidas?: PartidaResponse[];
}

export interface ConquistaResponse {
  id: number;
  titulo: string;
  campeonato: string;
  posicao: number;
  data?: string;
}

export interface PartidaResponse {
  id: number;
  adversario: string;
  dataHora: string;
  campeonatoNome?: string;
}

export interface CreateTimeRequest {
  nome: string;
  descricao?: string;
  logoUrl?: string;
}

export interface UpdateTimeRequest {
  nome?: string;
  descricao?: string;
  logoUrl?: string;
}

// ============================================
// JOGADOR DTOs
// ============================================

export interface JogadorResponse {
  id: number;
  nome: string;
  nickname: string;
  posicao?: string;
  funcao?: string;
  timeId: number;
  timeNome?: string;
  userId?: number;
  dataNascimento?: string;
  dataCriacao: string;
}

// ============================================
// CAMPEONATO DTOs
// ============================================

export interface JogoResponse {
  id: number;
  nome: string;
  slug: string;
  iconeUrl?: string;
  ativo: boolean;
}

export interface CampeonatoResponse {
  id: number;
  titulo: string;
  descricao?: string;
  dataInicio: string;
  dataFim: string;
  local?: string;
  cidade?: string;
  estado?: string;
  premiacao: number;
  numeroVagas: number;
  numeroInscritos: number;
  status: StatusCampeonato;
  jogo?: JogoResponse;
  // Aliases para compatibilidade com frontend
  nome?: string; // alias para titulo
  dataLimiteInscricao?: string;
  maximoTimes?: number; // alias para numeroVagas
  quantidadeTimesInscritos?: number; // alias para numeroInscritos
}

export interface CreateCampeonatoRequest {
  titulo: string;
  descricao?: string;
  dataInicio: string;
  dataFim: string;
  local?: string;
  cidade?: string;
  estado?: string;
  premiacao?: number;
  numeroVagas: number;
  jogoId: number;
}

// ============================================
// NOTICIA DTOs
// ============================================

export interface NoticiaResponse {
  id: number;
  titulo: string;
  slug: string;
  resumo: string;
  conteudo: string;
  categoria: string;
  imagemUrl: string;
  imagemCapa?: string; // Alias para imagemUrl
  dataPublicacao: string;
  visualizacoes: number;
  autor: UserResponse;
}

// ============================================
// PARTIDA DTOs
// ============================================

export interface PartidaResponse {
  id: number;
  campeonatoId: number;
  timeAId: number;
  timeBId: number;
  timeANome?: string;
  timeBNome?: string;
  dataHora: string;
  placarA?: number;
  placarB?: number;
  timeVencedorId?: number;
  status: string;
}

export interface FinishPartidaRequest {
  timeVencedorId: number;
  placarA: number;
  placarB: number;
}

// ============================================
// INSCRICAO CAMPEONATO DTOs
// ============================================

export interface InscricaoCampeonatoResponse {
  id: number;
  campeonatoId: number;
  campeonatoTitulo?: string;
  timeId: number;
  timeNome?: string;
  status: StatusInscricao;
  dataInscricao: string;
  dataAprovacao?: string;
}

export interface CreateInscricaoCampeonatoRequest {
  campeonatoId: number;
  timeId: number;
  observacoes?: string;
}

// ============================================
// PAGINATION
// ============================================

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface PaginationParams {
  page?: number;
  pageSize?: number;
}

// ============================================
// API ERROR
// ============================================

export interface ApiError {
  statusCode: number;
  message: string;
  details?: string;
  timestamp: string;
  traceId?: string;
}
