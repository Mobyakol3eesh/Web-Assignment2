


public class Match
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    
    public String Location { get; set; } = string.Empty;


    public int HomeTeamId { get; set; }
    public  Team? HomeTeam { get; set; }

    public int AwayTeamId { get; set; }
    public  Team? AwayTeam { get; set; }

    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }

    public IEnumerable<Goal> Goals { get; set; } = new List<Goal>();
    public IEnumerable<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>();

}