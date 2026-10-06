using System.ComponentModel.DataAnnotations;
using TeamworkManagamentProjects.Models.Enums;

namespace TeamworkManagamentProjects.Models;

public class ContactEnquiry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(120)]
    public string? CompanyName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string ServiceRequired { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string EngagementType { get; set; } = string.Empty;

    [StringLength(50)]
    public string? ExperienceRequired { get; set; }

    [StringLength(100)]
    public string? Technology { get; set; }

    [Required]
    [StringLength(3000)]
    public string ProjectDescription { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string PreferredContactMethod { get; set; } = "Email";

    [StringLength(50)]
    public string? ExpectedStartDate { get; set; }

    [StringLength(1500)]
    public string? AdditionalRequirements { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }

    public EnquiryStatus Status { get; set; } = EnquiryStatus.New;

    [StringLength(2000)]
    public string? AdminNotes { get; set; }
}
