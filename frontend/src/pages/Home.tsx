import React from 'react'
import { Link } from 'react-router-dom'

export const Home: React.FC = () => {
  return (
    <div className="page">
      <h2>Tuna Soccer League</h2>
      <p>
        Welcome to the league portal. Browse teams, players, matches, and goals, or sign in to manage league data.
      </p>
      <div className="button-row">
        <Link to="/league">View League Dashboard</Link>
      </div>
    </div>
  )
}
