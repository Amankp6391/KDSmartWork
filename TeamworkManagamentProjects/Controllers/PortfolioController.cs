using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels;

namespace TeamworkManagamentProjects.Controllers;

[Route("portfolio")]
[Route("projects")]
public class PortfolioController : Controller
{
    private readonly IContentService _contentService;

    public PortfolioController(IContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var projects = await _contentService.GetPortfolioProjectsAsync();
        var categories = projects.Select(p => p.ProjectType).Distinct().ToList();

        var model = new PortfolioPageViewModel
        {
            Projects = projects,
            FilterCategories = categories
        };

        return View(model);
    }
}
