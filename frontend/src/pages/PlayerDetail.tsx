import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../services/api'
import { DetailLayout } from '../components/DetailLayout'
import type { AxiosError } from 'axios'

type Player = {
  id: number
  name: string
  age: number
  position: string
  marketValue: number
  teamName?: string
}

export const PlayerDetail: React.FC = () => {
  const params = useParams()
  const playerId = Number(params.playerId)
  const [player, setPlayer] = useState<Player | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const res = await api.get('/players')
      const players = (res.data || []) as Player[]
      setPlayer(players.find((p) => p.id === playerId) ?? null)
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

      setError('Unable to load player details. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [playerId])

  if (Number.isNaN(playerId)) return <div className="page error">Invalid player.</div>
  if (loading) return <div className="page">Loading player...</div>
  if (error) return <div className="page error">{error}</div>
  if (!player) return <div className="page">Player not found.</div>

  return (
    <DetailLayout
      title={player.name}
      backLink="/league/players"
      backLabel="Back to Players"
      items={[
        { label: 'Name', value: player.name },
        { label: 'Age', value: player.age },
        { label: 'Position', value: player.position },
        { label: 'Market Value', value: player.marketValue },
        { label: 'Team', value: player.teamName ?? 'N/A' },
      ]}
    />
  )
}
