public class GoalReadDto
{
    public int Id { get; set; }
    public DateTime TimeScored { get; set; }
    public string ScorerName { get; set; } = string.Empty;
    public int PlayerId { get; set; }
    public int MatchId { get; set; }
    public string MatchLocation { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
}