import React, { useEffect, useState } from 'react'
import api from '../api'

type Team = { id: number; name: string; points?: number }

export const Teams: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [teams, setTeams] = useState<Team[]>([])
  const [name, setName] = useState('')
  const [points, setPoints] = useState<number>(0)
  const [editing, setEditing] = useState<Team | null>(null)

  const load = async () => {
    const res = await api.get('/teams')
    setTeams(res.data || [])
  }

  useEffect(() => {
    load()
  }, [])

  const create = async (e: React.FormEvent) => {
    e.preventDefault()
    await api.post('/teams', { name })
    setName('')
    load()
  }

  const startEdit = (t: Team) => {
    setEditing(t)
    setName(t.name)
    setPoints(t.points ?? 0)
  }

  const saveEdit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!editing) return
    await api.put(`/teams/${editing.id}`, { name, points })
    setEditing(null)
    setName('')
    setPoints(0)
    load()
  }

  return (
    <div className="page">
      <h2>Teams</h2>
      <div className="split-horizontal">
        {!readOnly && (
          <form onSubmit={editing ? saveEdit : create}>
            <label>Team name</label>
            <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Team name" />
            {editing && (
              <>
                <label>Points</label>
                <input
                  type="number"
                  value={points}
                  onChange={(e) => setPoints(Number(e.target.value))}
                  placeholder="Points"
                />
              </>
            )}
            <button type="submit">{editing ? 'Save' : 'Create'}</button>
            {editing && (
              <button
                type="button"
                onClick={() => {
                  setEditing(null)
                  setName('')
                  setPoints(0)
                }}
              >
                Cancel
              </button>
            )}
          </form>
        )}

        <ul className="scrollable-list">
          {teams.map((t) => (
            <li key={t.id}>
              {t.name} (Points: {t.points ?? 0}){!readOnly && ` Team ID: ${t.id}`}{' '}
              {!readOnly && <button type="button" onClick={() => startEdit(t)}>Edit</button>}
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
