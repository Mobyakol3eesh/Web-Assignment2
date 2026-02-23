

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? Points { get; set; }

    public required List<Player> Players { get; set; }

}