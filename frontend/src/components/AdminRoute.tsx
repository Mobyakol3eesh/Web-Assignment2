import React from 'react'
import { Navigate } from 'react-router-dom'
import { useAuth } from '../AuthProvider'

export const AdminRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const auth = useAuth()

  if (auth.loading) return <div>Loading...</div>
  if (!auth.isAuthenticated) return <Navigate to="/login" replace />
  if (auth.role !== 'Admin') return <Navigate to="/league" replace />

  return <>{children}</>
}
