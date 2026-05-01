
using System.ComponentModel.DataAnnotations;

public class CreateTeamDto
{
    [Required, MaxLength(100, ErrorMessage = "Name is required and cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    

    
    
}