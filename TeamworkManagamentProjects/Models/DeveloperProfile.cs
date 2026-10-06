using System.ComponentModel.DataAnnotations;

namespace TeamworkManagamentProjects.Models;

public class DeveloperProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Role { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Experience { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    [Required]
    public List<string> PrimarySkills { get; set; } = new();

    [Required]
    [StringLength(100)]
    public string Availability { get; set; } = "Full-Time / Part-Time";

    [Required]
    [StringLength(500)]
    public string Summary { get; set; } = string.Empty;

    public List<string> KeySpecializations { get; set; } = new();

    public int DisplayOrder { get; set; }

    [StringLength(250)]
    public string? ImageUrl { get; set; }
}
