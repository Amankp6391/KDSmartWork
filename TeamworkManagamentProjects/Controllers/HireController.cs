using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels;

namespace TeamworkManagamentProjects.Controllers;

[Route("hire-a-developer")]
[Route("hire")]
public class HireController : Controller
{
    private readonly IEnquiryService _enquiryService;
    private readonly ILogger<HireController> _logger;

    public HireController(IEnquiryService enquiryService, ILogger<HireController> logger)
    {
        _enquiryService = enquiryService;
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index(string? engagement, string? technology)
    {
        var model = new HireDeveloperViewModel();
        if (!string.IsNullOrWhiteSpace(engagement))
        {
            model.EngagementType = engagement;
        }
        if (!string.IsNullOrWhiteSpace(technology))
        {
            model.Technology = technology;
        }
        return View(model);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(HireDeveloperViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var enquiry = new ContactEnquiry
            {
                Name = model.Name,
                CompanyName = model.CompanyName,
                Email = model.Email,
                Phone = model.Phone,
                ServiceRequired = $"Hire {model.EngagementType} Developer ({model.Technology})",
                EngagementType = model.EngagementType,
                Technology = model.Technology,
                ExperienceRequired = model.ExperienceRequired,
                ProjectDescription = model.ProjectDescription,
                ExpectedStartDate = model.ExpectedStartDate,
                AdditionalRequirements = model.AdditionalRequirements,
                PreferredContactMethod = model.PreferredContactMethod
            };

            await _enquiryService.CreateEnquiryAsync(enquiry);

            TempData["SuccessMessage"] = "Thank you! Your hiring requirement has been submitted. Our team will review your specifications and contact you shortly with suitable developer profiles.";
            return RedirectToAction(nameof(Index), new { submitted = "true" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing developer hiring form for {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred while submitting your requirement. Please try again or contact us directly via email.");
            return View(model);
        }
    }
}
