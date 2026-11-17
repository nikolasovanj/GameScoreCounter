import { Navigate, Route, Routes } from 'react-router-dom'
import LandingPage from './pages/LandingPage';
import TestPage from './pages/TestPage';
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';


function App() {

  return (
    <Routes>
      <Route path='/test' element={<TestPage />} />
      {/*Auth */}
      <Route path='/login' element={<LoginPage />} />
      <Route path='/register' element={ <RegisterPage /> } />

      {/*Match */}
      {/* <Route path='/matches' element={<CreateMatchPage />} /> */}
      
      {/*Etc */}
      <Route path='/' element={<LandingPage />} />
      <Route path='*' element={<Navigate to="/" replace />} />
    </Routes> 
  );
}

export default App
