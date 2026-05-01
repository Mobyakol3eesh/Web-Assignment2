

using System.ComponentModel.DataAnnotations.Schema;

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? Points { get; set; }

    public Coach? Coach { get; set; }
    public IEnumerable<Player> Players { get; set; } = new List<Player>();
    public IEnumerable<Goal> Goals { get; set; } = new List<Goal>();

    

    public IEnumerable<Match> Matches { get; set; } = new List<Match>();


    

}