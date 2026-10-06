using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.Services;

public interface IAuthService
{
    Task<AdminUser?> ValidateAdminCredentialsAsync(string username, string password);
}
