import { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { goalsApi } from '../api/client';
import { Skeleton } from '../components/Skeleton';
import { ErrorState } from '../components/ErrorState';
import { useToast } from '../components/Toast';
import type { FinancialGoalResponse } from '../types/goal';

function formatCurrency(value: number) {
  return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value);
}

function formatDate(iso: string) {
  return new Date(iso).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
}

export function GoalDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const toast = useToast();
  const [goal, setGoal] = useState<FinancialGoalResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [actionLoading, setActionLoading] = useState(false);

  const fetchGoal = () => {
    if (!id) return;
    setLoading(true);
    setError(null);
    goalsApi.getById(id)
      .then(setGoal)
      .catch((err) => setError(err.response?.data?.error ?? 'Failed to load goal'))
      .finally(() => setLoading(false));
  };

  useEffect(() => { fetchGoal(); }, [id]);

  const handleDelete = async () => {
    if (!id || !confirm('Are you sure you want to delete this goal?')) return;
    setActionLoading(true);
    try {
      await goalsApi.delete(id);
      toast.success('Goal deleted');
      navigate('/');
    } catch { toast.error('Failed to delete goal'); }
    finally { setActionLoading(false); }
  };

  const handleComplete = async () => {
    if (!id) return;
    setActionLoading(true);
    try {
      const updated = await goalsApi.complete(id);
      setGoal(updated);
      toast.success('Goal completed!');
    } catch (err: any) { toast.error(err.response?.data?.error ?? 'Failed to complete goal'); }
    finally { setActionLoading(false); }
  };

  const handleCancel = async () => {
    if (!id || !confirm('Cancel this goal? This action cannot be undone.')) return;
    setActionLoading(true);
    try {
      const updated = await goalsApi.cancel(id);
      setGoal(updated);
      toast.success('Goal cancelled');
    } catch (err: any) { toast.error(err.response?.data?.error ?? 'Failed to cancel goal'); }
    finally { setActionLoading(false); }
  };

  const handleRemoveTransaction = async (transactionId: string) => {
    if (!id || !confirm('Remove this transaction?')) return;
    try {
      await goalsApi.removeTransaction(id, transactionId);
      fetchGoal();
      toast.success('Transaction removed');
    } catch { toast.error('Failed to remove transaction'); }
  };

  if (loading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-8 w-1/3" />
        <Skeleton className="h-4 w-2/3" />
        <Skeleton className="h-32 w-full" />
      </div>
    );
  }

  if (error || !goal) {
    return <ErrorState message={error ?? 'Goal not found'} onRetry={fetchGoal} />;
  }

  const progress = goal.targetAmount > 0
    ? Math.min((goal.currentAmount / goal.targetAmount) * 100, 100)
    : 0;

  return (
    <div className="space-y-6">
      <Link to="/" className="inline-flex items-center gap-1 text-sm text-slate-500 hover:text-indigo-600 transition-colors">
        <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" /></svg>
        Back to goals
      </Link>

      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
        <div className="flex flex-col sm:flex-row sm:items-start sm:justify-between gap-4">
          <div>
            <h1 className="text-2xl font-bold text-slate-900">{goal.name}</h1>
            {goal.description && <p className="mt-1 text-slate-500">{goal.description}</p>}
          </div>
          <span className={`self-start shrink-0 rounded-full px-3 py-1 text-sm font-medium ${
            goal.status === 'Active' ? 'bg-blue-100 text-blue-700' :
            goal.status === 'Completed' ? 'bg-green-100 text-green-700' :
            'bg-slate-100 text-slate-500'
          }`}>
            {goal.status}
          </span>
        </div>

        <div className="mt-6">
          <div className="flex justify-between text-sm mb-2">
            <span className="text-lg font-semibold text-slate-900">{formatCurrency(goal.currentAmount)}</span>
            <span className="text-slate-400">Target: {formatCurrency(goal.targetAmount)}</span>
          </div>
          <div className="h-3 w-full overflow-hidden rounded-full bg-slate-100" role="progressbar" aria-valuenow={progress} aria-valuemin={0} aria-valuemax={100}>
            <div
              className={`h-full rounded-full transition-all duration-500 ${progress >= 80 ? 'bg-green-500' : 'bg-indigo-500'}`}
              style={{ width: `${progress}%` }}
            />
          </div>
          <p className="mt-1 text-xs text-slate-400 text-right">{progress.toFixed(1)}% achieved</p>
        </div>

        <div className="mt-4 flex flex-wrap gap-4 text-sm text-slate-500">
          <span>Deadline: <strong className="text-slate-700">{formatDate(goal.deadline)}</strong></span>
          <span>Created: <strong className="text-slate-700">{formatDate(goal.createdAt)}</strong></span>
        </div>

        {goal.status === 'Active' && (
          <div className="mt-6 flex flex-wrap gap-3">
            <Link to={`/goals/${goal.id}/edit`} className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-50 transition-colors">Edit</Link>
            <Link to={`/goals/${goal.id}/transactions/new`} className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700 transition-colors">+ Add Transaction</Link>
            <button onClick={handleComplete} disabled={actionLoading} className="rounded-lg border border-green-300 px-4 py-2 text-sm font-medium text-green-700 hover:bg-green-50 transition-colors disabled:opacity-50">Complete</button>
            <button onClick={handleCancel} disabled={actionLoading} className="rounded-lg border border-red-300 px-4 py-2 text-sm font-medium text-red-700 hover:bg-red-50 transition-colors disabled:opacity-50">Cancel Goal</button>
            <button onClick={handleDelete} disabled={actionLoading} className="rounded-lg border border-red-300 px-4 py-2 text-sm font-medium text-red-700 hover:bg-red-50 transition-colors disabled:opacity-50">Delete</button>
          </div>
        )}
      </div>

      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
        <h2 className="text-lg font-semibold text-slate-900 mb-4">Transactions</h2>
        {goal.transactions.length === 0 ? (
          <p className="text-sm text-slate-400 py-4 text-center">No transactions yet. Add one to start tracking your progress.</p>
        ) : (
          <div className="divide-y divide-slate-100">
            {goal.transactions.map((t) => (
              <div key={t.id} className="flex items-center justify-between py-3">
                <div>
                  <p className="text-sm font-medium text-slate-900">
                    <span className={t.type === 'Deposit' ? 'text-green-600' : 'text-red-500'}>
                      {t.type === 'Deposit' ? '+' : '-'}{formatCurrency(t.amount)}
                    </span>
                  </p>
                  {t.description && <p className="text-xs text-slate-400">{t.description}</p>}
                </div>
                <div className="flex items-center gap-3">
                  <span className="text-xs text-slate-400">{formatDate(t.date)}</span>
                  {goal.status === 'Active' && (
                    <button
                      onClick={() => handleRemoveTransaction(t.id)}
                      className="text-xs text-red-400 hover:text-red-600 transition-colors"
                      aria-label="Remove transaction"
                    >
                      Remove
                    </button>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
