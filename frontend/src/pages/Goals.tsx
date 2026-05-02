import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'

type Goal = { id: number; playerId: number; matchId: number; teamId: number; timeScored: string; scorerName: string }

export const Goals: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [goals, setGoals] = useState<Goal[]>([])
  const [deleteLoading, setDeleteLoading] = useState<number | null>(null)
  const [deleteSuccess, setDeleteSuccess] = useState<string | null>(null)
  const [deleteError, setDeleteError] = useState<string | null>(null)

  const load = async () => {
    const res = await api.get('/goals')
    setGoals(res.data || [])
  }

  useEffect(() => { load() }, [])

  return (
    <div className="page">
      <div className="card-header">
        <h2>Goals</h2>
        {!readOnly && <Link to="/admin/goals/new">Create Goal</Link>}
      </div>
      <div className="split-horizontal">
        <ul className="scrollable-list">
          {goals.map((g) => (
            <li key={g.id}>
              Player: {g.scorerName} scored at {new Date(g.timeScored).toLocaleTimeString()}{' '}
              <Link to={`/league/goals/${g.id}`}>View details</Link>
              {!readOnly && (
                <button
                  type="button"
                  onClick={async () => {
                    if (!window.confirm('Delete this goal?')) return
                    setDeleteLoading(g.id)
                    setDeleteSuccess(null)
                    setDeleteError(null)
                    try {
                      await api.delete(`/goals/${g.id}`)
                      setGoals((s) => s.filter(x => x.id !== g.id))
                      setDeleteSuccess('Goal deleted successfully.')
                    } catch {
                      setDeleteError('Delete failed')
                    } finally {
                      setDeleteLoading(null)
                    }
                  }}
                  disabled={deleteLoading === g.id}
                >
                  {deleteLoading === g.id ? 'Deleting...' : 'Delete'}
                </button>
              )}
            </li>
          ))}
        </ul>
        {deleteLoading && <p>Deleting...</p>}
        {deleteSuccess && <p className="success">{deleteSuccess}</p>}
        {deleteError && <p className="error">{deleteError}</p>}
      </div>
    </div>
  )
}
