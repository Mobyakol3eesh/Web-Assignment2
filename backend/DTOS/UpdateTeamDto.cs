using System.ComponentModel.DataAnnotations;

public class UpdateTeamDto
{
    [Required, MaxLength(100, ErrorMessage = "Name is required and cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Points must be a non-negative integer.")]
    public int? Points { get; set; }
}