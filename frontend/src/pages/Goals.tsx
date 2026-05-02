import React, { useEffect, useState } from 'react'
import api from '../api'

type Goal = { id: number; playerId: number; matchId: number; teamId: number; timeScored: string; scorerName: string }

export const Goals: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [goals, setGoals] = useState<Goal[]>([])
  const [form, setForm] = useState({ playerId: 1, matchId: 1, teamId: 1, scorerName: '' })

  const load = async () => {
    const res = await api.get('/goals')
    setGoals(res.data || [])
  }

  useEffect(() => { load() }, [])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    await api.post('/goals', form)
    setForm({ playerId: 1, matchId: 1, teamId: 1, scorerName: '' })
    load()
  }

  return (
    <div className="page">
      <h2>Goals</h2>
      <div className="split-horizontal">
        {!readOnly && (
          <form onSubmit={submit}>
            <label>Player ID</label>
            <input type="number" value={form.playerId} onChange={(e) => setForm({ ...form, playerId: Number(e.target.value) })} />
            <label>Match ID</label>
            <input type="number" value={form.matchId} onChange={(e) => setForm({ ...form, matchId: Number(e.target.value) })} />
            <label>Team ID</label>
            <input type="number" value={form.teamId} onChange={(e) => setForm({ ...form, teamId: Number(e.target.value) })} />
            <label>Scorer Name</label>
            <input type="text" value={form.scorerName} onChange={(e) => setForm({ ...form, scorerName: e.target.value })} />
            <button type="submit">Create Goal</button>
          </form>
        )}

        <ul className="scrollable-list">
          {goals.map((g) => (
            <li key={g.id}>Player: {g.scorerName} scored at {new Date(g.timeScored).toLocaleTimeString()}</li>
          ))}
        </ul>
      </div>
    </div>
  )
}
