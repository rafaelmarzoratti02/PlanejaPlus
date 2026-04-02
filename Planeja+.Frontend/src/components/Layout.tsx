import { Link, Outlet, useNavigate } from 'react-router-dom';
import { ToastContainer } from './Toast';
import { useAuth } from '../contexts/AuthContext';

export function Layout() {
  const { email, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate('/login', { replace: true });
  }

  return (
    <div className="min-h-screen bg-slate-50">
      <header className="border-b border-slate-200 bg-white">
        <div className="mx-auto flex h-16 max-w-5xl items-center justify-between px-4">
          <Link to="/" className="text-xl font-bold text-indigo-600">
            Planeja<span className="text-slate-400">+</span>
          </Link>

          <nav className="flex items-center gap-3">
            {email && (
              <span className="hidden text-sm text-slate-500 sm:block">
                {email}
              </span>
            )}
            <Link
              to="/goals/new"
              className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700 transition-colors"
            >
              + New Goal
            </Link>
            <button
              onClick={handleLogout}
              className="rounded-lg border border-slate-300 px-3 py-2 text-sm font-medium text-slate-600 hover:bg-slate-100 hover:text-slate-800 transition-colors"
              aria-label="Sign out"
            >
              Sign out
            </button>
          </nav>
        </div>
      </header>

      <main className="mx-auto max-w-5xl px-4 py-8">
        <Outlet />
      </main>

      <ToastContainer />
    </div>
  );
}
