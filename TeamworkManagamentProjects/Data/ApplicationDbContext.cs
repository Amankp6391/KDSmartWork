using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TeamworkManagamentProjects.Models;

namespace TeamworkManagamentProjects.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactEnquiry> ContactEnquiries => Set<ContactEnquiry>();
    public DbSet<DeveloperProfile> DeveloperProfiles => Set<DeveloperProfile>();
    public DbSet<ServiceOffering> ServiceOfferings => Set<ServiceOffering>();
    public DbSet<TechnologyItem> TechnologyItems => Set<TechnologyItem>();
    public DbSet<PortfolioProject> PortfolioProjects => Set<PortfolioProject>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Serialize string lists as JSON strings for broad compatibility
        var stringListConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrWhiteSpace(v) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
        );

        modelBuilder.Entity<DeveloperProfile>(entity =>
        {
            entity.Property(e => e.PrimarySkills).HasConversion(stringListConverter);
            entity.Property(e => e.KeySpecializations).HasConversion(stringListConverter);
        });

        modelBuilder.Entity<ServiceOffering>(entity =>
        {
            entity.Property(e => e.Benefits).HasConversion(stringListConverter);
            entity.Property(e => e.TechnologiesUsed).HasConversion(stringListConverter);
        });

        modelBuilder.Entity<TechnologyItem>(entity =>
        {
            entity.Property(e => e.BestSuitedFor).HasConversion(stringListConverter);
        });

        modelBuilder.Entity<PortfolioProject>(entity =>
        {
            entity.Property(e => e.Technologies).HasConversion(stringListConverter);
            entity.Property(e => e.Features).HasConversion(stringListConverter);
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }
}
