import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { goalsApi } from '../api/client';
import { GoalCard } from '../components/GoalCard';
import { GoalCardSkeleton } from '../components/Skeleton';
import { EmptyState } from '../components/EmptyState';
import { ErrorState } from '../components/ErrorState';
import type { FinancialGoalResponse } from '../types/goal';

export function GoalsListPage() {
  const [goals, setGoals] = useState<FinancialGoalResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchGoals = () => {
    setLoading(true);
    setError(null);
    goalsApi.getAll()
      .then(setGoals)
      .catch((err) => setError(err.response?.data?.error ?? 'Failed to load goals'))
      .finally(() => setLoading(false));
  };

  useEffect(() => { fetchGoals(); }, []);

  if (loading) {
    return (
      <div>
        <h1 className="text-2xl font-bold text-slate-900 mb-6">My Financial Goals</h1>
        <div className="grid gap-4 sm:grid-cols-2">
          {Array.from({ length: 4 }).map((_, i) => <GoalCardSkeleton key={i} />)}
        </div>
      </div>
    );
  }

  if (error) {
    return <ErrorState message={error} onRetry={fetchGoals} />;
  }

  if (goals.length === 0) {
    return (
      <EmptyState
        title="No goals yet"
        description="Start tracking your financial future by creating your first goal."
        action={
          <Link
            to="/goals/new"
            className="rounded-lg bg-indigo-600 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-700 transition-colors"
          >
            Create your first goal
          </Link>
        }
      />
    );
  }

  return (
    <div>
      <h1 className="text-2xl font-bold text-slate-900 mb-6">My Financial Goals</h1>
      <div className="grid gap-4 sm:grid-cols-2">
        {goals.map((goal) => (
          <GoalCard key={goal.id} goal={goal} />
        ))}
      </div>
    </div>
  );
}
