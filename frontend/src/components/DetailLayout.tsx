import React from 'react'
import { Link } from 'react-router-dom'

type DetailItem = {
  label: string
  value: React.ReactNode
}

type DetailLayoutProps = {
  title: string
  backLink: string
  backLabel: string
  items: DetailItem[]
}

export const DetailLayout: React.FC<DetailLayoutProps> = ({ title, backLink, backLabel, items }) => {
  return (
    <div className="page">
      <div className="card-header">
        <h2>{title}</h2>
        <Link to={backLink}>{backLabel}</Link>
      </div>
      <ul className="scrollable-list">
        {items.map((item) => (
          <li key={item.label} className="readonly">
            {item.label}: {item.value}
          </li>
        ))}
      </ul>
    </div>
  )
}
