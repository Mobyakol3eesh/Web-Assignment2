
public class CoachReadDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int Age { get; set; }

    public int ExperienceYrs { get; set; }
    public string TeamName { get; set; } = string.Empty;
    
}