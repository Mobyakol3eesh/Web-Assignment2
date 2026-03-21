
using System.ComponentModel.DataAnnotations.Schema;
public class PlayerStats
{

    public int Id { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int Appearances { get; set; }
    [ForeignKey("Player")]
    public int PlayerId { get; set; }
    [ForeignKey("Match")]
    public int MatchId { get; set; }

    public required Player Player { get; set; }
    public required Match Match { get; set; }
}