using System.ComponentModel.DataAnnotations;

namespace TeamworkManagamentProjects.Models;

public class TechnologyItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty; // Backend, Database, Frontend, Architecture

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public List<string> BestSuitedFor { get; set; } = new();

    public int ProficiencyPercentage { get; set; } = 90;

    [StringLength(50)]
    public string IconBadgeClass { get; set; } = "bi-layers";

    public int DisplayOrder { get; set; }
}
