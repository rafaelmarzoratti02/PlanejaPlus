import { Link, Outlet } from 'react-router-dom';
import { ToastContainer } from './Toast';

export function Layout() {
  return (
    <div className="min-h-screen bg-slate-50">
      <header className="border-b border-slate-200 bg-white">
        <div className="mx-auto flex h-16 max-w-5xl items-center justify-between px-4">
          <Link to="/" className="text-xl font-bold text-indigo-600">
            Planeja<span className="text-slate-400">+</span>
          </Link>
          <nav>
            <Link
              to="/goals/new"
              className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700 transition-colors"
            >
              + New Goal
            </Link>
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
