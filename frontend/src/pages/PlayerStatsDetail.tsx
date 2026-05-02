import React, { useEffect, useMemo, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import api from '../services/api'
import type { AxiosError } from 'axios'

type Player = { id: number; name: string; teamName: string }

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

type Props = { isAdmin?: boolean }

export const PlayerStatsDetail: React.FC<Props> = ({ isAdmin = false }) => {
  const params = useParams()
  const playerId = Number(params.playerId)
  const [player, setPlayer] = useState<Player | null>(null)
  const [stats, setStats] = useState<Stats[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const [playersRes, statsRes] = await Promise.all([
        api.get('/players'),
        api.get('/players/player-stats'),
      ])
      const players = (playersRes.data || []) as Player[]
      setPlayer(players.find((p) => p.id === playerId) ?? null)

      const allStats = (statsRes.data || []) as Stats[]
      setStats(allStats.filter((s) => s.playerId === playerId))
    } catch (err) {
      const axiosErr = err as AxiosError
      const msg = axiosErr.response?.data ?? ''
      setError('Unable to load player stats. ' + msg)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [playerId])

  const title = useMemo(() => {
    if (!player) return 'Player Stats'
    return `${player.name} · ${player.teamName}`
  }, [player])

  const backLink = isAdmin ? '/admin/players' : '/league/players'

  if (loading) return <div className="page">Loading player stats...</div>
  if (error) return <div className="page error">{error}</div>

  return (
    <div className="page">
      <div className="card-header">
        <h2>{title}</h2>
        <Link to={backLink}>Back to Players</Link>
      </div>
      <ul className="scrollable-list">
        {stats.map((s) => (
          <li key={s.id} className="readonly">
            Match {s.matchId} · Goals {s.goals ?? 0} · Assists {s.assists ?? 0} · SOT {s.shotsOnTarget ?? 0} ·
            Touches {s.touches ?? 0} · Passes {s.passesCompleted ?? 0} · Score {s.score ?? 0}
          </li>
        ))}
        {stats.length === 0 && <li className="readonly">No stats available for this player.</li>}
      </ul>
    </div>
  )
}
