using System.ComponentModel.DataAnnotations;

namespace TeamworkManagamentProjects.ViewModels;

public class HireDeveloperViewModel
{
    [Required(ErrorMessage = "Please enter your full name.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [Display(Name = "Your Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your company / startup name.")]
    [StringLength(120, ErrorMessage = "Company name cannot exceed 120 characters.")]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your business email.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(150)]
    [Display(Name = "Work Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your phone number.")]
    [StringLength(30)]
    [Display(Name = "Phone / WhatsApp Number")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please choose developer engagement type.")]
    [Display(Name = "Developer Engagement")]
    public string EngagementType { get; set; } = "Full-Time";

    [Required(ErrorMessage = "Please select primary technology required.")]
    [Display(Name = "Primary Technology")]
    public string Technology { get; set; } = "ASP.NET Core";

    [Required(ErrorMessage = "Please select experience level.")]
    [Display(Name = "Experience Required")]
    public string ExperienceRequired { get; set; } = "4–5 Years";

    [Required(ErrorMessage = "Please provide details about your project or tasks.")]
    [StringLength(3000, MinimumLength = 15, ErrorMessage = "Please provide at least 15 characters of project description.")]
    [Display(Name = "Project Description / Scope of Work")]
    public string ProjectDescription { get; set; } = string.Empty;

    [Display(Name = "Expected Start Date")]
    [StringLength(50)]
    public string? ExpectedStartDate { get; set; } = "Immediately";

    [Display(Name = "Additional Requirements / Tools")]
    [StringLength(1500)]
    public string? AdditionalRequirements { get; set; }

    [Display(Name = "Preferred Contact Method")]
    public string PreferredContactMethod { get; set; } = "Email";

    public bool IsSuccess { get; set; }
}
