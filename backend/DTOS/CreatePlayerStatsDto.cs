using System.ComponentModel.DataAnnotations;

public class CreatePlayerStatsDto
{
    [Range(0, int.MaxValue, ErrorMessage = "Goals must be a non-negative integer.")]
    public int Goals { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Assists must be a non-negative integer.")]
    public int Assists { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ShotsOnTarget must be a non-negative integer.")]
    public int ShotsOnTarget { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Touches must be a non-negative integer.")]
    public int Touches { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "PassesCompleted must be a non-negative integer.")]
    public int PassesCompleted { get; set; }

    [Range(0, 10, ErrorMessage = "Score must be between 0 and 10.")]
    public float Score { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "PlayerId must be a positive integer.")]
    public int PlayerId { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "MatchId must be a positive integer.")]
    public int MatchId { get; set; }
}