using System.ComponentModel.DataAnnotations;

namespace TeamworkManagamentProjects.ViewModels;

public class ContactEnquiryViewModel
{
    [Required(ErrorMessage = "Please enter your full name.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(120, ErrorMessage = "Company name cannot exceed 120 characters.")]
    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide your phone or WhatsApp number.")]
    [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters.")]
    [Display(Name = "Phone / WhatsApp")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select the service you require.")]
    [Display(Name = "Service Required")]
    public string ServiceRequired { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select the engagement model.")]
    [Display(Name = "Engagement Type")]
    public string EngagementType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please describe your project or requirement.")]
    [StringLength(3000, MinimumLength = 10, ErrorMessage = "Please provide at least 10 characters describing your project.")]
    [Display(Name = "Project Description / Requirement")]
    public string ProjectDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select your preferred contact method.")]
    [Display(Name = "Preferred Contact Method")]
    public string PreferredContactMethod { get; set; } = "Email";

    public bool IsSuccess { get; set; }
}
