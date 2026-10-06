using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels;

namespace TeamworkManagamentProjects.Controllers;

[Route("contact")]
[Route("contact-us")]
public class ContactController : Controller
{
    private readonly IEnquiryService _enquiryService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IEnquiryService enquiryService, ILogger<ContactController> logger)
    {
        _enquiryService = enquiryService;
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index(string? service, string? engagement)
    {
        var model = new ContactEnquiryViewModel();
        if (!string.IsNullOrWhiteSpace(service))
        {
            model.ServiceRequired = service;
        }
        if (!string.IsNullOrWhiteSpace(engagement))
        {
            model.EngagementType = engagement;
        }
        return View(model);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactEnquiryViewModel model)
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
                ServiceRequired = model.ServiceRequired,
                EngagementType = model.EngagementType,
                ProjectDescription = model.ProjectDescription,
                PreferredContactMethod = model.PreferredContactMethod
            };

            await _enquiryService.CreateEnquiryAsync(enquiry);

            TempData["SuccessMessage"] = "Thank you for reaching out! We have received your project details and an experienced developer from our team will respond within 24 hours.";
            return RedirectToAction(nameof(Index), new { submitted = "true" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving contact enquiry for {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred while saving your enquiry. Please try again or reach out to us directly.");
            return View(model);
        }
    }
}
