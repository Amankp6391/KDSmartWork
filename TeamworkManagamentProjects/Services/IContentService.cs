using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.Services;

public interface IContentService
{
    Task<List<DeveloperProfile>> GetTeamMembersAsync();
    Task<List<ServiceOffering>> GetAllServicesAsync();
    Task<List<ServiceOffering>> GetFeaturedServicesAsync();
    Task<ServiceOffering?> GetServiceBySlugAsync(string slug);
    Task<List<TechnologyItem>> GetTechnologiesAsync();
    Task<List<PortfolioProject>> GetPortfolioProjectsAsync();
}
