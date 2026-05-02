import React from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthProvider'

const NavBar: React.FC = () => {
  const auth = useAuth()
  const navigate = useNavigate()

  const handleLogout = async () => {
    await auth.logout()
    navigate('/login')
  }

  return (
    <nav className="nav">
      <div className="nav-left">
        {auth.isAuthenticated && (
          <Link to="/">Home</Link>
        )}
        {auth.isAuthenticated && auth.role !== 'Admin' && (
          <>
            <Link to="/league">League</Link>
            <Link to="/league/teams">Teams</Link>
              <Link to="/league/players">Players</Link>
            <Link to="/league/matches">Matches</Link>
            <Link to="/league/goals">Goals</Link>
            <Link to="/league/coaches">Coaches</Link>
          </>
        )}
        {auth.role === 'Admin' && (
          <>
            <Link to="/admin/teams">Teams</Link>
            <Link to="/admin/players">Players</Link>
            <Link to="/admin/matches">Matches</Link>
            <Link to="/admin/goals">Goals</Link>
            <Link to="/admin/coaches">Coaches</Link>
            <Link to="/admin/player-stats">Player Stats</Link>
          </>
        )}
      </div>
      <div className="nav-right">
        {auth.loading ? (
          <span>Checking...</span>
        ) : auth.isAuthenticated ? (
          <>
            <span>{auth.role ?? 'User'}</span>
            <button onClick={handleLogout}>Logout</button>
          </>
        ) : (
          <>
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
          </>
        )}
      </div>
    </nav>
  )
}

export default NavBar
