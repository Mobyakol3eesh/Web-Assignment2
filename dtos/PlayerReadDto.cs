
using System.ComponentModel.DataAnnotations;

public class PlayerReadDto
{
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MarketValue { get; set; }
    
    
    public  TeamReadDto TeamReadDto { get; set; }
}


