using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Models.Enums;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels.Admin;

namespace TeamworkManagamentProjects.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    private readonly IEnquiryService _enquiryService;
    private readonly IAuthService _authService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IEnquiryService enquiryService, IAuthService authService, ILogger<AdminController> logger)
    {
        _enquiryService = enquiryService;
        _authService = authService;
        _logger = logger;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction(nameof(Index));
        }

        var model = new AdminLoginViewModel { ReturnUrl = returnUrl };
        return View(model);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AdminLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _authService.ValidateAdminCredentialsAsync(model.Username, model.Password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Administrator")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("logout")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> Index(string? searchTerm, string? service, string? engagement, EnquiryStatus? status)
    {
        var enquiries = await _enquiryService.GetEnquiriesAsync(searchTerm, service, engagement, status);
        var (total, newCount, inDiscussion, converted, closed) = await _enquiryService.GetEnquiryStatsAsync();
        var availableServices = await _enquiryService.GetAvailableServicesAsync();
        var availableEngagements = await _enquiryService.GetAvailableEngagementsAsync();

        var model = new AdminDashboardViewModel
        {
            Enquiries = enquiries,
            TotalEnquiries = total,
            NewEnquiriesCount = newCount,
            InDiscussionCount = inDiscussion,
            ConvertedCount = converted,
            ClosedCount = closed,
            SearchTerm = searchTerm,
            SelectedService = service,
            SelectedEngagement = engagement,
            SelectedStatus = status,
            AvailableServices = availableServices,
            AvailableEngagements = availableEngagements
        };

        return View(model);
    }

    [HttpGet("details/{id:int}")]
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var enquiry = await _enquiryService.GetEnquiryByIdAsync(id);
        if (enquiry == null)
        {
            return NotFound();
        }

        var model = new AdminEnquiryDetailsViewModel
        {
            Enquiry = enquiry
        };

        return View(model);
    }

    [HttpPost("update-status")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(UpdateStatusViewModel model)
    {
        var success = await _enquiryService.UpdateEnquiryStatusAsync(model.Id, model.Status, model.AdminNotes);
        if (success)
        {
            TempData["AdminMessage"] = $"Enquiry #{model.Id} status updated to '{model.Status}' successfully.";
        }
        else
        {
            TempData["AdminError"] = $"Failed to update enquiry #{model.Id}.";
        }

        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    [HttpPost("delete/{id:int}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _enquiryService.DeleteEnquiryAsync(id);
        if (success)
        {
            TempData["AdminMessage"] = $"Enquiry #{id} deleted successfully.";
        }
        else
        {
            TempData["AdminError"] = $"Failed to delete enquiry #{id}.";
        }

        return RedirectToAction(nameof(Index));
    }
}
