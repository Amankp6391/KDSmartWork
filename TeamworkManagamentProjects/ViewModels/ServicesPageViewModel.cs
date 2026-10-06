using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.ViewModels;

public class ServicesPageViewModel
{
    public List<ServiceOffering> Services { get; set; } = new();
    public string? HighlightedSlug { get; set; }
}
