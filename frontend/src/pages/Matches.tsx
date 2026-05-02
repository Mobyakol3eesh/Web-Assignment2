import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'

type Match = {
  id: number
  date: string
  location: string
  homeTeamId: number
  awayTeamId: number
  homeTeamScore?: number
  awayTeamScore?: number
  homeTeamName?: string
  awayTeamName?: string
}

const emptyForm = { date: new Date().toISOString().slice(0, 10), location: '', homeTeamId: 1, awayTeamId: 2, homeTeamScore: 0, awayTeamScore: 0 }

export const Matches: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [matches, setMatches] = useState<Match[]>([])
  const [form, setForm] = useState({ ...emptyForm })
  const [editing, setEditing] = useState<Match | null>(null)

  const load = async () => {
    const res = await api.get('/matches')
    setMatches(res.data || [])
  }

  useEffect(() => { load() }, [])

  const saveEdit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!editing) return
    const payload = { ...form, date: new Date(form.date) }
    await api.put(`/matches/${editing.id}`, payload)
    setForm({ ...emptyForm })
    setEditing(null)
    load()
  }

  const startEdit = (m: Match) => {
    setEditing(m)
    setForm({
      date: new Date(m.date).toISOString().slice(0, 10),
      location: m.location,
      homeTeamId: m.homeTeamId,
      awayTeamId: m.awayTeamId,
      homeTeamScore: m.homeTeamScore ?? 0,
      awayTeamScore: m.awayTeamScore ?? 0,
    })
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Matches</h2>
        {!readOnly && <Link to="/admin/matches/new">Create Match</Link>}
      </div>
      <div className="split-horizontal">
        {!readOnly && editing && (
          <form onSubmit={saveEdit}>
            <label>Date</label>
            <input type="date" value={form.date} onChange={(e) => setForm({ ...form, date: e.target.value })} />
            <label>Location</label>
            <input value={form.location} onChange={(e) => setForm({ ...form, location: e.target.value })} placeholder="Location" />
            <label>Home team ID</label>
            <input type="number" value={form.homeTeamId} onChange={(e) => setForm({ ...form, homeTeamId: Number(e.target.value) })} />
            <label>Away team ID</label>
            <input type="number" value={form.awayTeamId} onChange={(e) => setForm({ ...form, awayTeamId: Number(e.target.value) })} />
            <label>Home team score</label>
            <input type="number" value={form.homeTeamScore} onChange={(e) => setForm({ ...form, homeTeamScore: Number(e.target.value) })} />
            <label>Away team score</label>
            <input type="number" value={form.awayTeamScore} onChange={(e) => setForm({ ...form, awayTeamScore: Number(e.target.value) })} />
            <button type="submit">Save Match</button>
            <button type="button" onClick={() => { setEditing(null); setForm({ ...emptyForm }) }}>
              Cancel
            </button>
          </form>
        )}

        {!editing && (
          <ul className="scrollable-list">
            {matches.map((m) => (
              <li key={m.id}>
                {readOnly ? (
                  <>
                    {m.homeTeamName ?? `Team ${m.homeTeamId}`} vs {m.awayTeamName ?? `Team ${m.awayTeamId}`}{' '}
                    ({m.homeTeamScore ?? 0} - {m.awayTeamScore ?? 0}) · {new Date(m.date).toLocaleDateString()} · {m.location}
                  </>
                ) : (
                  <>
                    {m.location} - {new Date(m.date).toLocaleDateString()} (H{m.homeTeamId} {m.homeTeamScore ?? 0} : {m.awayTeamScore ?? 0} A{m.awayTeamId})
                  </>
                )}{' '}
                <Link to={`/league/matches/${m.id}`}>View details</Link>{' '}
                {!readOnly && <button type="button" onClick={() => startEdit(m)}>Edit</button>}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
