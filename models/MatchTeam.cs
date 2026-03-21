

using System.ComponentModel.DataAnnotations.Schema;

public class MatchTeam
{
    public int Id { get; set; }

    [ForeignKey("Match")]
    public int MatchId { get; set; }
    public required Match Match { get; set; }
    
    [ForeignKey("Team")]
    public int TeamId { get; set; }
    public required Team Team { get; set; }

    public int Score { get; set; }

}