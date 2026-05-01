public class TeamReadDto
{
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Points { get; set; }
    public int Goals { get; set; }


    public List<PlayerReadDto> Players { get; set; } = new List<PlayerReadDto>();

    public String CoachName { get; set; } = string.Empty;

    

    
    
    
}
