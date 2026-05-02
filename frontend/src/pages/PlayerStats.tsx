import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type Stats = {
  id: number
  playerId: number
  matchId: number
  goals?: number
  assists?: number
  shotsOnTarget?: number
  touches?: number
  passesCompleted?: number
  score?: number
}

const emptyUpdate = { goals: 0, assists: 0, shotsOnTarget: 0, touches: 0, passesCompleted: 0, score: 0 }

export const PlayerStats: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [stats, setStats] = useState<Stats[]>([])
  const [updateForm, setUpdateForm] = useState({ ...emptyUpdate })
  const [editing, setEditing] = useState<Stats | null>(null)
  const [saving, setSaving] = useState(false)
  const [success, setSuccess] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    const res = await api.get('/players/player-stats')
    setStats(res.data || [])
  }

  useEffect(() => { load() }, [])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!editing) return
    setSaving(true)
    setError(null)
    setSuccess(null)
    try {
      await api.put(`/players/player-stats/${editing.id}`, updateForm)
      setSuccess('Player stats updated successfully.')
      setUpdateForm({ ...emptyUpdate })
      setEditing(null)
      load()
    } catch (err) {
      const axiosErr = err as AxiosError<any>
      let msg = ''

      const data = axiosErr.response?.data
      if (typeof data === 'string') {
        msg = data
      } else if (data?.errors) {
        msg = Object.values(data.errors).flat().join(', ')
      } else if (data?.title) {
        msg = data.title
      }

      setError('Unable to save player stats. Try again. ' + msg)
    } finally {
      setSaving(false)
    }
  }

  const startEdit = (s: Stats) => {
    setEditing(s)
    setUpdateForm({
      goals: s.goals ?? 0,
      assists: s.assists ?? 0,
      shotsOnTarget: s.shotsOnTarget ?? 0,
      touches: s.touches ?? 0,
      passesCompleted: s.passesCompleted ?? 0,
      score: s.score ?? 0,
    })
  }

  const handleDelete = async (id: number) => {
    if (!window.confirm('Delete this player stats record?')) return
    try {
      await api.delete(`/players/player-stats/${id}`)
      setStats((s) => s.filter((x) => x.id !== id))
    } catch (err) {
      const axiosErr = err as AxiosError<any>
      let msg = ''

      const data = axiosErr.response?.data
      if (typeof data === 'string') {
        msg = data
      } else if (data?.errors) {
        msg = Object.values(data.errors).flat().join(', ')
      } else if (data?.title) {
        msg = data.title
      }

      alert('Delete failed. ' + msg)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Player Stats</h2>
        {!readOnly && <Link to="/admin/player-stats/new">Create Player Stats</Link>}
      </div>
      <div className="split-horizontal">
        {!readOnly && editing && (
          <form onSubmit={submit}>
            <label>Goals</label>
            <input type="number" value={updateForm.goals} onChange={(e) => setUpdateForm({ ...updateForm, goals: Number(e.target.value) })} />
            <label>Assists</label>
            <input type="number" value={updateForm.assists} onChange={(e) => setUpdateForm({ ...updateForm, assists: Number(e.target.value) })} />
            <label>Shots on target</label>
            <input type="number" value={updateForm.shotsOnTarget} onChange={(e) => setUpdateForm({ ...updateForm, shotsOnTarget: Number(e.target.value) })} />
            <label>Touches</label>
            <input type="number" value={updateForm.touches} onChange={(e) => setUpdateForm({ ...updateForm, touches: Number(e.target.value) })} />
            <label>Passes completed</label>
            <input type="number" value={updateForm.passesCompleted} onChange={(e) => setUpdateForm({ ...updateForm, passesCompleted: Number(e.target.value) })} />
            <label>Score</label>
            <input type="number" value={updateForm.score} onChange={(e) => setUpdateForm({ ...updateForm, score: Number(e.target.value) })} />
            <button type="button" onClick={(e) => { e.preventDefault(); submit(e as any) }}>Save Stats</button>
            <button type="button" onClick={() => { setEditing(null); setUpdateForm({ ...emptyUpdate }) }}>
              Cancel
            </button>
            {saving && <p>Saving...</p>}
            {success && <p className="success">{success}</p>}
            {error && <p className="error">{error}</p>}
          </form>
        )}

        {!editing && (
          <ul className="scrollable-list">
            {stats.map((s) => (
              <li key={s.id} className={readOnly ? 'readonly' : ''}>
                {readOnly
                  ? `Goals ${s.goals ?? 0} · Assists ${s.assists ?? 0} · SOT ${s.shotsOnTarget ?? 0} · Touches ${s.touches ?? 0} · Passes ${s.passesCompleted ?? 0} · Score ${s.score ?? 0}`
                  : `Player ${s.playerId} Match ${s.matchId} Score ${s.score ?? 0}`}{' '}
                <Link to={`/league/player-stats/${s.id}`}>View details</Link>{' '}
                {!readOnly && <button type="button" onClick={() => startEdit(s)}>Edit</button>}
                {!readOnly && <button type="button" onClick={() => handleDelete(s.id)}>Delete</button>}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
