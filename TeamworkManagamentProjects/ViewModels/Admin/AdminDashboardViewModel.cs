using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Models.Enums;

namespace TeamworkManagamentProjects.ViewModels.Admin;

public class AdminDashboardViewModel
{
    public List<ContactEnquiry> Enquiries { get; set; } = new();
    
    public int TotalEnquiries { get; set; }
    public int NewEnquiriesCount { get; set; }
    public int InDiscussionCount { get; set; }
    public int ConvertedCount { get; set; }
    public int ClosedCount { get; set; }

    // Filter properties
    public string? SearchTerm { get; set; }
    public string? SelectedService { get; set; }
    public string? SelectedEngagement { get; set; }
    public EnquiryStatus? SelectedStatus { get; set; }

    public List<string> AvailableServices { get; set; } = new();
    public List<string> AvailableEngagements { get; set; } = new();
}
