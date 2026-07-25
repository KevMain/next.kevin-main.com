namespace KevinMain.API.Models;

public class ServiceData
{
    public List<ServiceCategory> Categories { get; set; } = new();
}

public class ServiceCategory
{
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public List<Service> Services { get; set; } = new();
}

public class Service
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> KeyFeatures { get; set; } = new();
    public List<string> Technologies { get; set; } = new();
    public List<string> Deliverables { get; set; } = new();
    public string IdealFor { get; set; } = string.Empty;
}
