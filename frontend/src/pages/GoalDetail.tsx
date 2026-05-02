import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../services/api'
import { DetailLayout } from '../components/DetailLayout'

type Goal = { id: number; teamName: string; timeScored: string; scorerName: string }

export const GoalDetail: React.FC = () => {
  const params = useParams()
  const goalId = Number(params.goalId)
  const [goal, setGoal] = useState<Goal | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const res = await api.get('/goals')
      const goals = (res.data || []) as Goal[]
      setGoal(goals.find((g) => g.id === goalId) ?? null)
    } catch (err) {
      setError('Unable to load goal details.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [goalId])

  if (Number.isNaN(goalId)) return <div className="page error">Invalid goal.</div>
  if (loading) return <div className="page">Loading goal...</div>
  if (error) return <div className="page error">{error}</div>
  if (!goal) return <div className="page">Goal not found.</div>

  return (
    <DetailLayout
      title="Goal Details"
      backLink="/league/goals"
      backLabel="Back to Goals"
      items={[
        { label: 'Scorer', value: goal.scorerName },
        { label: 'Time Scored', value: new Date(goal.timeScored).toLocaleTimeString() },
        { label: 'Team', value: goal.teamName },
      ]}
    />
  )
}
