import React, { useEffect, useState } from 'react'
import api from '../api'

type Coach = { id: number; name: string; age?: number; experienceYrs?: number; teamId?: number }

const emptyForm = { name: '', age: 30, experienceYrs: 0, teamId: 1 }

export const Coaches: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [coaches, setCoaches] = useState<Coach[]>([])
  const [form, setForm] = useState({ ...emptyForm })
  const [editing, setEditing] = useState<Coach | null>(null)

  const load = async () => {
    const res = await api.get('/coaches')
    setCoaches(res.data || [])
  }

  useEffect(() => { load() }, [])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (editing) {
      await api.put(`/coaches/${editing.id}`, form)
    } else {
      await api.post('/coaches', form)
    }
    setForm({ ...emptyForm })
    setEditing(null)
    load()
  }

  const startEdit = (c: Coach) => {
    setEditing(c)
    setForm({
      name: c.name,
      age: c.age ?? 30,
      experienceYrs: c.experienceYrs ?? 0,
      teamId: c.teamId ?? 1,
    })
  }

  return (
    <div className="page">
      <h2>Coaches</h2>
      <div className="split-horizontal">
        {!readOnly && (
          <form onSubmit={submit}>
            <label>Name</label>
            <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} placeholder="Name" />
            <label>Age</label>
            <input type="number" value={form.age} onChange={(e) => setForm({ ...form, age: Number(e.target.value) })} />
            <label>Experience (years)</label>
            <input type="number" value={form.experienceYrs} onChange={(e) => setForm({ ...form, experienceYrs: Number(e.target.value) })} />
            <label>Team ID</label>
            <input type="number" value={form.teamId} onChange={(e) => setForm({ ...form, teamId: Number(e.target.value) })} />
            <button type="submit">{editing ? 'Save' : 'Create'} Coach</button>
            {editing && (
              <button type="button" onClick={() => { setEditing(null); setForm({ ...emptyForm }) }}>
                Cancel
              </button>
            )}
          </form>
        )}

        <ul className="scrollable-list">
          {coaches.map((c) => (
            <li key={c.id} className={readOnly ? 'readonly' : ''}>
              {readOnly
                ? `${c.name} · Age ${c.age ?? 'N/A'} · ${c.experienceYrs ?? 0} yrs exp`
                : `${c.name} (Exp: ${c.experienceYrs})`}{' '}
              {!readOnly && <button type="button" onClick={() => startEdit(c)}>Edit</button>}
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
