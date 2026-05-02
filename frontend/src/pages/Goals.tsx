import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'

type Goal = { id: number; playerId: number; matchId: number; teamId: number; timeScored: string; scorerName: string }

export const Goals: React.FC<{ readOnly?: boolean }> = ({ readOnly = false }) => {
  const [goals, setGoals] = useState<Goal[]>([])

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
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
