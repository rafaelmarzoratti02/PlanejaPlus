import { createContext, useContext, useState, useCallback, type ReactNode } from 'react';
import { TOKEN_KEY } from '../api/client';

interface AuthState {
  token: string | null;
  email: string | null;
}

interface AuthContextValue extends AuthState {
  isAuthenticated: boolean;
  login: (token: string, email: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

function parseEmailFromToken(token: string): string | null {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.email ?? null;
  } catch {
    return null;
  }
}

function loadInitialState(): AuthState {
  const token = localStorage.getItem(TOKEN_KEY);
  if (!token) return { token: null, email: null };
  return { token, email: parseEmailFromToken(token) };
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<AuthState>(loadInitialState);

  const login = useCallback((token: string, email: string) => {
    localStorage.setItem(TOKEN_KEY, token);
    setState({ token, email });
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem(TOKEN_KEY);
    setState({ token: null, email: null });
  }, []);

  return (
    <AuthContext.Provider
      value={{ ...state, isAuthenticated: state.token !== null, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider');
  return ctx;
}
