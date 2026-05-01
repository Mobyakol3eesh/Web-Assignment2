using System.ComponentModel.DataAnnotations.Schema;

public class Coach
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int Age { get; set; }

    public int ExperienceYrs { get; set; }
     [ForeignKey(nameof(Team))]
    public int? TeamId { get; set; }
   
    public Team? Team { get; set; }
}