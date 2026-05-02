import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../services/api'
import { DetailLayout } from '../components/DetailLayout'
import type { AxiosError } from 'axios'

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

export const PlayerStatDetail: React.FC = () => {
  const params = useParams()
  const statId = Number(params.statId)
  const [stat, setStat] = useState<Stats | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const res = await api.get('/players/player-stats')
      const stats = (res.data || []) as Stats[]
      setStat(stats.find((s) => s.id === statId) ?? null)
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

      setError('Unable to load player stats details. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [statId])

  if (Number.isNaN(statId)) return <div className="page error">Invalid player stats.</div>
  if (loading) return <div className="page">Loading stats...</div>
  if (error) return <div className="page error">{error}</div>
  if (!stat) return <div className="page">Player stats not found.</div>

  return (
    <DetailLayout
      title="Player Stats"
      backLink="/league/player-stats"
      backLabel="Back to Player Stats"
      items={[
        { label: 'Player ID', value: stat.playerId },
        { label: 'Match ID', value: stat.matchId },
        { label: 'Goals', value: stat.goals ?? 0 },
        { label: 'Assists', value: stat.assists ?? 0 },
        { label: 'Shots on Target', value: stat.shotsOnTarget ?? 0 },
        { label: 'Touches', value: stat.touches ?? 0 },
        { label: 'Passes Completed', value: stat.passesCompleted ?? 0 },
        { label: 'Score', value: stat.score ?? 0 },
      ]}
    />
  )
}
