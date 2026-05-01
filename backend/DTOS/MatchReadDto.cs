public class MatchReadDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;

    public string HomeTeamName { get; set; } = string.Empty;
    public int HomeTeamScore { get; set; }
    public int HomeTeamGoals { get; set; }

    public string AwayTeamName { get; set; } = string.Empty;
    public int AwayTeamScore { get; set; }
    public int AwayTeamGoals { get; set; }
}