using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.ViewModels;

public class HomeViewModel
{
    public List<ServiceOffering> Services { get; set; } = new();
    public List<DeveloperProfile> TeamMembers { get; set; } = new();
    public List<TechnologyItem> Technologies { get; set; } = new();
    public List<PortfolioProject> FeaturedProjects { get; set; } = new();
    public ContactEnquiryViewModel QuickContact { get; set; } = new();
}
