using Microsoft.EntityFrameworkCore;
using TeamworkManagamentProjects.Data;
using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Models.Enums;

namespace TeamworkManagamentProjects.Services;

public class EnquiryService : IEnquiryService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EnquiryService> _logger;

    public EnquiryService(ApplicationDbContext context, ILogger<EnquiryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ContactEnquiry> CreateEnquiryAsync(ContactEnquiry enquiry)
    {
        try
        {
            enquiry.CreatedDate = DateTime.UtcNow;
            enquiry.Status = EnquiryStatus.New;
            _context.ContactEnquiries.Add(enquiry);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New enquiry received from {Email} for {Service}", enquiry.Email, enquiry.ServiceRequired);
            return enquiry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create enquiry for {Email}", enquiry.Email);
            throw;
        }
    }

    public async Task<List<ContactEnquiry>> GetEnquiriesAsync(string? searchTerm, string? service, string? engagement, EnquiryStatus? status)
    {
        var query = _context.ContactEnquiries.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(e =>
                e.Name.ToLower().Contains(term) ||
                (e.CompanyName != null && e.CompanyName.ToLower().Contains(term)) ||
                e.Email.ToLower().Contains(term) ||
                e.ProjectDescription.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(service) && service != "All")
        {
            query = query.Where(e => e.ServiceRequired == service);
        }

        if (!string.IsNullOrWhiteSpace(engagement) && engagement != "All")
        {
            query = query.Where(e => e.EngagementType == engagement);
        }

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        return await query.OrderByDescending(e => e.CreatedDate).ToListAsync();
    }

    public async Task<ContactEnquiry?> GetEnquiryByIdAsync(int id)
    {
        return await _context.ContactEnquiries.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> UpdateEnquiryStatusAsync(int id, EnquiryStatus status, string? adminNotes)
    {
        var enquiry = await _context.ContactEnquiries.FirstOrDefaultAsync(e => e.Id == id);
        if (enquiry == null) return false;

        enquiry.Status = status;
        enquiry.AdminNotes = adminNotes;
        enquiry.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Enquiry {Id} updated to status {Status}", id, status);
        return true;
    }

    public async Task<bool> DeleteEnquiryAsync(int id)
    {
        var enquiry = await _context.ContactEnquiries.FirstOrDefaultAsync(e => e.Id == id);
        if (enquiry == null) return false;

        _context.ContactEnquiries.Remove(enquiry);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Enquiry {Id} deleted", id);
        return true;
    }

    public async Task<(int total, int newCount, int inDiscussion, int converted, int closed)> GetEnquiryStatsAsync()
    {
        var total = await _context.ContactEnquiries.CountAsync();
        var newCount = await _context.ContactEnquiries.CountAsync(e => e.Status == EnquiryStatus.New);
        var inDiscussion = await _context.ContactEnquiries.CountAsync(e => e.Status == EnquiryStatus.InDiscussion || e.Status == EnquiryStatus.Contacted);
        var converted = await _context.ContactEnquiries.CountAsync(e => e.Status == EnquiryStatus.Converted);
        var closed = await _context.ContactEnquiries.CountAsync(e => e.Status == EnquiryStatus.Closed);

        return (total, newCount, inDiscussion, converted, closed);
    }

    public async Task<List<string>> GetAvailableServicesAsync()
    {
        var distinctServices = await _context.ContactEnquiries
            .Select(e => e.ServiceRequired)
            .Distinct()
            .ToListAsync();

        var standardServices = new List<string>
        {
            "ASP.NET Core Development",
            "ASP.NET MVC Development",
            "ASP.NET Web Forms",
            "Angular & .NET Full-Stack",
            "React & .NET Full-Stack",
            "New Web Application Development",
            "Existing Project Maintenance",
            "Bug Fixing",
            "New Module Development",
            "API Development",
            "SQL Server Development",
            "Hire Full-Time Developer",
            "Hire Part-Time Developer",
            "Dedicated Development Team"
        };

        return standardServices.Union(distinctServices).Distinct().OrderBy(s => s).ToList();
    }

    public async Task<List<string>> GetAvailableEngagementsAsync()
    {
        return new List<string>
        {
            "Full-Time",
            "Part-Time",
            "Project-Based",
            "Long-Term Maintenance",
            "Dedicated Team"
        };
    }
}
