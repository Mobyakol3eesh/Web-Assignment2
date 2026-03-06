using Microsoft.EntityFrameworkCore;

public class FootballContext : DbContext {
    public FootballContext(DbContextOptions<FootballContext> options) : base(options) { }
    public DbSet<Player> players { get; set; }
    public DbSet<Team> teams { get; set; }

    public DbSet<Coach> coaches { get; set; }
}