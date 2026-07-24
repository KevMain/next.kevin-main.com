namespace KevinMain.API.Models;

public class RunningActivity
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Distance { get; set; } // In kilometers
    public TimeSpan Duration { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? AveragePace { get; set; } // Minutes per kilometer
    public int? Elevation { get; set; } // In meters
    public string? InstagramPostUrl { get; set; }
    public string? StravaUrl { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class PersonalBests
{
    public RunBest? Fastest5K { get; set; }
    public RunBest? Fastest10K { get; set; }
    public RunBest? FastestHalfMarathon { get; set; }
    public RunBest? FastestMarathon { get; set; }
}

public class RunBest
{
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Distance { get; set; }
    public TimeSpan Duration { get; set; }
    public decimal? Pace { get; set; }
    public int? Elevation { get; set; }
}
