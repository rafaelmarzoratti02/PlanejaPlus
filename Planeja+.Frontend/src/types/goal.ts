export interface TransactionResponse {
  id: string;
  amount: number;
  type: string;
  date: string;
  description: string | null;
  createdAt: string;
}

export interface FinancialGoalResponse {
  id: string;
  name: string;
  targetAmount: number;
  currentAmount: number;
  status: string;
  deadline: string;
  description: string | null;
  createdAt: string;
  updatedAt: string | null;
  transactions: TransactionResponse[];
}

export interface CreateFinancialGoalRequest {
  name: string;
  targetAmount: number;
  deadline: string;
  description?: string;
}

export interface UpdateFinancialGoalRequest {
  name: string;
  targetAmount: number;
  deadline: string;
  description?: string;
}

export interface AddTransactionRequest {
  amount: number;
  type: number;
  date: string;
  description?: string;
}

export interface ApiError {
  error: string;
  status: number;
}
