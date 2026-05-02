import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type Coach = { id: number; name: string; age?: number; experienceYrs?: number; teamId?: number }

const emptyForm = { name: '', age: 30, experienceYrs: 0, teamId: 1 }

export const Coaches: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [coaches, setCoaches] = useState<Coach[]>([])
  const [form, setForm] = useState({ ...emptyForm })
  const [editing, setEditing] = useState<Coach | null>(null)
  const [saving, setSaving] = useState(false)
  const [success, setSuccess] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    const res = await api.get('/coaches')
    setCoaches(res.data || [])
  }

  useEffect(() => { load() }, [])

  const saveEdit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!editing) return
    await api.put(`/coaches/${editing.id}`, form)
    setSaving(true)
    setError(null)
    setSuccess(null)
    try {
      await api.put(`/coaches/${editing.id}`, form)
      setSuccess('Coach updated successfully.')
      setForm({ ...emptyForm })
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

      setError('Unable to save coach. Try again. ' + msg)
    } finally {
      setSaving(false)
    }
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

  const handleDelete = async (id: number) => {
    if (!window.confirm('Delete this coach?')) return
    try {
      await api.delete(`/coaches/${id}`)
      setCoaches((s) => s.filter((c) => c.id !== id))
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
        <h2>Coaches</h2>
        {!readOnly && <Link to="/admin/coaches/new">Create Coach</Link>}
      </div>
      <div className="split-horizontal">
        {!readOnly && editing && (
          <form onSubmit={saveEdit}>
            <label>Name</label>
            <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} placeholder="Name" />
            <label>Age</label>
            <input type="number" value={form.age} onChange={(e) => setForm({ ...form, age: Number(e.target.value) })} />
            <label>Experience (years)</label>
            <input type="number" value={form.experienceYrs} onChange={(e) => setForm({ ...form, experienceYrs: Number(e.target.value) })} />
            <label>Team ID</label>
            <input type="number" value={form.teamId} onChange={(e) => setForm({ ...form, teamId: Number(e.target.value) })} />
            <button type="button" onClick={(e) => { e.preventDefault(); saveEdit(e as any) }}>Save Coach</button>
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
            {coaches.map((c) => (
              <li key={c.id} className={readOnly ? 'readonly' : ''}>
                {readOnly
                  ? `${c.name} · Age ${c.age ?? 'N/A'} · ${c.experienceYrs ?? 0} yrs exp`
                  : `${c.name} (Exp: ${c.experienceYrs})`}{' '}
                <Link to={`/league/coaches/${c.id}`}>View details</Link>{' '}
                {!readOnly && <button type="button" onClick={() => startEdit(c)}>Edit</button>}
                {!readOnly && <button type="button" onClick={() => handleDelete(c.id)}>Delete</button>}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}
