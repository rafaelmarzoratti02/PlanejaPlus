import axios from 'axios';
import type {
  FinancialGoalResponse,
  CreateFinancialGoalRequest,
  UpdateFinancialGoalRequest,
  AddTransactionRequest,
  TransactionResponse,
} from '../types/goal';

const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
});

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
