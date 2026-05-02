import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'

type GoalForm = {
  playerId: number
  matchId: number
  teamId: number
  scorerName: string
}

const emptyForm: GoalForm = {
  playerId: 1,
  matchId: 1,
  teamId: 1,
  scorerName: '',
}

export const CreateGoal: React.FC = () => {
  const [form, setForm] = useState<GoalForm>({ ...emptyForm })
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
      await api.post('/goals', form)
      setSuccess('Goal created successfully.')
      setForm({ ...emptyForm })
    } catch (err) {
      setError('Unable to create goal. Check the form values and try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Goal</h2>
        <Link to="/admin/goals">Back to Goals</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label>Player ID</label>
        <input type="number" value={form.playerId} onChange={(e) => setForm({ ...form, playerId: Number(e.target.value) })} />
        <label>Match ID</label>
        <input type="number" value={form.matchId} onChange={(e) => setForm({ ...form, matchId: Number(e.target.value) })} />
        <label>Team ID</label>
        <input type="number" value={form.teamId} onChange={(e) => setForm({ ...form, teamId: Number(e.target.value) })} />
        <label>Scorer Name</label>
        <input value={form.scorerName} onChange={(e) => setForm({ ...form, scorerName: e.target.value })} placeholder="Scorer Name" />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Goal'}</button>
          <button type="button" onClick={() => navigate('/admin/goals')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success">{success}</p>}
        {error && <p className="error">{error}</p>}
      </form>
    </div>
  )
}
