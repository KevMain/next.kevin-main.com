using KevinMain.API.Models;

namespace KevinMain.API.Services;

public class InMemoryRunningService : IRunningService
{
    private readonly IWebHostEnvironment _environment;

    public InMemoryRunningService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    private readonly List<RunningActivity> _activities = new()
    {
        new RunningActivity
        {
            Id = 1,
            Date = new DateTime(2024, 3, 15),
            Title = "5K Personal Best",
            Distance = 5.0m,
            Duration = TimeSpan.FromMinutes(18).Add(TimeSpan.FromSeconds(33)),
            Location = "Parkrun",
            Description = "Amazing 5K PB! Felt strong all the way through and managed to negative split. Perfect conditions and great pacing.",
            ImageUrl = null,
            AveragePace = 3.71m,
            Elevation = 45,
            InstagramPostUrl = "https://www.instagram.com/runmainorun/",
            Tags = new List<string> { "5k", "pb", "parkrun", "race" }
        },
        new RunningActivity
        {
            Id = 2,
            Date = new DateTime(2023, 9, 10),
            Title = "10K Personal Best",
            Distance = 10.0m,
            Duration = TimeSpan.FromMinutes(39).Add(TimeSpan.FromSeconds(14)),
            Location = "City 10K Race",
            Description = "Incredible 10K PB! Sub-40 minutes achieved! Consistent pacing throughout and strong finish.",
            ImageUrl = null,
            AveragePace = 3.92m,
            Elevation = 85,
            InstagramPostUrl = "https://www.instagram.com/runmainorun/",
            Tags = new List<string> { "10k", "pb", "race", "sub40" }
        },
        new RunningActivity
        {
            Id = 3,
            Date = new DateTime(2023, 5, 21),
            Title = "Half Marathon Personal Best",
            Distance = 21.1m,
            Duration = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(27)).Add(TimeSpan.FromSeconds(55)),
            Location = "City Half Marathon",
            Description = "New half marathon PB! Solid pacing strategy paid off. Managed the distance well and pushed hard in the final 5K.",
            ImageUrl = null,
            AveragePace = 4.17m,
            Elevation = 180,
            InstagramPostUrl = "https://www.instagram.com/runmainorun/",
            Tags = new List<string> { "half-marathon", "pb", "race", "21k" }
        },
        new RunningActivity
        {
            Id = 4,
            Date = new DateTime(2022, 10, 2),
            Title = "Marathon Personal Best",
            Distance = 42.2m,
            Duration = TimeSpan.FromHours(3).Add(TimeSpan.FromMinutes(39)).Add(TimeSpan.FromSeconds(12)),
            Location = "Manchester Marathon",
            Description = "Marathon PB! What an incredible journey. Trained hard for months and it all came together on race day. The last 10K was tough but I held on for a great time!",
            ImageUrl = null,
            AveragePace = 5.19m,
            Elevation = 320,
            InstagramPostUrl = "https://www.instagram.com/runmainorun/",
            Tags = new List<string> { "marathon", "pb", "race", "42k", "manchester" }
        },
        new RunningActivity
        {
            Id = 5,
            Date = DateTime.Now.AddDays(-7),
            Title = "Morning Easy Run",
            Distance = 8.5m,
            Duration = TimeSpan.FromMinutes(48),
            Location = "Local Park",
            Description = "Comfortable morning run. Building base mileage and keeping the legs fresh. Beautiful weather today!",
            ImageUrl = null,
            AveragePace = 5.65m,
            Elevation = 85,
            InstagramPostUrl = "https://www.instagram.com/runmainorun/",
            Tags = new List<string> { "morning", "easy", "base-building" }
        },
        new RunningActivity
        {
            Id = 6,
            Date = DateTime.Now.AddDays(-3),
            Title = "Tempo Run",
            Distance = 12.0m,
            Duration = TimeSpan.FromMinutes(60),
            Location = "Canal Path",
            Description = "Solid tempo effort. Maintained a good rhythm throughout. Feeling strong in training!",
            ImageUrl = null,
            AveragePace = 5.0m,
            Elevation = 50,
            InstagramPostUrl = "https://www.instagram.com/runmainorun/",
            Tags = new List<string> { "tempo", "training", "workout" }
        }
    };

    public Task<IEnumerable<RunningActivity>> GetAllActivitiesAsync()
    {
        return Task.FromResult(_activities.OrderByDescending(a => a.Date).AsEnumerable());
    }

    public Task<RunningActivity?> GetActivityByIdAsync(int id)
    {
        var activity = _activities.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(activity);
    }

    public Task<PersonalBests> GetPersonalBestsAsync()
    {
        var pbs = new PersonalBests();

        // Helper function to create RunBest from activity
        RunBest CreateBest(RunningActivity? activity)
        {
            if (activity == null) return null!;
            return new RunBest
            {
                Title = activity.Title,
                Date = activity.Date,
                Distance = activity.Distance,
                Duration = activity.Duration,
                Pace = activity.AveragePace,
                Elevation = activity.Elevation
            };
        }

        // Fastest 5K (4.5km - 5.5km range)
        var fastest5K = _activities
            .Where(a => a.Distance >= 4.5m && a.Distance <= 5.5m && a.AveragePace.HasValue)
            .OrderBy(a => a.AveragePace)
            .FirstOrDefault();
        pbs.Fastest5K = CreateBest(fastest5K);

        // Fastest 10K (9.5km - 10.5km range)
        var fastest10K = _activities
            .Where(a => a.Distance >= 9.5m && a.Distance <= 10.5m && a.AveragePace.HasValue)
            .OrderBy(a => a.AveragePace)
            .FirstOrDefault();
        pbs.Fastest10K = CreateBest(fastest10K);

        // Fastest Half Marathon (20km - 22km range)
        var fastestHalf = _activities
            .Where(a => a.Distance >= 20m && a.Distance <= 22m && a.AveragePace.HasValue)
            .OrderBy(a => a.AveragePace)
            .FirstOrDefault();
        pbs.FastestHalfMarathon = CreateBest(fastestHalf);

        // Fastest Marathon (41km - 43km range)
        var fastestMarathon = _activities
            .Where(a => a.Distance >= 41m && a.Distance <= 43m && a.AveragePace.HasValue)
            .OrderBy(a => a.AveragePace)
            .FirstOrDefault();
        pbs.FastestMarathon = CreateBest(fastestMarathon);

        return Task.FromResult(pbs);
    }

    public Task<IEnumerable<RunningActivity>> GetRecentActivitiesAsync(int count = 5)
    {
        var recent = _activities.OrderByDescending(a => a.Date)
                                .Take(count)
                                .AsEnumerable();
        return Task.FromResult(recent);
    }

    public Task<IEnumerable<string>> GetRecentImagesAsync(int count = 6)
    {
        var imagesPath = Path.Combine(_environment.WebRootPath, "images", "running");

        // Ensure directory exists
        if (!Directory.Exists(imagesPath))
        {
            Directory.CreateDirectory(imagesPath);
        }

        // Get all image files from the directory
        var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var imageFiles = Directory.GetFiles(imagesPath)
            .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()))
            .OrderByDescending(f => new FileInfo(f).CreationTime)
            .Take(count)
            .Select(f => $"/images/running/{Path.GetFileName(f)}")
            .ToList();

        // If no images found, return placeholder images
        if (!imageFiles.Any())
        {
            imageFiles = new List<string>
            {
                "https://images.unsplash.com/photo-1552674605-db6ffd4facb5?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1571008887538-b36bb32f4571?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1483721310020-03333e577078?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1476480862126-209bfaa8edc8?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1523633589114-88eaf4b4f1a8?w=400&h=400&fit=crop",
                "https://images.unsplash.com/photo-1519315901367-f34ff9154487?w=400&h=400&fit=crop"
            };
        }

        return Task.FromResult(imageFiles.Take(count).AsEnumerable());
    }
}
