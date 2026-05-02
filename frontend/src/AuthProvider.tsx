import React, { createContext, useContext, useEffect, useState } from 'react'
import api from './services/api'

type AuthContextType = {
    isAuthenticated: boolean
    loading: boolean
    login: (username: string, password: string) => Promise<void>
    register: (username: string, password: string) => Promise<void>
    logout: () => Promise<void>
    role: string | null
    checkAuth: () => Promise<void>
}

const AuthContext = createContext<AuthContextType | null>(null)

export const useAuth = () => {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [role, setRole] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)

  const checkAuth = async () => {
    setLoading(true)
    try {
      const res = await api.get('/auth/me')
      setIsAuthenticated(true)
      setRole(res.data?.role ?? 'User')
    } catch (err) {
      setIsAuthenticated(false)
      setRole(null)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    checkAuth()
  }, [])

  const login = async (username: string, password: string) => {
    const res = await api.post('/auth/login', { username, password })
    if (res.status === 200) {
      setIsAuthenticated(true)
      setRole(res.data?.role ?? 'User')
    }
  }

  const register = async (username: string, password: string) => {
    const res = await api.post('/auth/register', { username, password })
    if (res.status === 200) {
      setIsAuthenticated(true)
      setRole(res.data?.role ?? 'User')
    }
  }

  const logout = async () => {
    await api.post('/auth/logout')
    setIsAuthenticated(false)
    setRole(null)
  }

  return (
    <AuthContext.Provider value={{ isAuthenticated, role, loading, login, register, logout, checkAuth }}>
      {children}
    </AuthContext.Provider>
  )
}
