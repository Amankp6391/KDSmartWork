using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels;

namespace TeamworkManagamentProjects.Controllers;

public class HomeController : Controller
{
    private readonly IContentService _contentService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IContentService contentService, ILogger<HomeController> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            Services = await _contentService.GetFeaturedServicesAsync(),
            TeamMembers = await _contentService.GetTeamMembersAsync(),
            Technologies = await _contentService.GetTechnologiesAsync(),
            FeaturedProjects = await _contentService.GetPortfolioProjectsAsync(),
            QuickContact = new ContactEnquiryViewModel()
        };

        return View(model);
    }

    [HttpGet]
    [Route("about")]
    [Route("about-us")]
    public async Task<IActionResult> About()
    {
        var team = await _contentService.GetTeamMembersAsync();
        return View(team);
    }

    [HttpGet]
    [Route("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
