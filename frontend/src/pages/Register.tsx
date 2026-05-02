import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthProvider'

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
    } catch (err: any) {
      setError(err?.response?.data?.message || 'Register failed')
    }
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
