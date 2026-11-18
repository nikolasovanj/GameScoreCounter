import { Navigate, Route, Routes } from 'react-router-dom'
import LandingPage from './pages/LandingPage';
import TestPage from './pages/TestPage';
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import CreateMatchPage from './pages/match/CreateMatchPage';
import MatchDetailsPage from './pages/match/MatchDetailsPage';
import CreateSchedulePage from './pages/schedule/CreateSchedulePage';
import ScheduleDetailsPage from './pages/schedule/ScheduleDetailsPage';
import InvitaionPage from './pages/schedule/InvitationPage';
import ProfilePage from './pages/user/ProfilePage';


function App() {

  return (
    <Routes>
      <Route path='/test' element={<TestPage />} />
      {/* Auth */}
      <Route path='/login' element={<LoginPage />} />
      <Route path='/register' element={ <RegisterPage /> } />

      {/* Match */}
      <Route path='/matches' element={<CreateMatchPage />} />
      <Route path='/matches/:id' element={<MatchDetailsPage />} />

      {/* Schedule */}
      <Route path='/schedules' element={<CreateSchedulePage />} />
      <Route path='/schedules/:id' element={<ScheduleDetailsPage />} />
      <Route path='schedules/invitation' element={<InvitaionPage />} />

      {/* User */}
      <Route path='/profile/:id' element={<ProfilePage />} />
      {/* Etc */}
      <Route path='/' element={<LandingPage />} />
      <Route path='*' element={<Navigate to="/" replace />} />
    </Routes> 
  );
}

export default App
