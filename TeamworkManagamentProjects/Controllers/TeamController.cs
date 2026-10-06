using Microsoft.AspNetCore.Mvc;
using TeamworkManagamentProjects.Services;
using TeamworkManagamentProjects.ViewModels;

namespace TeamworkManagamentProjects.Controllers;

[Route("our-team")]
[Route("team")]
public class TeamController : Controller
{
    private readonly IContentService _contentService;

    public TeamController(IContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var developers = await _contentService.GetTeamMembersAsync();
        var model = new TeamPageViewModel
        {
            Developers = developers
        };
        return View(model);
    }
}
