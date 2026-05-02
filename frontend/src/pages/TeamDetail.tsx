import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../services/api'
import { DetailLayout } from '../components/DetailLayout'
import type { AxiosError } from 'axios'

type Team = { id: number; name: string; points?: number }

export const TeamDetail: React.FC = () => {
  const params = useParams()
  const teamId = Number(params.teamId)
  const [team, setTeam] = useState<Team | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const res = await api.get('/teams')
      const teams = (res.data || []) as Team[]
      setTeam(teams.find((t) => t.id === teamId) ?? null)
    } catch (err) {
      const axiosErr = err as AxiosError;
      const msg = axiosErr.response?.data ?? ''
      setError('Unable to load team details.' + msg)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [teamId])

  if (Number.isNaN(teamId)) return <div className="page error">Invalid team.</div>
  if (loading) return <div className="page">Loading team...</div>
  if (error) return <div className="page error">{error}</div>
  if (!team) return <div className="page">Team not found.</div>

  return (
    <DetailLayout
      title={team.name}
      backLink="/league/teams"
      backLabel="Back to Teams"
      items={[
        { label: 'Name', value: team.name },
        { label: 'Points', value: team.points ?? 0 },
      ]}
    />
  )
}
