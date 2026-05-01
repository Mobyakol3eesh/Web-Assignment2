using System.ComponentModel.DataAnnotations;

public class CreateCoachDto
{
    [Required, MaxLength(100, ErrorMessage = "Name is required and cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(18, 90, ErrorMessage = "Age must be between 18 and 90.")]
    public int Age { get; set; }

    [Range(0, 70, ErrorMessage = "Experience years must be between 0 and 70.")]
    public int ExperienceYrs { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "TeamId must be a positive integer.")]
    public int? TeamId { get; set; }
}