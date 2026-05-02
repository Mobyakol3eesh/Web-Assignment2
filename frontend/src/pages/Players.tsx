import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios';

type Player = { id: number; name: string; age: number; position: string; marketValue: number; teamName: string , teamID: number}

const emptyForm = { name: '', age: 18, position: '', marketValue: 0, teamName: '', teamID: 0 }

export const Players: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [players, setPlayers] = useState<Player[]>([])
  const [form, setForm] = useState({ ...emptyForm })
  const [editing, setEditing] = useState<Player | null>(null)
  const [saving, setSaving] = useState(false)
  const [success, setSuccess] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    const res = await api.get('/players')
    setPlayers(res.data || [])
  }

  useEffect(() => { load() }, [])

  const saveEdit = async () => {
    if (!editing) return
    setSaving(true)
    setError(null)
    setSuccess(null)
    try {
      await api.put(`/players/${editing.id}`, form)
      setSuccess('Player updated successfully.')
      setForm({ ...emptyForm })
      setEditing(null)
      load()
    } catch (err) {
       
    const axiosErr = err as AxiosError<any>;
    let msg = '';

    const data = axiosErr.response?.data;
    if (typeof data === 'string') {
      msg = data;
      
    }
    else if (data?.errors) {
        msg = Object.values(data.errors)
            .flat()
            .join(', ');
    } else if (data?.title) {
        msg = data.title;
    }

    setError('Unable to save player. Try again. ' + msg);
    

    } finally {
      setSaving(false)
    }
  }

  const startEdit = (p: Player) => {
    setEditing(p)
    setForm({
      name: p.name,
      age: p.age,
      position: p.position,
      marketValue: p.marketValue,
      teamName: p.teamName ?? '',
      teamID: p.teamID ?? 0,
    })
  }

  const handleDelete = async (id: number) => {
    if (!window.confirm('Delete this player and related records?')) return
    try {
      await api.delete(`/players/${id}`)
      setPlayers((s) => s.filter((p) => p.id !== id))
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
            <input type="number" value={form.teamID} onChange={(e) => setForm({ ...form, teamID: Number(e.target.value) })} placeholder="Team ID" />
            <button type="button" onClick={(e) => { e.preventDefault(); saveEdit(e as any) }}>Save Player</button>
            <button type="button" onClick={() => { setEditing(null); setForm({ ...emptyForm }) }}>
              Cancel
            </button>
            {saving && <p>Saving...</p>}
            {success && <p className="success">{success}</p>}
            {error && <p className="error">{error}</p>}
          </form>
        )}

        {!editing && (
          <ul className="scrollable-list">
            {players.map((p) => (
              <li key={p.id}>
                {p.name} - {p.position} - Age {p.age} - Team {p.teamName}{' '}
                <Link to={`/league/players/${p.id}`}>View details</Link>{' '}
                <Link to={(readOnly ? '/league' : '/admin') + `/players/${p.id}/stats`}>Stats</Link>{' '}
                {!readOnly && <button type="button" onClick={() => startEdit(p)}>Edit</button>}
                {!readOnly && <button type="button" onClick={() => handleDelete(p.id)}>Delete</button>}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
