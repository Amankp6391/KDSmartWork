using System.ComponentModel.DataAnnotations;

namespace TeamworkManagamentProjects.Models;

public class ServiceOffering
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string DetailedDescription { get; set; } = string.Empty;

    public List<string> Benefits { get; set; } = new();

    public List<string> TechnologiesUsed { get; set; } = new();

    [StringLength(50)]
    public string IconClass { get; set; } = "bi-code-slash";

    public int DisplayOrder { get; set; }

    public bool IsFeaturedOnHome { get; set; } = true;
}
