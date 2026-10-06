using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels;

namespace TeamworkManagamentProjects.Controllers;

[Route("technologies")]
public class TechnologiesController : Controller
{
    private readonly IContentService _contentService;

    public TechnologiesController(IContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var techs = await _contentService.GetTechnologiesAsync();
        var categories = techs.Select(t => t.Category).Distinct().ToList();

        var model = new TechnologiesPageViewModel
        {
            Technologies = techs,
            Categories = categories
        };

        return View(model);
    }
}
