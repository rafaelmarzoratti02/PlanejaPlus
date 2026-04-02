import { useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { goalsApi } from '../api/client';
import { useToast } from '../components/Toast';

export function AddTransactionPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const toast = useToast();

  const [amount, setAmount] = useState('');
  const [type, setType] = useState<'1' | '2'>('1');
  const [date, setDate] = useState(new Date().toISOString().split('T')[0]);
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  const validate = () => {
    const errs: Record<string, string> = {};
    const num = parseFloat(amount);
    if (!amount || isNaN(num) || num <= 0) errs.amount = 'Enter a positive amount';
    if (!date) errs.date = 'Date is required';
    else if (new Date(date) > new Date()) errs.date = 'Date cannot be in the future';
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id || !validate()) return;

    setLoading(true);
    try {
      await goalsApi.addTransaction(id, {
        amount: parseFloat(amount),
        type: parseInt(type),
        date,
        description: description.trim() || undefined,
      });
      toast.success('Transaction added!');
      navigate(`/goals/${id}`);
    } catch (err: any) {
      toast.error(err.response?.data?.error ?? 'Failed to add transaction');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-lg mx-auto">
      <Link to={`/goals/${id}`} className="inline-flex items-center gap-1 text-sm text-slate-500 hover:text-indigo-600 transition-colors mb-6">
        <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" /></svg>
        Back to goal
      </Link>

      <h1 className="text-2xl font-bold text-slate-900 mb-6">Add Transaction</h1>

      <form onSubmit={handleSubmit} className="space-y-5" noValidate>
        <div>
          <label className="block text-sm font-medium text-slate-700 mb-2">Type</label>
          <div className="flex gap-3">
            <button
              type="button"
              onClick={() => setType('1')}
              className={`flex-1 rounded-lg border py-2.5 text-sm font-medium transition-colors ${
                type === '1'
                  ? 'border-green-500 bg-green-50 text-green-700'
                  : 'border-slate-300 text-slate-500 hover:bg-slate-50'
              }`}
            >
              Deposit
            </button>
            <button
              type="button"
              onClick={() => setType('2')}
              className={`flex-1 rounded-lg border py-2.5 text-sm font-medium transition-colors ${
                type === '2'
                  ? 'border-red-500 bg-red-50 text-red-700'
                  : 'border-slate-300 text-slate-500 hover:bg-slate-50'
              }`}
            >
              Withdraw
            </button>
          </div>
        </div>

        <div>
          <label htmlFor="amount" className="block text-sm font-medium text-slate-700 mb-1">Amount ($)</label>
          <input
            id="amount"
            type="number"
            step="0.01"
            min="0.01"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            onBlur={validate}
            placeholder="500.00"
            className={`w-full rounded-lg border px-3 py-2.5 text-sm outline-none transition-colors focus:ring-2 focus:ring-indigo-500 ${errors.amount ? 'border-red-300 bg-red-50' : 'border-slate-300'}`}
          />
          {errors.amount && <p className="mt-1 text-xs text-red-500">{errors.amount}</p>}
        </div>

        <div>
          <label htmlFor="date" className="block text-sm font-medium text-slate-700 mb-1">Date</label>
          <input
            id="date"
            type="date"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            onBlur={validate}
            className={`w-full rounded-lg border px-3 py-2.5 text-sm outline-none transition-colors focus:ring-2 focus:ring-indigo-500 ${errors.date ? 'border-red-300 bg-red-50' : 'border-slate-300'}`}
          />
          {errors.date && <p className="mt-1 text-xs text-red-500">{errors.date}</p>}
        </div>

        <div>
          <label htmlFor="desc" className="block text-sm font-medium text-slate-700 mb-1">Description <span className="text-slate-400">(optional)</span></label>
          <input
            id="desc"
            type="text"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="Monthly savings"
            className="w-full rounded-lg border border-slate-300 px-3 py-2.5 text-sm outline-none transition-colors focus:ring-2 focus:ring-indigo-500"
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full rounded-lg bg-indigo-600 py-2.5 text-sm font-medium text-white hover:bg-indigo-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {loading ? 'Adding...' : 'Add Transaction'}
        </button>
      </form>
    </div>
  );
}
