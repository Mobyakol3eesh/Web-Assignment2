
using System.ComponentModel.DataAnnotations;

public class PlayerReadDto
{
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MarketValue { get; set; }
    
    public int Age { get; set; }

    public String Position { get; set; } = string.Empty;
    public  String TeamName { get; set; } = string.Empty;

}


