# Database Setup Guide - Kevin Main Portfolio

This guide covers setting up Entity Framework Core, creating migrations, and seeding your CV data into Azure SQL Database.

## 📦 Part 1: Install Entity Framework Core

### Step 1: Add NuGet Packages

```bash
cd KevinMain.API

# Entity Framework Core for SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.0

# EF Core Tools for migrations
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 10.0.0

# EF Core Design for scaffolding
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.0
```

---

## 🗂️ Part 2: Create Database Models

### Step 1: Create Data Models Directory

Create these model classes in `KevinMain.API/Data/Models/`:

**CVData.cs** - Main CV entity
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KevinMain.API.Data.Models;

public class CVData
{
	[Key]
	public int Id { get; set; }

	public required string Name { get; set; }
	public required string JobTitle { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public string? Location { get; set; }
	public string? LinkedInUrl { get; set; }
	public string? GitHubUrl { get; set; }

	[Column(TypeName = "nvarchar(MAX)")]
	public required string Profile { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	// Navigation properties
	public ICollection<Skill> Skills { get; set; } = new List<Skill>();
	public ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
	public Education? Education { get; set; }
	public string? LeisureActivities { get; set; }
}
```

**Skill.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Data.Models;

public class Skill
{
	[Key]
	public int Id { get; set; }

	public required string Name { get; set; }
	public string? Category { get; set; }
	public int DisplayOrder { get; set; }

	// Foreign key
	public int CVDataId { get; set; }
	public CVData? CVData { get; set; }
}
```

**WorkExperience.cs**
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KevinMain.API.Data.Models;

public class WorkExperience
{
	[Key]
	public int Id { get; set; }

	public required string Company { get; set; }
	public required string Position { get; set; }
	public required string Duration { get; set; }

	[Column(TypeName = "nvarchar(MAX)")]
	public string? Description { get; set; }

	public int DisplayOrder { get; set; }

	// Foreign key
	public int CVDataId { get; set; }
	public CVData? CVData { get; set; }

	// Navigation property
	public ICollection<Responsibility> Responsibilities { get; set; } = new List<Responsibility>();
}
```

**Responsibility.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Data.Models;

public class Responsibility
{
	[Key]
	public int Id { get; set; }

	public required string Description { get; set; }
	public int DisplayOrder { get; set; }

	// Foreign key
	public int WorkExperienceId { get; set; }
	public WorkExperience? WorkExperience { get; set; }
}
```

**Education.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace KevinMain.API.Data.Models;

public class Education
{
	[Key]
	public int Id { get; set; }

	// Higher Education
	public string? Degree { get; set; }
	public string? University { get; set; }
	public string? GraduationYear { get; set; }
	public string? FieldOfStudy { get; set; }

	// Secondary Education
	public string? SecondarySchool { get; set; }
	public string? SecondaryQualifications { get; set; }

	// Foreign key
	public int CVDataId { get; set; }
	public CVData? CVData { get; set; }
}
```

---

## 🔌 Part 3: Create DbContext

Create `KevinMain.API/Data/ApplicationDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using KevinMain.API.Data.Models;

namespace KevinMain.API.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<CVData> CVData { get; set; }
	public DbSet<Skill> Skills { get; set; }
	public DbSet<WorkExperience> WorkExperiences { get; set; }
	public DbSet<Responsibility> Responsibilities { get; set; }
	public DbSet<Education> Education { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure relationships
		modelBuilder.Entity<CVData>()
			.HasMany(c => c.Skills)
			.WithOne(s => s.CVData)
			.HasForeignKey(s => s.CVDataId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<CVData>()
			.HasMany(c => c.WorkExperiences)
			.WithOne(w => w.CVData)
			.HasForeignKey(w => w.CVDataId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<WorkExperience>()
			.HasMany(w => w.Responsibilities)
			.WithOne(r => r.WorkExperience)
			.HasForeignKey(r => r.WorkExperienceId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<CVData>()
			.HasOne(c => c.Education)
			.WithOne(e => e.CVData)
			.HasForeignKey<Education>(e => e.CVDataId)
			.OnDelete(DeleteBehavior.Cascade);

		// Configure indexes
		modelBuilder.Entity<CVData>()
			.HasIndex(c => c.Name);

		modelBuilder.Entity<Skill>()
			.HasIndex(s => new { s.CVDataId, s.DisplayOrder });

		modelBuilder.Entity<WorkExperience>()
			.HasIndex(w => new { w.CVDataId, w.DisplayOrder });
	}
}
```

---

## ⚙️ Part 4: Update Program.cs

Add DbContext registration in `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using KevinMain.API.Data;

// Add this after builder creation
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	options.UseSqlServer(connectionString);
});
```

---

## 🔄 Part 5: Create Database Service

Create `KevinMain.API/Services/DatabaseCVDataService.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using KevinMain.API.Data;
using KevinMain.API.Data.Models;
using KevinMain.API.Models;

namespace KevinMain.API.Services;

public class DatabaseCVDataService : ICVDataService
{
	private readonly ApplicationDbContext _context;
	private readonly ILogger<DatabaseCVDataService> _logger;

	public DatabaseCVDataService(ApplicationDbContext context, ILogger<DatabaseCVDataService> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task<CVModel> GetCVDataAsync()
	{
		try
		{
			// Get the first (and should be only) CV record with all related data
			var cvData = await _context.CVData
				.Include(c => c.Skills)
				.Include(c => c.WorkExperiences)
					.ThenInclude(w => w.Responsibilities)
				.Include(c => c.Education)
				.OrderByDescending(c => c.UpdatedAt)
				.FirstOrDefaultAsync();

			if (cvData == null)
			{
				_logger.LogWarning("No CV data found in database");
				throw new InvalidOperationException("CV data not found");
			}

			// Map database entity to API model
			return new CVModel
			{
				Name = cvData.Name,
				JobTitle = cvData.JobTitle,
				Email = cvData.Email,
				Phone = cvData.Phone,
				Location = cvData.Location,
				LinkedInUrl = cvData.LinkedInUrl,
				GitHubUrl = cvData.GitHubUrl,
				Profile = cvData.Profile,
				Skills = cvData.Skills
					.OrderBy(s => s.DisplayOrder)
					.Select(s => s.Name)
					.ToList(),
				WorkExperience = cvData.WorkExperiences
					.OrderBy(w => w.DisplayOrder)
					.Select(w => new WorkExperienceModel
					{
						Company = w.Company,
						Position = w.Position,
						Duration = w.Duration,
						Description = w.Description ?? string.Empty,
						Responsibilities = w.Responsibilities
							.OrderBy(r => r.DisplayOrder)
							.Select(r => r.Description)
							.ToList()
					})
					.ToList(),
				Education = cvData.Education != null ? new EducationModel
				{
					Higher = new HigherEducationModel
					{
						Degree = cvData.Education.Degree ?? string.Empty,
						University = cvData.Education.University ?? string.Empty,
						GraduationYear = cvData.Education.GraduationYear ?? string.Empty,
						FieldOfStudy = cvData.Education.FieldOfStudy ?? string.Empty
					},
					Secondary = string.IsNullOrEmpty(cvData.Education.SecondaryQualifications) 
						? new List<string>() 
						: cvData.Education.SecondaryQualifications.Split('|').ToList()
				} : new EducationModel
				{
					Higher = new HigherEducationModel(),
					Secondary = new List<string>()
				},
				LeisureActivities = cvData.LeisureActivities ?? string.Empty
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retrieving CV data from database");
			throw;
		}
	}
}
```

---

## 🌱 Part 6: Create Data Seeder

Create `KevinMain.API/Data/DbSeeder.cs`:

```csharp
using KevinMain.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KevinMain.API.Data;

public static class DbSeeder
{
	public static async Task SeedAsync(ApplicationDbContext context)
	{
		// Check if data already exists
		if (await context.CVData.AnyAsync())
		{
			return; // Database already seeded
		}

		var cv = new CVData
		{
			Name = "Kevin Main",
			JobTitle = "Lead Developer / Senior Software Engineer",
			Email = "KevMain@gmail.com",
			Phone = "07739688271",
			Location = "Cheshire, UK",
			LinkedInUrl = "https://uk.linkedin.com/in/kevmain",
			GitHubUrl = "https://github.com/KevMain",
			Profile = @"Highly experienced full-stack developer and technical leader with over 20 years of expertise in software development, system architecture, and team management. Proven track record of delivering large-scale enterprise solutions, leading development teams, and implementing modern cloud-based architectures. Strong communicator with a passion for mentoring developers and driving technical excellence.",
			Skills = new List<Skill>
			{
				new() { Name = ".NET / C#", Category = "Backend", DisplayOrder = 1 },
				new() { Name = "Azure", Category = "Cloud", DisplayOrder = 2 },
				new() { Name = "SQL Server", Category = "Database", DisplayOrder = 3 },
				new() { Name = "Vue.js", Category = "Frontend", DisplayOrder = 4 },
				new() { Name = "React", Category = "Frontend", DisplayOrder = 5 },
				new() { Name = "Docker", Category = "DevOps", DisplayOrder = 6 },
				new() { Name = "Kubernetes", Category = "DevOps", DisplayOrder = 7 },
				new() { Name = "CI/CD", Category = "DevOps", DisplayOrder = 8 },
				new() { Name = "Microservices", Category = "Architecture", DisplayOrder = 9 },
				new() { Name = "REST APIs", Category = "Backend", DisplayOrder = 10 }
			},
			WorkExperiences = new List<WorkExperience>
			{
				new()
				{
					Company = "Arbor Education Partners",
					Position = "Lead Full-Stack Developer",
					Duration = "Sep 2017 - Present",
					Description = "Leading development of cloud-based school management system serving 50,000+ users",
					DisplayOrder = 1,
					Responsibilities = new List<Responsibility>
					{
						new() { Description = "Architected and implemented microservices-based system on Azure", DisplayOrder = 1 },
						new() { Description = "Led team of 12 developers across multiple feature squads", DisplayOrder = 2 },
						new() { Description = "Delivered major features including timetabling, attendance tracking, and reporting", DisplayOrder = 3 },
						new() { Description = "Implemented CI/CD pipelines reducing deployment time by 70%", DisplayOrder = 4 }
					}
				}
				// Add more work experiences here...
			},
			Education = new Education
			{
				Degree = "BSc (Hons) Computer Science",
				University = "University of Manchester",
				GraduationYear = "2003",
				FieldOfStudy = "Computer Science",
				SecondarySchool = "Altrincham Grammar School for Boys",
				SecondaryQualifications = "A-Levels: Mathematics (A), Computer Science (A), Physics (B)|GCSEs: 10 A*-B including Mathematics and English"
			},
			LeisureActivities = @"Outside of work, I enjoy staying active through cycling and hiking. I'm passionate about technology and regularly contribute to open-source projects. I also enjoy mentoring junior developers through local coding meetups and online communities."
		};

		context.CVData.Add(cv);
		await context.SaveChangesAsync();
	}
}
```

---

## 🚀 Part 7: Create and Run Migrations

### Step 1: Create Initial Migration

```bash
# From KevinMain.API directory
dotnet ef migrations add InitialCreate
```

### Step 2: Update Database (Local Development)

```bash
# Update your local database
dotnet ef database update
```

### Step 3: Generate SQL Script for Azure

```bash
# Generate SQL script for production deployment
dotnet ef migrations script --output migration.sql
```

---

## ☁️ Part 8: Deploy to Azure SQL Database

### Option 1: Using Azure Data Studio

1. Open Azure Data Studio
2. Connect to your Azure SQL Database
3. Open file → Select `migration.sql`
4. Click "Run" to execute

### Option 2: Using Azure CLI

```bash
# Run migration script
az sql db execute \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --name kevinmain-db \
  --file migration.sql
```

### Option 3: Using sqlcmd

```bash
sqlcmd -S kevinmain-sql.database.windows.net \
  -d kevinmain-db \
  -U sqladmin \
  -P <YourPassword> \
  -i migration.sql
```

---

## 🔄 Part 9: Update Program.cs to Use Database

Update `Program.cs` to use database service in production:

```csharp
// Configuration for data source
var dataSourceConfig = builder.Configuration.GetSection("DataSource");
var useDatabase = dataSourceConfig.GetValue<bool>("UseDatabase");

if (useDatabase)
{
	// Use database in production
	builder.Services.AddScoped<ICVDataService, DatabaseCVDataService>();

	// Run migrations and seed data on startup (optional)
	var app = builder.Build();
	using (var scope = app.Services.CreateScope())
	{
		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await context.Database.MigrateAsync(); // Run migrations
		await DbSeeder.SeedAsync(context); // Seed data if empty
	}
}
else
{
	// Use in-memory service for development
	builder.Services.AddSingleton<ICVDataService, InMemoryCVDataService>();
}
```

---

## ✅ Part 10: Test Locally

1. **Update appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=KevinMainCV;Trusted_Connection=true;"
  },
  "DataSource": {
	"UseDatabase": true,
	"Provider": "SqlServer"
  }
}
```

2. **Run migrations:**
```bash
dotnet ef database update
```

3. **Start the API:**
```bash
dotnet run
```

4. **Test the endpoint:**
```bash
curl https://localhost:5001/api/cv
```

---

## 📊 Database Schema

```
┌─────────────┐
│   CVData    │
├─────────────┤
│ Id (PK)     │
│ Name        │
│ JobTitle    │
│ Email       │
│ Phone       │
│ ...         │
└─────┬───────┘
	  │
	  ├─────────────┐
	  │             │
	  ▼             ▼
┌──────────┐  ┌────────────────┐
│  Skills  │  │ WorkExperience │
├──────────┤  ├────────────────┤
│ Id (PK)  │  │ Id (PK)        │
│ Name     │  │ Company        │
│ CVDataId │  │ Position       │
└──────────┘  │ CVDataId       │
			  └────┬───────────┘
				   │
				   ▼
			  ┌──────────────────┐
			  │ Responsibilities │
			  ├──────────────────┤
			  │ Id (PK)          │
			  │ Description      │
			  │ WorkExperienceId │
			  └──────────────────┘
```

---

## 🔧 Useful Commands

```bash
# Create a new migration
dotnet ef migrations add <MigrationName>

# Remove last migration
dotnet ef migrations remove

# Update database to specific migration
dotnet ef database update <MigrationName>

# Generate SQL script
dotnet ef migrations script

# Drop database (be careful!)
dotnet ef database drop
```

---

## 🎯 Next Steps

1. ✅ Test database locally
2. ✅ Deploy migrations to Azure SQL
3. ✅ Verify API returns data from database
4. ✅ Update Container App environment variables
5. ✅ (Optional) Create admin API for CV updates

---

**🎉 Database is ready!** Your CV data is now persisted in Azure SQL Database!
