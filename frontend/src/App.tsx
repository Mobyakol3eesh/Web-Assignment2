import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import NavBar from './components/NavBar'
import { Login } from './pages/Login'
import { Register } from './pages/Register'

import { LeagueDashboard } from './pages/LeagueDashboard'
import { Teams } from './pages/Teams'
import { Players } from './pages/Players'
import { Matches } from './pages/Matches'
import { Goals } from './pages/Goals'
import { Coaches } from './pages/Coaches'
import { PlayerStats } from './pages/PlayerStats'
import { PlayerStatsDetail } from './pages/PlayerStatsDetail'
import { ProtectedRoute } from './components/ProtectedRoute'
import { AdminRoute } from './components/AdminRoute'

function App() {
  return (
    <div className="app-shell">
      <NavBar />
      <main className="container">
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
            path="/league"
            element={
              <ProtectedRoute>
                <LeagueDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/league/teams"
            element={
              <ProtectedRoute>
                <Teams readOnly />
              </ProtectedRoute>
            }
          />
          <Route
            path="/league/players"
            element={
              <ProtectedRoute>
                <Players readOnly />
              </ProtectedRoute>
            }
          />
          <Route
            path="/league/matches"
            element={
              <ProtectedRoute>
                <Matches readOnly />
              </ProtectedRoute>
            }
          />
          <Route
            path="/league/goals"
            element={
              <ProtectedRoute>
                <Goals readOnly />
              </ProtectedRoute>
            }
          />
          <Route
            path="/league/coaches"
            element={
              <ProtectedRoute>
                <Coaches readOnly />
              </ProtectedRoute>
            }
          />
          <Route
            path="/league/players/:playerId/stats"
            element={
              <ProtectedRoute>
                <PlayerStatsDetail />
              </ProtectedRoute>
            }
          />
          <Route path="/" element={<Navigate to="/league" replace />} />
          
          <Route
            path="/admin/teams"
            element={
              <AdminRoute>
                <Teams />
              </AdminRoute>
            }
          />
          <Route
            path="/admin/players"
            element={
              <AdminRoute>
                <Players />
              </AdminRoute>
            }
          />
          <Route
            path="/admin/matches"
            element={
              <AdminRoute>
                <Matches />
              </AdminRoute>
            }
          />
          <Route
            path="/admin/goals"
            element={
              <AdminRoute>
                <Goals />
              </AdminRoute>
            }
          />
          <Route
            path="/admin/coaches"
            element={
              <AdminRoute>
                <Coaches />
              </AdminRoute>
            }
          />
          <Route
            path="/admin/player-stats"
            element={
              <AdminRoute>
                <PlayerStats />
              </AdminRoute>
            }
          />
          <Route
            path="/admin/players/:playerId/stats"
            element={
              <AdminRoute>
                <PlayerStatsDetail isAdmin />
              </AdminRoute>
            }
          />
          <Route path="*" element={<Navigate to="/league" replace />} />
        </Routes>
      </main>
    </div>
  )
}

export default App
