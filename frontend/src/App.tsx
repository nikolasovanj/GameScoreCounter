import { Navigate, Route, Routes } from 'react-router-dom'
import TestPage from './pages/TestPage'
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import { LandingPage } from './pages/LandingPage';

function App() {

  return (
    <Routes>
      <Route path='/' element={<LandingPage />} />
      <Route path='/login' element={<LoginPage />} />
      <Route path='/register' element={ <RegisterPage /> } />
      <Route path='/test' element={<TestPage />} />
      <Route path='*' element={<Navigate to="/" replace />} />
    </Routes> 
  );
}

export default App
