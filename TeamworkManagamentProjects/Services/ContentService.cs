using Microsoft.EntityFrameworkCore;
using TeamworkManagamentProjects.Data;
using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.Services;

public class ContentService : IContentService
{
    private readonly ApplicationDbContext _context;

    public ContentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeveloperProfile>> GetTeamMembersAsync()
    {
        return await _context.DeveloperProfiles
            .AsNoTracking()
            .OrderBy(d => d.DisplayOrder)
            .ToListAsync();
    }

    public async Task<List<ServiceOffering>> GetAllServicesAsync()
    {
        return await _context.ServiceOfferings
            .AsNoTracking()
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
    }

    public async Task<List<ServiceOffering>> GetFeaturedServicesAsync()
    {
        return await _context.ServiceOfferings
            .AsNoTracking()
            .Where(s => s.IsFeaturedOnHome)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
    }

    public async Task<ServiceOffering?> GetServiceBySlugAsync(string slug)
    {
        return await _context.ServiceOfferings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Slug.ToLower() == slug.ToLower());
    }

    public async Task<List<TechnologyItem>> GetTechnologiesAsync()
    {
        return await _context.TechnologyItems
            .AsNoTracking()
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
    }

    public async Task<List<PortfolioProject>> GetPortfolioProjectsAsync()
    {
        return await _context.PortfolioProjects
            .AsNoTracking()
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();
    }
}
