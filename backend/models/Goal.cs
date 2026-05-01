public class Goal
{
    public int Id { get; set; }
    public DateTime TimeScored { get; set; }
    public string ScorerName { get; set; } = string.Empty;


    public int PlayerId { get; set; }
    public Player? Player { get; set; }

    public int MatchId { get; set; }
    public Match? Match { get; set; }

    public int TeamId { get; set; }
    public Team? Team { get; set; }
}