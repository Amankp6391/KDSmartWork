using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.ViewModels;

public class TechnologiesPageViewModel
{
    public List<TechnologyItem> Technologies { get; set; } = new();
    public List<string> Categories { get; set; } = new();
}
