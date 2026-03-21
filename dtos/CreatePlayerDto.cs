using System.ComponentModel.DataAnnotations;

public class CreatePlayerDto
{
   
   
    public string Name { get; set; } = string.Empty;
    public int MarketValue { get; set; }

    public int Age { get; set; }
    
    [Required]
    public int TeamId { get; set; }
}