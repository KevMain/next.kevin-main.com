namespace KevinMain.API.Models;

public class StravaSettings
{
    public bool Enabled { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class StravaTokenResponse
{
    public string access_token { get; set; } = string.Empty;
    public string refresh_token { get; set; } = string.Empty;
    public int expires_at { get; set; }
}

public class StravaActivity
{
    public long id { get; set; }
    public string name { get; set; } = string.Empty;
    public float distance { get; set; }
    public int moving_time { get; set; }
    public int elapsed_time { get; set; }
    public float total_elevation_gain { get; set; }
    public string type { get; set; } = string.Empty;
    public DateTime start_date { get; set; }
    public float? average_speed { get; set; }
    public bool manual { get; set; }
    public string? description { get; set; }
}
