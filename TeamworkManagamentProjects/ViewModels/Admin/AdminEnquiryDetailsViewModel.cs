using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Models.Enums;

namespace TeamworkManagamentProjects.ViewModels.Admin;

public class AdminEnquiryDetailsViewModel
{
    public ContactEnquiry Enquiry { get; set; } = null!;
}

public class UpdateStatusViewModel
{
    public int Id { get; set; }
    public EnquiryStatus Status { get; set; }
    public string? AdminNotes { get; set; }
}
