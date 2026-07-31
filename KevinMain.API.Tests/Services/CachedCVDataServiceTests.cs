using FluentAssertions;
using KevinMain.API.Models;
using KevinMain.API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KevinMain.API.Tests.Services;

/// <summary>
/// Unit tests for CachedCVDataService covering caching behavior, thread-safety, and error handling.
/// </summary>
public class CachedCVDataServiceTests : IDisposable
{
    private readonly Mock<ICVDataService> _mockInnerService;
    private readonly Mock<ILogger<CachedCVDataService>> _mockLogger;
    private readonly CachedCVDataService _sut;

    public CachedCVDataServiceTests()
    {
        _mockInnerService = new Mock<ICVDataService>();
        _mockLogger = new Mock<ILogger<CachedCVDataService>>();
        var settings = new CachingSettings { CVCacheDurationHours = 1 };
        _sut = new CachedCVDataService(_mockInnerService.Object, _mockLogger.Object, settings);
    }

    private static CVData CreateSampleCVData(string name = "John Doe")
    {
        return new CVData
        {
            PersonalInfo = new PersonalInfo
            {
                Name = name,
                Title = "Software Engineer",
                Email = "john@example.com",
                Phone = "123-456-7890",
                Location = "UK"
            },
            Profile = "Sample profile text",
            KeySkills = new List<string> { "C#", ".NET", "Azure" },
            Tools = new List<string> { "Visual Studio", "Git" },
            WorkExperience = new List<WorkExperience>
            {
                new WorkExperience
                {
                    Company = "Tech Corp",
                    Position = "Software Engineer",
                    StartDate = "Jan 2020",
                    EndDate = "Present",
                    Location = "UK",
                    Description = "Develop software",
                    Highlights = new List<string> { "Built APIs", "Wrote tests" },
                    TechStack = ".NET, Azure"
                }
            },
            Education = new Education
            {
                Higher = new HigherEducation
                {
                    University = "University",
                    Course = "Computer Science",
                    Grade = "2:1",
                    Dates = "2016-2019"
                }
            },
            LeisureActivities = "Running, Reading"
        };
    }

    [Fact]
    public async Task GetCVDataAsync_FirstCall_FetchesFromInnerService()
    {
        // Arrange
        var expectedData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(expectedData);

        // Act
        var result = await _sut.GetCVDataAsync();

        // Assert
        result.Should().BeSameAs(expectedData);
        _mockInnerService.Verify(s => s.GetCVDataAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCVDataAsync_SecondCallWithinCacheDuration_ReturnsCachedData()
    {
        // Arrange
        var expectedData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(expectedData);

        // Act
        var firstResult = await _sut.GetCVDataAsync();
        var secondResult = await _sut.GetCVDataAsync();

        // Assert
        secondResult.Should().BeSameAs(firstResult);
        _mockInnerService.Verify(s => s.GetCVDataAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCVDataAsync_InnerServiceThrows_NoCachedData_Rethrows()
    {
        // Arrange
        _mockInnerService.Setup(s => s.GetCVDataAsync())
            .ThrowsAsync(new InvalidOperationException("Data source unavailable"));

        // Act
        var act = async () => await _sut.GetCVDataAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Data source unavailable");
    }

    [Fact]
    public async Task GetCVDataAsync_ConcurrentRequests_OnlyFetchesOnce()
    {
        // Arrange
        var expectedData = CreateSampleCVData();
        var callCount = 0;

        _mockInnerService.Setup(s => s.GetCVDataAsync())
            .Returns(async () =>
            {
                Interlocked.Increment(ref callCount);
                await Task.Delay(50); // Simulate slow data fetch
                return expectedData;
            });

        // Act - 10 concurrent requests
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => Task.Run(() => _sut.GetCVDataAsync()))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        // Assert
        callCount.Should().Be(1, "only one thread should fetch from the inner service");
        results.Should().AllSatisfy(r => r.Should().BeSameAs(expectedData));
    }

    [Fact]
    public async Task GetPersonalInfoAsync_ReturnsPersonalInfoFromCachedData()
    {
        // Arrange
        var cvData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(cvData);

        // Act
        var result = await _sut.GetPersonalInfoAsync();

        // Assert
        result.Should().BeSameAs(cvData.PersonalInfo);
        result.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task GetProfileAsync_ReturnsProfileDataFromCachedData()
    {
        // Arrange
        var cvData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(cvData);

        // Act
        var result = await _sut.GetProfileAsync();

        // Assert
        result.Should().NotBeNull();
        result.Profile.Should().Be(cvData.Profile);
        result.KeySkills.Should().BeEquivalentTo(cvData.KeySkills);
        result.Tools.Should().BeEquivalentTo(cvData.Tools);
    }

    [Fact]
    public async Task GetWorkExperienceAsync_ReturnsWorkExperienceFromCachedData()
    {
        // Arrange
        var cvData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(cvData);

        // Act
        var result = await _sut.GetWorkExperienceAsync();

        // Assert
        result.Should().BeSameAs(cvData.WorkExperience);
        result.Should().ContainSingle(w => w.Company == "Tech Corp");
    }

    [Fact]
    public async Task GetEducationAsync_ReturnsEducationFromCachedData()
    {
        // Arrange
        var cvData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(cvData);

        // Act
        var result = await _sut.GetEducationAsync();

        // Assert
        result.Should().BeSameAs(cvData.Education);
        result.Higher.University.Should().Be("University");
    }

    [Fact]
    public async Task GetLeisureActivitiesAsync_ReturnsLeisureActivitiesFromCachedData()
    {
        // Arrange
        var cvData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(cvData);

        // Act
        var result = await _sut.GetLeisureActivitiesAsync();

        // Assert
        result.Should().Be("Running, Reading");
    }

    [Fact]
    public async Task AllDerivedGetters_ShareTheSameCachedCVData()
    {
        // Arrange
        var cvData = CreateSampleCVData();
        _mockInnerService.Setup(s => s.GetCVDataAsync()).ReturnsAsync(cvData);

        // Act - call every derived getter
        await _sut.GetPersonalInfoAsync();
        await _sut.GetProfileAsync();
        await _sut.GetWorkExperienceAsync();
        await _sut.GetEducationAsync();
        await _sut.GetLeisureActivitiesAsync();

        // Assert - underlying data was fetched only once
        _mockInnerService.Verify(s => s.GetCVDataAsync(), Times.Once);
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
}
