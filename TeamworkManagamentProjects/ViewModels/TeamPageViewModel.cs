using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.ViewModels;

public class TeamPageViewModel
{
    public List<DeveloperProfile> Developers { get; set; } = new();
    public int TotalDevelopers => Developers.Count;
    public string AverageExperience => "4–8 Years";
}
