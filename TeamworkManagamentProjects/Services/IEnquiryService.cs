using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Models.Enums;

namespace TeamworkManagamentProjects.Services;

public interface IEnquiryService
{
    Task<ContactEnquiry> CreateEnquiryAsync(ContactEnquiry enquiry);
    Task<List<ContactEnquiry>> GetEnquiriesAsync(string? searchTerm, string? service, string? engagement, EnquiryStatus? status);
    Task<ContactEnquiry?> GetEnquiryByIdAsync(int id);
    Task<bool> UpdateEnquiryStatusAsync(int id, EnquiryStatus status, string? adminNotes);
    Task<bool> DeleteEnquiryAsync(int id);
    Task<(int total, int newCount, int inDiscussion, int converted, int closed)> GetEnquiryStatsAsync();
    Task<List<string>> GetAvailableServicesAsync();
    Task<List<string>> GetAvailableEngagementsAsync();
}
