using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TeamworkManagamentProjects.Models;
using TeamworkManagamentProjects.Models.Enums;

namespace TeamworkManagamentProjects.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Ensure database schema is created
        context.Database.EnsureCreated();

        // Safely ensure new schema columns exist in existing SQL Server tables
        if (context.Database.IsSqlServer())
        {
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    IF OBJECT_ID(N'[dbo].[DeveloperProfiles]', N'U') IS NOT NULL
                    BEGIN
                        IF NOT EXISTS (
                            SELECT 1 FROM sys.columns 
                            WHERE object_id = OBJECT_ID(N'[dbo].[DeveloperProfiles]') 
                            AND name = 'ImageUrl'
                        )
                        BEGIN
                            ALTER TABLE [dbo].[DeveloperProfiles] ADD [ImageUrl] NVARCHAR(250) NULL;
                        END
                    END
                ");
            }
            catch
            {
                // Ignore if fallback database or column already managed
            }
        }

        // Seed Admin User if none exists
        if (!context.AdminUsers.Any())
        {
            context.AdminUsers.Add(new AdminUser
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = HashPassword("AdminPassword123!"),
                CreatedAt = DateTime.UtcNow
            });
            context.SaveChanges();
        }

        // Seed or Update 5 Developers with Leadership Team Structure
        if (!context.DeveloperProfiles.Any())
        {
            context.DeveloperProfiles.AddRange(
                new DeveloperProfile
                {
                    Name = "Aman Kumar Pandey",
                    Role = "Founder & CEO | Principal .NET Architect",
                    Experience = "8 Years",
                    YearsOfExperience = 8,
                    PrimarySkills = new List<string> { "C#", "ASP.NET Core", "SQL Server", "System Architecture", "Microservices", "REST APIs" },
                    Availability = "Full-Time / Architecture Consulting",
                    Summary = "Founder & CEO leading technical strategy, enterprise architecture, and high-performance .NET application delivery for global clients and startups.",
                    KeySpecializations = new List<string> { "Enterprise System Architecture", "Performance Profiling", "Clean Code & SOLID", "Database Optimization" },
                    ImageUrl = "/image/1761761014538.jfif",
                    DisplayOrder = 1
                },
                new DeveloperProfile
                {
                    Name = "Yashwant",
                    Role = "CTO & Technical Lead",
                    Experience = "7 Years",
                    YearsOfExperience = 7,
                    PrimarySkills = new List<string> { "ASP.NET Core", "Entity Framework Core", "C#", "Web APIs", "Docker", "SQL Server" },
                    Availability = "Full-Time / Tech Leadership",
                    Summary = "CTO & Technical Lead overseeing engineering execution, code quality standards, scalable REST APIs, and modern cloud deployment architectures.",
                    KeySpecializations = new List<string> { "Technical Direction & QA", "ASP.NET Core 8/9/10", "EF Core Query Optimization", "API Security & JWT" },
                    DisplayOrder = 2
                },
                new DeveloperProfile
                {
                    Name = "Kashish Dwivedi",
                    Role = "Senior Full-Stack .NET Developer & Operations",
                    Experience = "6 Years",
                    YearsOfExperience = 6,
                    PrimarySkills = new List<string> { "ASP.NET MVC", "ASP.NET Core", "C#", "JavaScript / jQuery", "Bootstrap 5", "SQL Server" },
                    Availability = "Full-Time / Project-Based",
                    Summary = "Senior full-stack engineer and operations specialist managing responsive enterprise web applications, frontend integration, and project delivery.",
                    KeySpecializations = new List<string> { "Full-Stack ASP.NET Core & MVC", "Responsive UI Design", "AJAX & Client Scripting", "Sprint & Project Management" },
                    DisplayOrder = 3
                },
                new DeveloperProfile
                {
                    Name = "Senior .NET Specialist",
                    Role = "ASP.NET Web Forms & Maintenance Specialist",
                    Experience = "5 Years",
                    YearsOfExperience = 5,
                    PrimarySkills = new List<string> { "ASP.NET Web Forms", "VB.NET / C#", "Legacy Maintenance", "Bug Fixing", "SQL Server", "WCF" },
                    Availability = "Full-Time / Part-Time / Maintenance",
                    Summary = "Specialist in legacy .NET codebases. Highly skilled in debugging, updating, maintaining, and gradually modernizing mature ASP.NET Web Forms applications.",
                    KeySpecializations = new List<string> { "ASP.NET Web Forms (.NET 4.x)", "Legacy Code Refactoring", "Bug Diagnostics", "Incremental Modernization" },
                    DisplayOrder = 4
                },
                new DeveloperProfile
                {
                    Name = "Database & API Engineer",
                    Role = "SQL Server & API Integration Developer",
                    Experience = "4–5 Years",
                    YearsOfExperience = 5,
                    PrimarySkills = new List<string> { "SQL Server", "T-SQL", "Stored Procedures", "REST APIs", "C#", "Third-Party Integrations" },
                    Availability = "Full-Time / Part-Time",
                    Summary = "Backend engineer specializing in SQL Server database design, stored procedures, indexing, performance tuning, and seamless external API integrations.",
                    KeySpecializations = new List<string> { "Complex SQL & Indexing", "Third-Party API Integration", "ETL & Data Sync", "Webhook & Background Services" },
                    DisplayOrder = 5
                }
            );
            context.SaveChanges();
        }
        else
        {
            // If profiles already exist, update them with the specified leadership team
            var firstDev = context.DeveloperProfiles.FirstOrDefault(d => d.DisplayOrder == 1);
            if (firstDev != null)
            {
                firstDev.Name = "Aman Kumar Pandey";
                firstDev.Role = "Founder & CEO | Principal .NET Architect";
                firstDev.Experience = "8 Years";
                firstDev.Summary = "Founder & CEO leading technical strategy, enterprise architecture, and high-performance .NET application delivery for global clients and startups.";
                firstDev.ImageUrl = "/image/1761761014538.jfif";
            }

            var secondDev = context.DeveloperProfiles.FirstOrDefault(d => d.DisplayOrder == 2);
            if (secondDev != null)
            {
                secondDev.Name = "Yashwant";
                secondDev.Role = "CTO & Technical Lead";
                secondDev.Experience = "7 Years";
                secondDev.Summary = "CTO & Technical Lead overseeing engineering execution, code quality standards, scalable REST APIs, and modern cloud deployment architectures.";
            }

            var thirdDev = context.DeveloperProfiles.FirstOrDefault(d => d.DisplayOrder == 3);
            if (thirdDev != null)
            {
                thirdDev.Name = "Kashish Dwivedi";
                thirdDev.Role = "Senior Full-Stack .NET Developer & Operations";
                thirdDev.Experience = "6 Years";
                thirdDev.Summary = "Senior full-stack engineer and operations specialist managing responsive enterprise web applications, frontend integration, and project delivery.";
            }

            context.SaveChanges();
        }

        // Seed Service Offerings
        if (!context.ServiceOfferings.Any())
        {
            context.ServiceOfferings.AddRange(
                new ServiceOffering
                {
                    Title = "ASP.NET Core Development",
                    Slug = "aspnet-core-development",
                    ShortDescription = "Develop modern, secure, and scalable ASP.NET Core applications tailored to your business needs.",
                    DetailedDescription = "We build high-performance web applications, backend APIs, and microservices using modern ASP.NET Core. Our team applies clean architecture, dependency injection, and best security practices.",
                    Benefits = new List<string> { "High throughput and cross-platform capability", "Modern dependency injection and modular design", "Strong security and authentication mechanisms", "Easy maintenance and long-term upgrade path" },
                    TechnologiesUsed = new List<string> { "ASP.NET Core", "C#", "EF Core", "SQL Server", "REST API" },
                    IconClass = "bi-speedometer2",
                    DisplayOrder = 1,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "ASP.NET MVC Development",
                    Slug = "aspnet-mvc-development",
                    ShortDescription = "Build and maintain enterprise-grade MVC web applications with robust architecture.",
                    DetailedDescription = "From enterprise dashboards to line-of-business software, we develop and maintain ASP.NET MVC applications with clean separation of concerns, rich Razor views, and reliable business logic.",
                    Benefits = new List<string> { "Clear separation of models, views, and controllers", "Rich server-rendered UI with Bootstrap 5", "Extensive testability and maintainability", "Seamless enterprise integration" },
                    TechnologiesUsed = new List<string> { "ASP.NET MVC", "C#", "SQL Server", "Bootstrap 5", "JavaScript" },
                    IconClass = "bi-window-stack",
                    DisplayOrder = 2,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "ASP.NET Web Forms Support & Maintenance",
                    Slug = "aspnet-webforms-maintenance",
                    ShortDescription = "Maintain, enhance, and debug legacy ASP.NET Web Forms applications with dependable support.",
                    DetailedDescription = "Do you have existing ASP.NET Web Forms applications that need bug fixes, new features, or ongoing support? Our team has extensive hands-on experience handling mature .NET Framework 3.5/4.x systems without breaking existing business logic.",
                    Benefits = new List<string> { "Experienced developers who know Web Forms inside-out", "Safe bug fixing without regressions", "Feature additions without requiring full rewrites", "Cost-effective legacy system lifecycle extension" },
                    TechnologiesUsed = new List<string> { "ASP.NET Web Forms", "C#", "VB.NET", "SQL Server", ".NET Framework" },
                    IconClass = "bi-gear-wide-connected",
                    DisplayOrder = 3,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "New Web Application Development",
                    Slug = "new-application-development",
                    ShortDescription = "Build modern, scalable web applications from scratch based on your custom specifications.",
                    DetailedDescription = "We partner with companies and startups to design, develop, test, and deploy end-to-end custom web applications from conceptual wireframes to production release.",
                    Benefits = new List<string> { "Tailor-made software aligned with your business processes", "Scalable database schema and architecture", "Clean code that your in-house team can easily inherit", "Transparent agile milestone delivery" },
                    TechnologiesUsed = new List<string> { "C#", "ASP.NET Core", "SQL Server", "HTML5", "CSS3", "Bootstrap" },
                    IconClass = "bi-laptop",
                    DisplayOrder = 4,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "Existing Project Maintenance",
                    Slug = "existing-project-maintenance",
                    ShortDescription = "Maintain, improve, stabilize, and support your ongoing production software.",
                    DetailedDescription = "Keep your critical software running smoothly. We take over maintenance, resolve production tickets, apply security patches, and perform scheduled updates.",
                    Benefits = new List<string> { "Quick onboarding on existing codebases", "Flexible hours (part-time or full-time)", "Dedicated tracking of issues and code fixes", "Peace of mind for your core business operations" },
                    TechnologiesUsed = new List<string> { "C#", "ASP.NET Core/MVC", "SQL Server", "Git", "Azure/IIS" },
                    IconClass = "bi-tools",
                    DisplayOrder = 5,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "Bug Fixing & Code Diagnostics",
                    Slug = "bug-fixing",
                    ShortDescription = "Fast, systematic troubleshooting and resolution for tough software defects.",
                    DetailedDescription = "Stuck with mysterious crashes, memory leaks, unhandled exceptions, or database deadlocks? Our senior developers diagnose root causes and deliver verified fixes.",
                    Benefits = new List<string> { "Deep diagnostic approach using logs and profiling", "Root-cause resolution instead of band-aids", "Regression testing after every fix", "Clear technical explanation of what caused the bug" },
                    TechnologiesUsed = new List<string> { "C#", "Visual Studio Diagnostics", "SQL Server Profiler", "Logging" },
                    IconClass = "bi-bug",
                    DisplayOrder = 6,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "New Module Development",
                    Slug = "new-module-development",
                    ShortDescription = "Add new business modules, workflows, and features to your existing systems.",
                    DetailedDescription = "Expand your existing application without disrupting current operations. We build reporting modules, payment gateways, admin dashboards, workflow managers, and user portals.",
                    Benefits = new List<string> { "Seamless integration with existing databases and UI", "Minimal disruption to production users", "Full documentation and clean code integration", "Scalable for future requirements" },
                    TechnologiesUsed = new List<string> { "C#", "ASP.NET Core / MVC", "Entity Framework", "SQL Server" },
                    IconClass = "bi-plus-square",
                    DisplayOrder = 7,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "API Development & Integration",
                    Slug = "api-development",
                    ShortDescription = "Develop secure REST APIs and connect with third-party web services and webhooks.",
                    DetailedDescription = "We create high-performance RESTful APIs, Swagger/OpenAPI documentation, token-based authentication (JWT/OAuth), and integrate external APIs like payment gateways, CRMs, and ERPs.",
                    Benefits = new List<string> { "Secure, standards-compliant REST endpoints", "Interactive API documentation", "Robust error handling and rate limiting", "Seamless webhook receivers and background sync" },
                    TechnologiesUsed = new List<string> { "ASP.NET Core Web API", "REST", "JSON", "JWT", "HTTP Client" },
                    IconClass = "bi-hdd-network",
                    DisplayOrder = 8,
                    IsFeaturedOnHome = true
                },
                new ServiceOffering
                {
                    Title = "SQL Server Development & Optimization",
                    Slug = "sql-server-development",
                    ShortDescription = "Database design, indexing, stored procedures, query optimization, and maintenance.",
                    DetailedDescription = "Optimize database performance and ensure data integrity. We handle table normalization, indexing strategies, complex stored procedures, triggers, views, and data migrations.",
                    Benefits = new List<string> { "Accelerated query execution and reduced latency", "Optimized index and constraint structures", "Elimination of deadlocks and blocking queries", "Reliable backup and schema versioning plans" },
                    TechnologiesUsed = new List<string> { "SQL Server", "T-SQL", "EF Core", "Execution Plans", "SSMS" },
                    IconClass = "bi-database",
                    DisplayOrder = 9,
                    IsFeaturedOnHome = false
                },
                new ServiceOffering
                {
                    Title = "Third-Party API Integration",
                    Slug = "third-party-api-integration",
                    ShortDescription = "Connect your .NET software with payment gateways, CRMs, shipping providers, and ERPs.",
                    DetailedDescription = "Integrate Stripe, PayPal, Salesforce, HubSpot, SendGrid, Twilio, accounting software, and custom partner APIs securely with resilient retry mechanisms.",
                    Benefits = new List<string> { "Automated business data synchronization", "Reliable HTTP retry policies and circuit breakers", "Secure API credential management", "Webhook payload processing and validation" },
                    TechnologiesUsed = new List<string> { "C#", "HttpClientFactory", "Polly", "REST APIs", "JSON" },
                    IconClass = "bi-link-45deg",
                    DisplayOrder = 10,
                    IsFeaturedOnHome = false
                },
                new ServiceOffering
                {
                    Title = "Performance Optimization",
                    Slug = "performance-optimization",
                    ShortDescription = "Speed up slow page loads, reduce CPU/memory consumption, and optimize database queries.",
                    DetailedDescription = "We analyze bottlenecks across the entire application stack: frontend asset loading, server response times, ORM query efficiency, caching strategies, and database index tuning.",
                    Benefits = new List<string> { "Faster response times and improved user satisfaction", "Lower server and cloud hosting costs", "Memory leak detection and resolution", "In-memory and distributed caching integration" },
                    TechnologiesUsed = new List<string> { "ASP.NET Core", "MemoryCache", "SQL Server Profiler", "Async/Await" },
                    IconClass = "bi-lightning-charge",
                    DisplayOrder = 11,
                    IsFeaturedOnHome = false
                },
                new ServiceOffering
                {
                    Title = "Dedicated Technical Support",
                    Slug = "technical-support",
                    ShortDescription = "Ongoing support agreements with predictable response times and direct developer access.",
                    DetailedDescription = "Ensure your software always has expert eyes on it. We provide regular maintenance packages, code audits, security review, and direct communication via Teams/Slack/Email.",
                    Benefits = new List<string> { "Direct communication with assigned senior developers", "Fast turnaround for urgent issues", "Transparent time tracking and reporting", "Proactive health checks and dependency updates" },
                    TechnologiesUsed = new List<string> { ".NET Ecosystem", "SQL Server", "Git", "Issue Trackers" },
                    IconClass = "bi-headset",
                    DisplayOrder = 12,
                    IsFeaturedOnHome = false
                }
            );
            context.SaveChanges();
        }

        // Seed Technology Items
        if (!context.TechnologyItems.Any())
        {
            context.TechnologyItems.AddRange(
                new TechnologyItem
                {
                    Name = "C#",
                    Category = "Backend & Core",
                    Description = "Modern, type-safe, object-oriented language for high-performance enterprise applications.",
                    BestSuitedFor = new List<string> { "Enterprise Logic", "Microservices", "REST APIs", "Background Workers" },
                    ProficiencyPercentage = 95,
                    IconBadgeClass = "bi-filetype-cs",
                    DisplayOrder = 1
                },
                new TechnologyItem
                {
                    Name = "ASP.NET Core",
                    Category = "Backend & Core",
                    Description = "Modern, cross-platform, high-performance web framework for modern cloud and on-premise solutions.",
                    BestSuitedFor = new List<string> { "Greenfield Web Apps", "High-Throughput APIs", "Cloud Deployments", "Microservices" },
                    ProficiencyPercentage = 95,
                    IconBadgeClass = "bi-cpu",
                    DisplayOrder = 2
                },
                new TechnologyItem
                {
                    Name = "ASP.NET MVC",
                    Category = "Backend & Core",
                    Description = "Proven MVC architecture for enterprise server-side web applications with rich Razor templating.",
                    BestSuitedFor = new List<string> { "Enterprise Portals", "Internal Business Tools", "CRUD Applications", "E-Commerce" },
                    ProficiencyPercentage = 92,
                    IconBadgeClass = "bi-window",
                    DisplayOrder = 3
                },
                new TechnologyItem
                {
                    Name = "ASP.NET Web Forms",
                    Category = "Legacy & Maintenance",
                    Description = "Legacy .NET Framework application maintenance, bug fixing, and modern component integration.",
                    BestSuitedFor = new List<string> { "Legacy ERP/CRM Systems", "Enterprise Intranets", "Maintenance & Bug Fixes" },
                    ProficiencyPercentage = 88,
                    IconBadgeClass = "bi-layout-text-window-reverse",
                    DisplayOrder = 4
                },
                new TechnologyItem
                {
                    Name = "SQL Server",
                    Category = "Database & Storage",
                    Description = "Enterprise relational database design, complex T-SQL stored procedures, and index optimization.",
                    BestSuitedFor = new List<string> { "Relational Data Modeling", "High-Volume Transactions", "Stored Procedures & Triggers", "Reporting" },
                    ProficiencyPercentage = 90,
                    IconBadgeClass = "bi-database",
                    DisplayOrder = 5
                },
                new TechnologyItem
                {
                    Name = "Entity Framework Core",
                    Category = "Database & Storage",
                    Description = "Object-relational mapper for .NET enabling strongly typed database querying and automated migrations.",
                    BestSuitedFor = new List<string> { "ORM Data Access", "Code-First Migrations", "LINQ Queries", "Repository Layer" },
                    ProficiencyPercentage = 92,
                    IconBadgeClass = "bi-diagram-3",
                    DisplayOrder = 6
                },
                new TechnologyItem
                {
                    Name = "Angular",
                    Category = "Frontend & SPAs",
                    Description = "Enterprise-grade TypeScript framework for building rich, modular Single-Page Applications (SPAs) integrated with ASP.NET Core Web APIs.",
                    BestSuitedFor = new List<string> { "Enterprise Web Portals", "Single Page Apps (SPA)", "Complex Data Tables", "Role-Based Dashboards" },
                    ProficiencyPercentage = 90,
                    IconBadgeClass = "bi-shield-shaded",
                    DisplayOrder = 7
                },
                new TechnologyItem
                {
                    Name = "React",
                    Category = "Frontend & SPAs",
                    Description = "Modern declarative UI library for building responsive, component-driven client user interfaces with seamless .NET backend connectivity.",
                    BestSuitedFor = new List<string> { "Interactive Web Apps", "Real-Time SaaS Dashboards", "Micro-Frontends", "High-Performance UI" },
                    ProficiencyPercentage = 90,
                    IconBadgeClass = "bi-code-square",
                    DisplayOrder = 8
                },
                new TechnologyItem
                {
                    Name = "REST API & Webhooks",
                    Category = "Integration & APIs",
                    Description = "RESTful services, JWT security, third-party webhook receivers, and API orchestration.",
                    BestSuitedFor = new List<string> { "Mobile App Backends", "Third-Party Integrations", "Partner Data Exchange", "Microservice Comm" },
                    ProficiencyPercentage = 94,
                    IconBadgeClass = "bi-arrow-left-right",
                    DisplayOrder = 9
                },
                new TechnologyItem
                {
                    Name = "JavaScript & jQuery",
                    Category = "Frontend",
                    Description = "Client-side interactivity, AJAX asynchronous requests, and DOM manipulation.",
                    BestSuitedFor = new List<string> { "Dynamic Forms", "AJAX Data Grids", "Client-Side Validation", "Interactive Dashboards" },
                    ProficiencyPercentage = 85,
                    IconBadgeClass = "bi-filetype-js",
                    DisplayOrder = 10
                },
                new TechnologyItem
                {
                    Name = "HTML5 & CSS3",
                    Category = "Frontend",
                    Description = "Semantic web markup, modern CSS layouts, flexbox, CSS grid, and responsive styling.",
                    BestSuitedFor = new List<string> { "Responsive Web UI", "SEO Semantics", "Cross-Browser Compatibility", "Accessibility" },
                    ProficiencyPercentage = 92,
                    IconBadgeClass = "bi-filetype-html",
                    DisplayOrder = 11
                },
                new TechnologyItem
                {
                    Name = "Bootstrap 5",
                    Category = "Frontend",
                    Description = "Modern, responsive CSS framework for rapid, mobile-friendly interface development.",
                    BestSuitedFor = new List<string> { "Mobile-First Layouts", "Admin Dashboards", "Responsive Tables & Modals", "B2B UI" },
                    ProficiencyPercentage = 94,
                    IconBadgeClass = "bi-bootstrap",
                    DisplayOrder = 12
                }
            );
            context.SaveChanges();
        }
        else
        {
            // Ensure Angular and React exist in database
            if (!context.TechnologyItems.Any(t => t.Name == "Angular"))
            {
                context.TechnologyItems.Add(new TechnologyItem
                {
                    Name = "Angular",
                    Category = "Frontend & SPAs",
                    Description = "Enterprise-grade TypeScript framework for building rich, modular Single-Page Applications (SPAs) integrated with ASP.NET Core Web APIs.",
                    BestSuitedFor = new List<string> { "Enterprise Web Portals", "Single Page Apps (SPA)", "Complex Data Tables", "Role-Based Dashboards" },
                    ProficiencyPercentage = 90,
                    IconBadgeClass = "bi-shield-shaded",
                    DisplayOrder = 7
                });
            }

            if (!context.TechnologyItems.Any(t => t.Name == "React"))
            {
                context.TechnologyItems.Add(new TechnologyItem
                {
                    Name = "React",
                    Category = "Frontend & SPAs",
                    Description = "Modern declarative UI library for building responsive, component-driven client user interfaces with seamless .NET backend connectivity.",
                    BestSuitedFor = new List<string> { "Interactive Web Apps", "Real-Time SaaS Dashboards", "Micro-Frontends", "High-Performance UI" },
                    ProficiencyPercentage = 90,
                    IconBadgeClass = "bi-code-square",
                    DisplayOrder = 8
                });
            }

            context.SaveChanges();
        }

        // Seed Sample Architectural Portfolio Projects (Placeholder Showcases)
        if (!context.PortfolioProjects.Any())
        {
            context.PortfolioProjects.AddRange(
                new PortfolioProject
                {
                    ProjectName = "Enterprise Logistics & Fleet Management System",
                    ProjectType = "ASP.NET Core Web Application",
                    Technologies = new List<string> { "ASP.NET Core", "C#", "SQL Server", "EF Core", "Bootstrap 5", "REST API" },
                    ShortDescription = "A scalable B2B logistics tracking and dispatch system handling shipment workflows and fleet telemetry.",
                    DetailedOverview = "Demonstrates an enterprise architecture for multi-tenant logistics management with automated status tracking, driver assignment, real-time dispatch alerts, and comprehensive reporting.",
                    Features = new List<string> { "Role-based access control (Admin, Dispatcher, Driver)", "Real-time shipment status transitions", "Automated PDF bill of lading generation", "High-performance SQL indexing for millions of tracking logs" },
                    ProjectStatus = "Architecture Showcase / Reference Case",
                    DisplayOrder = 1
                },
                new PortfolioProject
                {
                    ProjectName = "Healthcare Provider Portal & Appointment Scheduling",
                    ProjectType = "ASP.NET MVC Web Application",
                    Technologies = new List<string> { "ASP.NET MVC", "C#", "SQL Server", "JavaScript", "Bootstrap 5" },
                    ShortDescription = "A secure patient and practitioner appointment coordination portal with automated reminders.",
                    DetailedOverview = "Built with ASP.NET MVC, this project demonstrates secure handling of sensitive data, calendar scheduling with conflict resolution, and patient notification workflows.",
                    Features = new List<string> { "Interactive scheduling calendar", "Automated email/SMS notification queues", "Audit logging for patient record access", "Custom data export to Excel and PDF" },
                    ProjectStatus = "Architecture Showcase / Reference Case",
                    DisplayOrder = 2
                },
                new PortfolioProject
                {
                    ProjectName = "Legacy ERP Maintenance & ASP.NET Core Modernization",
                    ProjectType = "Web Forms & ASP.NET Core Migration",
                    Technologies = new List<string> { "ASP.NET Web Forms", "ASP.NET Core", "C#", "SQL Server", "Stored Procedures" },
                    ShortDescription = "Incremental modernization and maintenance of a 10-year-old financial accounting system.",
                    DetailedOverview = "Showcases our capability to maintain legacy Web Forms applications while gradually extracting business logic into modern ASP.NET Core REST micro-modules without business downtime.",
                    Features = new List<string> { "Refactored legacy stored procedures for 40% faster ledger computation", "Resolved memory leaks and concurrency locks", "Replaced outdated ActiveX components with modern HTML5/Bootstrap", "Created API bridges between legacy and new modules" },
                    ProjectStatus = "Architecture Showcase / Reference Case",
                    DisplayOrder = 3
                },
                new PortfolioProject
                {
                    ProjectName = "B2B Payment & Billing Gateway Integration",
                    ProjectType = "REST API & Microservice",
                    Technologies = new List<string> { "ASP.NET Core Web API", "C#", "SQL Server", "Polly", "JWT" },
                    ShortDescription = "A resilient payment processing hub connecting multiple third-party merchant APIs with automated reconciliation.",
                    DetailedOverview = "Engineered for high availability, this service handles recurring subscription billing, webhook event verification, automated retry policies, and transaction auditing.",
                    Features = new List<string> { "Idempotent payment transaction processing", "Polly-based exponential backoff & retry mechanism", "HMAC webhook verification", "Comprehensive structured logging and diagnostics" },
                    ProjectStatus = "Architecture Showcase / Reference Case",
                    DisplayOrder = 4
                }
            );
            context.SaveChanges();
        }

        // Seed initial sample enquiry for admin testing
        if (!context.ContactEnquiries.Any())
        {
            context.ContactEnquiries.AddRange(
                new ContactEnquiry
                {
                    Name = "Alex Mercer",
                    CompanyName = "Apex Digital Solutions",
                    Email = "alex.mercer@example.com",
                    Phone = "+1 (555) 234-5678",
                    ServiceRequired = "ASP.NET Core Development",
                    EngagementType = "Full-Time",
                    ExperienceRequired = "6–8 Years",
                    Technology = "ASP.NET Core",
                    ProjectDescription = "We need an experienced full-time ASP.NET Core developer to join our team for building a SaaS customer portal and integrating REST APIs with SQL Server.",
                    PreferredContactMethod = "Email",
                    ExpectedStartDate = "Next 2 Weeks",
                    AdditionalRequirements = "Experience with EF Core and Clean Architecture is a plus.",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    Status = EnquiryStatus.New
                },
                new ContactEnquiry
                {
                    Name = "Sarah Jenkins",
                    CompanyName = "Vanguard Financial Systems",
                    Email = "s.jenkins@example.com",
                    Phone = "+1 (555) 987-6543",
                    ServiceRequired = "ASP.NET Web Forms",
                    EngagementType = "Part-Time",
                    ExperienceRequired = "4–5 Years",
                    Technology = "ASP.NET Web Forms",
                    ProjectDescription = "We have an existing ASP.NET Web Forms application that requires maintenance, bug fixing, and new reporting module development 20 hours per week.",
                    PreferredContactMethod = "VideoCall",
                    ExpectedStartDate = "Immediately",
                    AdditionalRequirements = "Need developer comfortable with legacy VB.NET/C# codebases and SQL Server stored procedures.",
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    Status = EnquiryStatus.InDiscussion,
                    AdminNotes = "Held introductory call. Client sending sample codebase repository next Monday."
                }
            );
            context.SaveChanges();
        }
    }

    public static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password + "DevTeamSalt_2026");
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        var computedHash = HashPassword(enteredPassword);
        return computedHash == storedHash;
    }
}
