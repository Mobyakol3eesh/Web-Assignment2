import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import api from '../services/api'

type TeamForm = {
  name: string
}

const emptyForm: TeamForm = {
  name: '',
}

export const CreateTeam: React.FC = () => {
  const [form, setForm] = useState<TeamForm>({ ...emptyForm })
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const navigate = useNavigate()

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError(null)
    setSuccess(null)
    try {
      await api.post('/teams', form)
      setSuccess('Team created successfully.')
      setForm({ ...emptyForm })
    } catch (err) {
      setError('Unable to create team. Check the form values and try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page">
      <div className="card-header">
        <h2>Create Team</h2>
        <Link to="/admin/teams">Back to Teams</Link>
      </div>
      <form onSubmit={submit} className="stack">
        <label>Team name</label>
        <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} placeholder="Team name" />
        <div className="button-row">
          <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Create Team'}</button>
          <button type="button" onClick={() => navigate('/admin/teams')} disabled={loading}>Cancel</button>
        </div>
        {success && <p className="success">{success}</p>}
        {error && <p className="error">{error}</p>}
      </form>
    </div>
  )
}
