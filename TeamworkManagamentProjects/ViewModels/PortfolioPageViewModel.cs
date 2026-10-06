using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.ViewModels;

public class PortfolioPageViewModel
{
    public List<PortfolioProject> Projects { get; set; } = new();
    public List<string> FilterCategories { get; set; } = new();
}
