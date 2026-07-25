using KevinMain.API.Models;

namespace KevinMain.API.Services;

public interface IServiceDataService
{
    Task<ServiceData> GetServiceDataAsync();
}
