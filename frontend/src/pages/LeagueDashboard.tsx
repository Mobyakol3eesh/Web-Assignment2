import React, { useEffect, useMemo, useState } from 'react'
import api from '../api'

type Team = { id: number; name: string; points?: number }

type Player = {
  id: number
  name: string
  position: string
  teamName?: string
}

type Match = {
  id: number
  date: string
  location: string
  homeTeamName: string
  awayTeamName: string
  homeTeamScore: number
  awayTeamScore: number
}

type MvpMap = Record<number, Player | null>

export const LeagueDashboard: React.FC = () => {
  const [teams, setTeams] = useState<Team[]>([])
  const [mvps, setMvps] = useState<MvpMap>({})
  const [matches, setMatches] = useState<Match[]>([])
  const [matchTab, setMatchTab] = useState<'upcoming' | 'today' | 'played'>('upcoming')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const [teamsRes, matchesRes] = await Promise.all([
        api.get('/teams'),
        api.get('/matches'),
      ])
      const teamList = (teamsRes.data || []) as Team[]
      setMatches((matchesRes.data || []) as Match[])
      setTeams(teamList)

      const mvpResults = await Promise.all(
        teamList.map(async (team) => {
          try {
            const res = await api.get(`/teams/MVP/${team.id}`)
            return [team.id, res.data as Player] as const
          } catch {
            return [team.id, null] as const
          }
        })
      )

      const mvpMap: MvpMap = {}
      mvpResults.forEach(([teamId, player]) => {
        mvpMap[teamId] = player
      })
      setMvps(mvpMap)
    } catch (err) {
      setError('Unable to load league data.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [])

  const standings = useMemo(() => {
    return [...teams].sort((a, b) => (b.points ?? 0) - (a.points ?? 0))
  }, [teams])

  const topStandings = standings
  const topMvps = standings

  const toLocalDateKey = (value: Date) => {
    const year = value.getFullYear()
    const month = String(value.getMonth() + 1).padStart(2, '0')
    const day = String(value.getDate()).padStart(2, '0')
    return `${year}-${month}-${day}`
  }

  const todayKey = toLocalDateKey(new Date())

  const { upcoming, today, played } = useMemo(() => {
    const upcoming: Match[] = []
    const today: Match[] = []
    const played: Match[] = []

    matches.forEach((match) => {
      const matchKey = toLocalDateKey(new Date(match.date))
      if (matchKey === todayKey) {
        today.push(match)
      } else if (matchKey > todayKey) {
        upcoming.push(match)
      } else {
        played.push(match)
      }
    })

    return {
      upcoming: upcoming.sort((a, b) => a.date.localeCompare(b.date)),
      today,
      played: played.sort((a, b) => b.date.localeCompare(a.date)),
    }
  }, [matches, todayKey])

  const activeMatches = useMemo(() => {
    if (matchTab === 'today') return today
    if (matchTab === 'played') return played
    return upcoming
  }, [matchTab, upcoming, today, played])

  if (loading) return <div className="page">Loading league...</div>
  if (error) return <div className="page error">{error}</div>

  return (
    <div className="page">
      <h2>League Dashboard</h2>
      <div className="section-row">
        <section className="card">
          <h3>Standings</h3>
          <div className="scrollable">
            <table className="standings">
              <thead>
                <tr>
                  <th>#</th>
                  <th>Team</th>
                  <th>Points</th>
                </tr>
              </thead>
              <tbody>
                {topStandings.map((team, index) => (
                  <tr key={team.id}>
                    <td>{index + 1}</td>
                    <td>{team.name}</td>
                    <td>{team.points ?? 0}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        <section className="card">
          <h3>Team MVPs</h3>
          <div className="scrollable">
            <ul className="mvp-list">
              {topMvps.map((team) => (
                <li key={team.id}>
                  <div className="mvp-team">{team.name}</div>
                  <div className="mvp-player">
                    {mvps[team.id]?.name ?? 'No MVP yet'}
                    {mvps[team.id]?.position ? ` · ${mvps[team.id]?.position}` : ''}
                  </div>
                </li>
              ))}
            </ul>
          </div>
        </section>
      </div>
      <div className="section-row">
        <section className="card">
          <div className="card-header">
            <h3>Matches</h3>
            <div className="tab-group">
              <button
                type="button"
                className={matchTab === 'upcoming' ? 'tab active' : 'tab'}
                onClick={() => setMatchTab('upcoming')}
              >
                Upcoming
              </button>
              <button
                type="button"
                className={matchTab === 'today' ? 'tab active' : 'tab'}
                onClick={() => setMatchTab('today')}
              >
                Today
              </button>
              <button
                type="button"
                className={matchTab === 'played' ? 'tab active' : 'tab'}
                onClick={() => setMatchTab('played')}
              >
                Played
              </button>
            </div>
          </div>
          <div className="scrollable">
            <ul className="match-list">
              {activeMatches.map((match) => (
                <li key={match.id}>
                  <div className="match-teams">
                    {match.homeTeamName}
                    {matchTab === 'played'
                      ? ` ${match.homeTeamScore} - ${match.awayTeamScore} ${match.awayTeamName}`
                      : ` vs ${match.awayTeamName}`}
                  </div>
                  <div className="match-meta">
                    {matchTab === 'today'
                      ? match.location
                      : `${new Date(match.date).toLocaleDateString()} · ${match.location}`}
                  </div>
                </li>
              ))}
              {activeMatches.length === 0 && (
                <li className="muted">
                  {matchTab === 'today'
                    ? 'No matches today'
                    : matchTab === 'played'
                      ? 'No matches played'
                      : 'No upcoming matches'}
                </li>
              )}
            </ul>
          </div>
        </section>
      </div>
    </div>
  )
}
