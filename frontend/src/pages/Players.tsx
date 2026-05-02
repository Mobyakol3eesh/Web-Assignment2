import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'

type Player = { id: number; name: string; age: number; position: string; marketValue: number; teamName: string }

const emptyForm = { name: '', age: 18, position: '', marketValue: 0, teamId: 1 }

export const Players: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [players, setPlayers] = useState<Player[]>([])
  const [form, setForm] = useState({ ...emptyForm })
  const [editing, setEditing] = useState<Player | null>(null)

  const load = async () => {
    const res = await api.get('/players')
    setPlayers(res.data || [])
  }

  useEffect(() => { load() }, [])

  const saveEdit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!editing) return
    await api.put(`/players/${editing.id}`, form)
    setForm({ ...emptyForm })
    setEditing(null)
    load()
  }

  const startEdit = (p: Player) => {
    setEditing(p)
    setForm({
      name: p.name,
      age: p.age,
      position: p.position,
      marketValue: p.marketValue,
      teamName: p.teamName ?? '',
    })
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Players</h2>
        {!readOnly && <Link to="/admin/players/new">Create Player</Link>}
      </div>
      <div className="split-horizontal">
        {!readOnly && editing && (
          <form onSubmit={saveEdit}>
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
            <button type="submit">Save Player</button>
            <button type="button" onClick={() => { setEditing(null); setForm({ ...emptyForm }) }}>
              Cancel
            </button>
          </form>
        )}

        <ul className="scrollable-list">
          {players.map((p) => (
            <li key={p.id}>
              {p.name} - {p.position} - Age {p.age} - Team {p.teamName}{' '}
              <Link to={`/league/players/${p.id}`}>View details</Link>{' '}
              <Link to={(readOnly ? '/league' : '/admin') + `/players/${p.id}/stats`}>Stats</Link>{' '}
              {!readOnly && <button type="button" onClick={() => startEdit(p)}>Edit</button>}
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
