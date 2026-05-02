import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type PlayerStatsForm = {
  playerId: number
  matchId: number
  goals: number
  assists: number
  shotsOnTarget: number
  touches: number
  passesCompleted: number
  score: number
}

const emptyForm: PlayerStatsForm = {
  playerId: 1,
  matchId: 1,
  goals: 0,
  assists: 0,
  shotsOnTarget: 0,
  touches: 0,
  passesCompleted: 0,
  score: 0,
}

export const CreatePlayerStats: React.FC = () => {
  const [form, setForm] = useState<PlayerStatsForm>({ ...emptyForm })
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
      await api.post('/players/player-stats', form)
      setSuccess('Player stats created successfully.')
      setForm({ ...emptyForm })
    } catch (err) {
        const axiosErr = err as AxiosError;
        const msg = axiosErr.response?.data ?? ''
      setError('Unable to create player stats. Check the form values and try again.' + msg)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Player Stats</h2>
        <Link to="/admin/player-stats">Back to Player Stats</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label>Player ID</label>
        <input type="number" value={form.playerId} onChange={(e) => setForm({ ...form, playerId: Number(e.target.value) })} />
        <label>Match ID</label>
        <input type="number" value={form.matchId} onChange={(e) => setForm({ ...form, matchId: Number(e.target.value) })} />
        <label>Goals</label>
        <input type="number" value={form.goals} onChange={(e) => setForm({ ...form, goals: Number(e.target.value) })} />
        <label>Assists</label>
        <input type="number" value={form.assists} onChange={(e) => setForm({ ...form, assists: Number(e.target.value) })} />
        <label>Shots on target</label>
        <input type="number" value={form.shotsOnTarget} onChange={(e) => setForm({ ...form, shotsOnTarget: Number(e.target.value) })} />
        <label>Touches</label>
        <input type="number" value={form.touches} onChange={(e) => setForm({ ...form, touches: Number(e.target.value) })} />
        <label>Passes completed</label>
        <input type="number" value={form.passesCompleted} onChange={(e) => setForm({ ...form, passesCompleted: Number(e.target.value) })} />
        <label>Score</label>
        <input type="number" value={form.score} onChange={(e) => setForm({ ...form, score: Number(e.target.value) })} />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Stats'}</button>
          <button type="button" onClick={() => navigate('/admin/player-stats')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success">{success}</p>}
        {error && <p className="error">{error}</p>}
      </form>
    </div>
  )
}
