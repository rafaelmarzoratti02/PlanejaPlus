import { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { goalsApi } from '../api/client';
import { useToast } from '../components/Toast';
import { Skeleton } from '../components/Skeleton';

export function GoalFormPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const toast = useToast();
  const isEdit = Boolean(id);

  const [name, setName] = useState('');
  const [targetAmount, setTargetAmount] = useState('');
  const [deadline, setDeadline] = useState('');
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(isEdit);
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (!id) return;
    goalsApi.getById(id)
      .then((goal) => {
        setName(goal.name);
        setTargetAmount(goal.targetAmount.toString());
        setDeadline(goal.deadline.split('T')[0]);
        setDescription(goal.description ?? '');
      })
      .catch(() => toast.error('Failed to load goal'))
      .finally(() => setFetching(false));
  }, [id]);

  const validate = () => {
    const errs: Record<string, string> = {};
    if (!name.trim()) errs.name = 'Name is required';
    if (name.length > 200) errs.name = 'Name must be under 200 characters';
    const amount = parseFloat(targetAmount);
    if (!targetAmount || isNaN(amount) || amount <= 0) errs.targetAmount = 'Enter a positive amount';
    if (!deadline) errs.deadline = 'Deadline is required';
    else if (new Date(deadline) < new Date(new Date().toDateString())) errs.deadline = 'Deadline cannot be in the past';
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    setLoading(true);
    try {
      const data = {
        name: name.trim(),
        targetAmount: parseFloat(targetAmount),
        deadline,
        description: description.trim() || undefined,
      };

      if (isEdit && id) {
        await goalsApi.update(id, data);
        toast.success('Goal updated');
        navigate(`/goals/${id}`);
      } else {
        const created = await goalsApi.create(data);
        toast.success('Goal created!');
        navigate(`/goals/${created.id}`);
      }
    } catch (err: any) {
      toast.error(err.response?.data?.error ?? 'Something went wrong');
    } finally {
      setLoading(false);
    }
  };

  if (fetching) {
    return (
      <div className="space-y-6 max-w-lg mx-auto">
        <Skeleton className="h-8 w-1/3" />
        <Skeleton className="h-12 w-full" />
        <Skeleton className="h-12 w-full" />
        <Skeleton className="h-12 w-full" />
      </div>
    );
  }

  return (
    <div className="max-w-lg mx-auto">
      <Link to={isEdit ? `/goals/${id}` : '/'} className="inline-flex items-center gap-1 text-sm text-slate-500 hover:text-indigo-600 transition-colors mb-6">
        <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" /></svg>
        Back
      </Link>

      <h1 className="text-2xl font-bold text-slate-900 mb-6">{isEdit ? 'Edit Goal' : 'Create New Goal'}</h1>

      <form onSubmit={handleSubmit} className="space-y-5" noValidate>
        <div>
          <label htmlFor="name" className="block text-sm font-medium text-slate-700 mb-1">Goal Name</label>
          <input
            id="name"
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            onBlur={validate}
            placeholder="e.g., Emergency Fund"
            className={`w-full rounded-lg border px-3 py-2.5 text-sm outline-none transition-colors focus:ring-2 focus:ring-indigo-500 ${errors.name ? 'border-red-300 bg-red-50' : 'border-slate-300'}`}
          />
          {errors.name && <p className="mt-1 text-xs text-red-500">{errors.name}</p>}
        </div>

        <div>
          <label htmlFor="targetAmount" className="block text-sm font-medium text-slate-700 mb-1">Target Amount ($)</label>
          <input
            id="targetAmount"
            type="number"
            step="0.01"
            min="0.01"
            value={targetAmount}
            onChange={(e) => setTargetAmount(e.target.value)}
            onBlur={validate}
            placeholder="10000.00"
            className={`w-full rounded-lg border px-3 py-2.5 text-sm outline-none transition-colors focus:ring-2 focus:ring-indigo-500 ${errors.targetAmount ? 'border-red-300 bg-red-50' : 'border-slate-300'}`}
          />
          {errors.targetAmount && <p className="mt-1 text-xs text-red-500">{errors.targetAmount}</p>}
        </div>

        <div>
          <label htmlFor="deadline" className="block text-sm font-medium text-slate-700 mb-1">Deadline</label>
          <input
            id="deadline"
            type="date"
            value={deadline}
            onChange={(e) => setDeadline(e.target.value)}
            onBlur={validate}
            className={`w-full rounded-lg border px-3 py-2.5 text-sm outline-none transition-colors focus:ring-2 focus:ring-indigo-500 ${errors.deadline ? 'border-red-300 bg-red-50' : 'border-slate-300'}`}
          />
          {errors.deadline && <p className="mt-1 text-xs text-red-500">{errors.deadline}</p>}
        </div>

        <div>
          <label htmlFor="description" className="block text-sm font-medium text-slate-700 mb-1">Description <span className="text-slate-400">(optional)</span></label>
          <textarea
            id="description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={3}
            placeholder="What is this goal for?"
            className="w-full rounded-lg border border-slate-300 px-3 py-2.5 text-sm outline-none transition-colors focus:ring-2 focus:ring-indigo-500 resize-none"
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full rounded-lg bg-indigo-600 py-2.5 text-sm font-medium text-white hover:bg-indigo-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {loading ? 'Saving...' : isEdit ? 'Update Goal' : 'Create Goal'}
        </button>
      </form>
    </div>
  );
}
