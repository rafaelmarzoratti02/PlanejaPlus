import { useEffect, useState } from 'react';

type ToastType = 'success' | 'error';

interface ToastMessage {
  id: number;
  text: string;
  type: ToastType;
}

let addToast: (text: string, type: ToastType) => void;

export function useToast() {
  return {
    success: (text: string) => addToast?.(text, 'success'),
    error: (text: string) => addToast?.(text, 'error'),
  };
}

export function ToastContainer() {
  const [toasts, setToasts] = useState<ToastMessage[]>([]);

  useEffect(() => {
    addToast = (text: string, type: ToastType) => {
      const id = Date.now();
      setToasts((prev) => [...prev, { id, text, type }]);
      setTimeout(() => setToasts((prev) => prev.filter((t) => t.id !== id)), 3000);
    };
  }, []);

  return (
    <div className="fixed bottom-4 right-4 z-50 flex flex-col gap-2" aria-live="polite">
      {toasts.map((t) => (
        <div
          key={t.id}
          className={`rounded-lg px-4 py-3 text-sm font-medium text-white shadow-lg transition-all animate-in slide-in-from-right ${
            t.type === 'success' ? 'bg-green-600' : 'bg-red-600'
          }`}
        >
          {t.text}
        </div>
      ))}
    </div>
  );
}
