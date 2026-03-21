using Microsoft.EntityFrameworkCore;

public class TunaLeagueContext : DbContext {
    public TunaLeagueContext(DbContextOptions<TunaLeagueContext> options) : base(options) { }
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }

    public DbSet<Coach> Coaches { get; set; }

    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchTeam> MatchTeams { get; set; }
    
    public DbSet<PlayerStats> PlayerStats { get; set; } 
    
    }