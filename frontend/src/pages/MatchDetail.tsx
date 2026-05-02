import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../services/api'
import { DetailLayout } from '../components/DetailLayout'
import type { AxiosError } from 'axios'

type Match = {
  id: number
  date: string
  location: string
  homeTeamId: number
  awayTeamId: number
  homeTeamScore?: number
  awayTeamScore?: number
  homeTeamName?: string
  awayTeamName?: string
}

export const MatchDetail: React.FC = () => {
  const params = useParams()
  const matchId = Number(params.matchId)
  const [match, setMatch] = useState<Match | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const res = await api.get('/matches')
      const matches = (res.data || []) as Match[]
      setMatch(matches.find((m) => m.id === matchId) ?? null)
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

      setError('Unable to load match details. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [matchId])

  if (Number.isNaN(matchId)) return <div className="page error">Invalid match.</div>
  if (loading) return <div className="page">Loading match...</div>
  if (error) return <div className="page error">{error}</div>
  if (!match) return <div className="page">Match not found.</div>

  return (
    <DetailLayout
      title="Match Details"
      backLink="/league/matches"
      backLabel="Back to Matches"
      items={[
        { label: 'Date', value: new Date(match.date).toLocaleDateString() },
        { label: 'Location', value: match.location },
        { label: 'Home Team', value: match.homeTeamName ?? `Team ${match.homeTeamId}` },
        { label: 'Away Team', value: match.awayTeamName ?? `Team ${match.awayTeamId}` },
        { label: 'Score', value: `${match.homeTeamScore ?? 0} - ${match.awayTeamScore ?? 0}` },
      ]}
    />
  )
}
