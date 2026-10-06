using Microsoft.EntityFrameworkCore;
using TeamworkManagamentProjects.Data;
using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<AdminUser?> ValidateAdminCredentialsAsync(string username, string password)
    {
        var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username.ToLower() == username.Trim().ToLower());
        if (user == null)
        {
            _logger.LogWarning("Admin login failed: User {Username} not found", username);
            return null;
        }

        if (DbInitializer.VerifyPassword(password, user.PasswordHash))
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Admin user {Username} logged in successfully", username);
            return user;
        }

        _logger.LogWarning("Admin login failed: Invalid password for {Username}", username);
        return null;
    }
}
