import React from 'react'
import { Navigate } from 'react-router-dom'
import { useAuth } from '../AuthProvider'

export const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const auth = useAuth()

  if (auth.loading) return <div>Loading...</div>
  if (!auth.isAuthenticated) return <Navigate to="/login" replace />
  

  return <>{children}</>
}
