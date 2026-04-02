import axios from 'axios';
import type {
  FinancialGoalResponse,
  CreateFinancialGoalRequest,
  UpdateFinancialGoalRequest,
  AddTransactionRequest,
  TransactionResponse,
  RegisterRequest,
  LoginRequest,
  AuthResponse,
} from '../types/goal';

export const TOKEN_KEY = 'planeja_token';

export const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
});

// Attach the JWT on every request if present
api.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_KEY);
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Redirect to /login on 401 so stale tokens are handled automatically
api.interceptors.response.use(
  (res) => res,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem(TOKEN_KEY);
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authApi = {
  register: (data: RegisterRequest) =>
    api.post<AuthResponse>('/auth/register', data).then((r) => r.data),

  login: (data: LoginRequest) =>
    api.post<AuthResponse>('/auth/login', data).then((r) => r.data),
};

export const goalsApi = {
  getAll: () =>
    api.get<FinancialGoalResponse[]>('/financialgoals').then((r) => r.data),

  getById: (id: string) =>
    api.get<FinancialGoalResponse>(`/financialgoals/${id}`).then((r) => r.data),

  create: (data: CreateFinancialGoalRequest) =>
    api.post<FinancialGoalResponse>('/financialgoals', data).then((r) => r.data),

  update: (id: string, data: UpdateFinancialGoalRequest) =>
    api.put<FinancialGoalResponse>(`/financialgoals/${id}`, data).then((r) => r.data),

  delete: (id: string) =>
    api.delete(`/financialgoals/${id}`),

  complete: (id: string) =>
    api.post<FinancialGoalResponse>(`/financialgoals/${id}/complete`).then((r) => r.data),

  cancel: (id: string) =>
    api.post<FinancialGoalResponse>(`/financialgoals/${id}/cancel`).then((r) => r.data),

  addTransaction: (goalId: string, data: AddTransactionRequest) =>
    api.post<TransactionResponse>(`/financialgoals/${goalId}/transactions`, data).then((r) => r.data),

  removeTransaction: (goalId: string, transactionId: string) =>
    api.delete(`/financialgoals/${goalId}/transactions/${transactionId}`),
};
