import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../services/api'
import { DetailLayout } from '../components/DetailLayout'

type Coach = { id: number; name: string; age?: number; experienceYrs?: number; teamName?: string }

export const CoachDetail: React.FC = () => {
  const params = useParams()
  const coachId = Number(params.coachId)
  const [coach, setCoach] = useState<Coach | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const res = await api.get('/coaches')
      const coaches = (res.data || []) as Coach[]
      setCoach(coaches.find((c) => c.id === coachId) ?? null)
    } catch (err) {
      setError('Unable to load coach details.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [coachId])

  if (Number.isNaN(coachId)) return <div className="page error">Invalid coach.</div>
  if (loading) return <div className="page">Loading coach...</div>
  if (error) return <div className="page error">{error}</div>
  if (!coach) return <div className="page">Coach not found.</div>

  return (
    <DetailLayout
      title={coach.name}
      backLink="/league/coaches"
      backLabel="Back to Coaches"
      items={[
        { label: 'Name', value: coach.name },
        { label: 'Age', value: coach.age ?? 'N/A' },
        { label: 'Experience', value: `${coach.experienceYrs ?? 0} years` },
        { label: 'Team', value: coach.teamName ?? 'N/A' },
      ]}
    />
  )
}
