using Microsoft.EntityFrameworkCore;

public class TunaLeagueContext : DbContext {
    public TunaLeagueContext(DbContextOptions<TunaLeagueContext> options) : base(options) { }
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }

    public DbSet<Coach> Coaches { get; set; }

    public DbSet<Match> Matches { get; set; }
    public DbSet<Goal> Goals { get; set; }
    
    public DbSet<PlayerStats> PlayerStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Goal>()
            .HasOne(g => g.Player)
            .WithMany(p => p.Goals)
            .HasForeignKey(g => g.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Goal>()
            .HasOne(g => g.Match)
            .WithMany(m => m.Goals)
            .HasForeignKey(g => g.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Goal>()
            .HasOne(g => g.Team)
            .WithMany(t => t.Goals)
            .HasForeignKey(g => g.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    
}