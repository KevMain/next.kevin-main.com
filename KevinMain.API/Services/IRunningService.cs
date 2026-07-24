using KevinMain.API.Models;

namespace KevinMain.API.Services;

public interface IRunningService
{
    Task<IEnumerable<RunningActivity>> GetAllActivitiesAsync();
    Task<RunningActivity?> GetActivityByIdAsync(int id);
    Task<PersonalBests> GetPersonalBestsAsync();
    Task<IEnumerable<RunningActivity>> GetRecentActivitiesAsync(int count = 5);
    Task<IEnumerable<string>> GetRecentImagesAsync(int count = 6);
}
