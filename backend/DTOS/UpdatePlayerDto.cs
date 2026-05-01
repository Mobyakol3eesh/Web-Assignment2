using System.ComponentModel.DataAnnotations;

public class UpdatePlayerDto
{
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters."), Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "MarketValue must be a non-negative integer.")]
    public int MarketValue { get; set; }

    [Range(16, 50, ErrorMessage = "Age must be between 16 and 50.")]
    public int Age { get; set; }

    [MaxLength(50, ErrorMessage = "Position cannot exceed 50 characters."), Required(ErrorMessage = "Position is required.")]
    public string Position { get; set; } = string.Empty;

    [Required, Range(1, int.MaxValue, ErrorMessage = "TeamId must be a positive integer.")]
    public int TeamId { get; set; }
}