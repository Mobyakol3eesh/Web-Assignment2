public class PlayerStatsReadDto
{
    public int Id { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int ShotsOnTarget { get; set; }
    public int Touches { get; set; }
    public int PassesCompleted { get; set; }
    public float Score { get; set; }
    public int PlayerId { get; set; }
    public int MatchId { get; set; }

    public string PlayerName { get; set; } = string.Empty;
    public DateTime MatchDate { get; set; }
    public string MatchLocation { get; set; } = string.Empty;
}