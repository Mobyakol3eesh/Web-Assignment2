using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CreateMatchDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required, MaxLength(150, ErrorMessage = "Location cannot exceed 150 characters.")]
    
   
    public string Location { get; set; } = string.Empty;

    [Required, Range(1, int.MaxValue, ErrorMessage = "HomeTeamId must be a positive integer.")]
    public int HomeTeamId { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "AwayTeamId must be a positive integer.")]
    public int AwayTeamId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "HomeTeamScore must be non-negative.")]
    public int HomeTeamScore { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "AwayTeamScore must be non-negative.")]
    public int AwayTeamScore { get; set; }
}