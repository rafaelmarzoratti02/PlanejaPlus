import { Link } from 'react-router-dom';
import type { FinancialGoalResponse } from '../types/goal';

const statusColors: Record<string, string> = {
  Active: 'bg-blue-100 text-blue-700',
  Completed: 'bg-green-100 text-green-700',
  Cancelled: 'bg-slate-100 text-slate-500',
};

function formatCurrency(value: number) {
  return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value);
}

function formatDate(iso: string) {
  return new Date(iso).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
}

export function GoalCard({ goal }: { goal: FinancialGoalResponse }) {
  const progress = goal.targetAmount > 0
    ? Math.min((goal.currentAmount / goal.targetAmount) * 100, 100)
    : 0;

  const isOverdue = goal.status === 'Active' && new Date(goal.deadline) < new Date();

  return (
    <Link
      to={`/goals/${goal.id}`}
      className="group block rounded-2xl border border-slate-200 bg-white p-6 shadow-sm hover:shadow-md hover:border-indigo-300 transition-all"
    >
      <div className="flex items-start justify-between gap-3">
        <h3 className="text-base font-semibold text-slate-900 group-hover:text-indigo-600 transition-colors truncate">
          {goal.name}
        </h3>
        <span className={`shrink-0 rounded-full px-2.5 py-0.5 text-xs font-medium ${statusColors[goal.status] ?? 'bg-slate-100 text-slate-600'}`}>
          {goal.status}
        </span>
      </div>

      {goal.description && (
        <p className="mt-1 text-sm text-slate-500 line-clamp-1">{goal.description}</p>
      )}

      <div className="mt-4">
        <div className="flex justify-between text-sm mb-1.5">
          <span className="font-medium text-slate-700">{formatCurrency(goal.currentAmount)}</span>
          <span className="text-slate-400">of {formatCurrency(goal.targetAmount)}</span>
        </div>
        <div className="h-2 w-full overflow-hidden rounded-full bg-slate-100" role="progressbar" aria-valuenow={progress} aria-valuemin={0} aria-valuemax={100}>
          <div
            className={`h-full rounded-full transition-all duration-500 ${
              goal.status === 'Completed' ? 'bg-green-500' :
              goal.status === 'Cancelled' ? 'bg-slate-300' :
              progress >= 80 ? 'bg-green-500' :
              progress >= 50 ? 'bg-indigo-500' :
              'bg-indigo-400'
            }`}
            style={{ width: `${progress}%` }}
          />
        </div>
      </div>

      <div className="mt-3 flex items-center justify-between text-xs text-slate-400">
        <span>{goal.transactions.length} transaction{goal.transactions.length !== 1 ? 's' : ''}</span>
        <span className={isOverdue ? 'text-red-500 font-medium' : ''}>
          {isOverdue ? 'Overdue · ' : ''}Deadline: {formatDate(goal.deadline)}
        </span>
      </div>
    </Link>
  );
}
