import React, { useState } from 'react'
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthProvider'
import type { AxiosError } from 'axios'

export const Register: React.FC = () => {
  const auth = useAuth()
  const navigate = useNavigate()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      await auth.register(username, password)
      navigate('/')
    } catch (err) {
      const axiosErr = err as AxiosError
      const msg = axiosErr.response?.data ?? ''
      setError('Register failed ' + msg)
    }
  }

  if (auth.isAuthenticated) {
    return <Navigate to="/league" replace />
  }

  return (
    <div className="page">
      <h2>Register</h2>
      <form onSubmit={submit}>
        <label>Username</label>
        <input value={username} onChange={(e) => setUsername(e.target.value)} />
        <label>Password</label>
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
        <button type="submit">Register</button>
      </form>
      {error && <div className="error">{error}</div>}
    </div>
  )
}
