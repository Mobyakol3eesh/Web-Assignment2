

public class Player
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int Age { get; set; }
    public int MarketValue { get; set; }
    
    public String Position { get; set; } = string.Empty;

    public int JerseyNumber { get; set; }
    

    public PlayerStats? PlayerStats { get; set; }
    public IEnumerable<Goal> Goals { get; set; } = new List<Goal>();
    public required int TeamId { get; set; }
    public  Team? Team { get; set; }


    



}

