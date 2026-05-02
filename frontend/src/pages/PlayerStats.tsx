import React, { useEffect, useState } from 'react'
import api from '../api'

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

const emptyCreate = { playerId: 1, matchId: 1, goals: 0, assists: 0, shotsOnTarget: 0, touches: 0, passesCompleted: 0, score: 0 }
const emptyUpdate = { goals: 0, assists: 0, shotsOnTarget: 0, touches: 0, passesCompleted: 0, score: 0 }

export const PlayerStats: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [stats, setStats] = useState<Stats[]>([])
  const [createForm, setCreateForm] = useState({ ...emptyCreate })
  const [updateForm, setUpdateForm] = useState({ ...emptyUpdate })
  const [editing, setEditing] = useState<Stats | null>(null)

  const load = async () => {
    const res = await api.get('/players/player-stats')
    setStats(res.data || [])
  }

  useEffect(() => { load() }, [])

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (editing) {
      await api.put(`/players/player-stats/${editing.id}`, updateForm)
    } else {
      await api.post('/players/player-stats', createForm)
    }
    setCreateForm({ ...emptyCreate })
    setUpdateForm({ ...emptyUpdate })
    setEditing(null)
    load()
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

  return (
    <div className="page">
      <h2>Player Stats</h2>
      <div className="split-horizontal">
        {!readOnly && (
          <form onSubmit={submit}>
            {editing ? (
              <>
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
              </>
            ) : (
              <>
                <label>Player ID</label>
                <input type="number" value={createForm.playerId} onChange={(e) => setCreateForm({ ...createForm, playerId: Number(e.target.value) })} />
                <label>Match ID</label>
                <input type="number" value={createForm.matchId} onChange={(e) => setCreateForm({ ...createForm, matchId: Number(e.target.value) })} />
                <label>Goals</label>
                <input type="number" value={createForm.goals} onChange={(e) => setCreateForm({ ...createForm, goals: Number(e.target.value) })} />
                <label>Assists</label>
                <input type="number" value={createForm.assists} onChange={(e) => setCreateForm({ ...createForm, assists: Number(e.target.value) })} />
                <label>Shots on target</label>
                <input type="number" value={createForm.shotsOnTarget} onChange={(e) => setCreateForm({ ...createForm, shotsOnTarget: Number(e.target.value) })} />
                <label>Touches</label>
                <input type="number" value={createForm.touches} onChange={(e) => setCreateForm({ ...createForm, touches: Number(e.target.value) })} />
                <label>Passes completed</label>
                <input type="number" value={createForm.passesCompleted} onChange={(e) => setCreateForm({ ...createForm, passesCompleted: Number(e.target.value) })} />
                <label>Score</label>
                <input type="number" value={createForm.score} onChange={(e) => setCreateForm({ ...createForm, score: Number(e.target.value) })} />
              </>
            )}
            <button type="submit">{editing ? 'Save' : 'Create'} Stats</button>
            {editing && (
              <button type="button" onClick={() => { setEditing(null); setUpdateForm({ ...emptyUpdate }) }}>
                Cancel
              </button>
            )}
          </form>
        )}

        <ul className="scrollable-list">
          {stats.map((s) => (
            <li key={s.id} className={readOnly ? 'readonly' : ''}>
              {readOnly
                ? `Goals ${s.goals ?? 0} · Assists ${s.assists ?? 0} · SOT ${s.shotsOnTarget ?? 0} · Touches ${s.touches ?? 0} · Passes ${s.passesCompleted ?? 0} · Score ${s.score ?? 0}`
                : `Player ${s.playerId} Match ${s.matchId} Score ${s.score ?? 0}`}{' '}
              {!readOnly && <button type="button" onClick={() => startEdit(s)}>Edit</button>}
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
