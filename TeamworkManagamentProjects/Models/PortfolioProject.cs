using System.ComponentModel.DataAnnotations;

namespace TeamworkManagamentProjects.Models;

public class PortfolioProject
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string ProjectName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string ProjectType { get; set; } = string.Empty; // ASP.NET Core, ASP.NET MVC, Web Forms Migration, REST API

    [Required]
    public List<string> Technologies { get; set; } = new();

    [Required]
    [StringLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string DetailedOverview { get; set; } = string.Empty;

    public List<string> Features { get; set; } = new();

    [Required]
    [StringLength(100)]
    public string ProjectStatus { get; set; } = "Architecture Showcase / Reference Case";

    public int DisplayOrder { get; set; }
}
