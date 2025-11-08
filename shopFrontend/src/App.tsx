import Dashboard from './features/pages/products/components/Dashboard';
import { Login } from './features/auth/pages/Login';
import { Register } from './features/auth/pages/Register';
import { useBoundStore } from './store';
import { Navigate, Route, Routes } from 'react-router';

function App() {
  const isAuthenticated = useBoundStore((state) => state.isAuthenticated)

  if (!isAuthenticated) {
    return (
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="*" element={<Login />} />
      </Routes>
    );
  }

  return (
    <Routes>
      <Route path="/" element={<Navigate to="/dashboard" />} />
      <Route path="/dashboard" element={<Dashboard />} />
    </Routes>
  );
}

export default App;
