import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'

type PlayerForm = {
  name: string
  age: number
  position: string
  marketValue: number
  teamId: number
}

const emptyForm: PlayerForm = {
  name: '',
  age: 18,
  position: '',
  marketValue: 0,
  teamId: 1,
}

export const CreatePlayer: React.FC = () => {
  const [form, setForm] = useState<PlayerForm>({ ...emptyForm })
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const navigate = useNavigate()

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError(null)
    setSuccess(null)
    try {
      await api.post('/players', form)
      setSuccess('Player created successfully.')
      setForm({ ...emptyForm })
    } catch (err) {
      setError('Unable to create player. Check the form values and try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Player</h2>
        <Link to="/admin/players">Back to Players</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label>Name</label>
        <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} placeholder="Name" />
        <label>Age</label>
        <input type="number" value={form.age} onChange={(e) => setForm({ ...form, age: Number(e.target.value) })} />
        <label>Position</label>
        <input value={form.position} onChange={(e) => setForm({ ...form, position: e.target.value })} placeholder="Position" />
        <label>Market value</label>
        <input type="number" value={form.marketValue} onChange={(e) => setForm({ ...form, marketValue: Number(e.target.value) })} />
        <label>Team ID</label>
        <input type="number" value={form.teamId} onChange={(e) => setForm({ ...form, teamId: Number(e.target.value) })} />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Player'}</button>
          <button type="button" onClick={() => navigate('/admin/players')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success">{success}</p>}
        {error && <p className="error">{error}</p>}
      </form>
    </div>
  )
}
