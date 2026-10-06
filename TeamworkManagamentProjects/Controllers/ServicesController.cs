using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels;

namespace TeamworkManagamentProjects.Controllers;

[Route("services")]
public class ServicesController : Controller
{
    private readonly IContentService _contentService;

    public ServicesController(IContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? highlight)
    {
        var services = await _contentService.GetAllServicesAsync();
        var model = new ServicesPageViewModel
        {
            Services = services,
            HighlightedSlug = highlight
        };
        return View(model);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var service = await _contentService.GetServiceBySlugAsync(slug);
        if (service == null)
        {
            return NotFound();
        }
        return View(service);
    }
}
