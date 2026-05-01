
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class PlayerStats
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

    public required Player Player { get; set; }
    public required Match Match { get; set; }
}