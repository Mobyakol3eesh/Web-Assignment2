using System.ComponentModel.DataAnnotations;

public class CreateGoalDto
{
    [Required, Range(1, int.MaxValue, ErrorMessage = "PlayerId must be a positive integer.")]
    public int PlayerId { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "MatchId must be a positive integer.")]
    public int MatchId { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "TeamId must be a positive integer.")]
    public int TeamId { get; set; }

    public DateTime TimeScored { get; set; } = DateTime.UtcNow;
}