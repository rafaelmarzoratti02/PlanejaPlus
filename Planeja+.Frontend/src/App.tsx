import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { GoalsListPage } from './pages/GoalsListPage';
import { GoalDetailPage } from './pages/GoalDetailPage';
import { GoalFormPage } from './pages/GoalFormPage';
import { AddTransactionPage } from './pages/AddTransactionPage';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<GoalsListPage />} />
          <Route path="/goals/new" element={<GoalFormPage />} />
          <Route path="/goals/:id" element={<GoalDetailPage />} />
          <Route path="/goals/:id/edit" element={<GoalFormPage />} />
          <Route path="/goals/:id/transactions/new" element={<AddTransactionPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
