using FluentAssertions;
using KevinMain.API.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace KevinMain.API.Tests.Services;

/// <summary>
/// Unit tests for InMemoryRunningService (fallback when Strava is disabled).
/// </summary>
public class InMemoryRunningServiceTests : IDisposable
{
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly InMemoryRunningService _sut;
    private readonly string _tempWebRoot;

    public InMemoryRunningServiceTests()
    {
        // Use a temp directory as web root so image tests are isolated
        _tempWebRoot = Path.Combine(Path.GetTempPath(), $"running-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempWebRoot);

        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockEnvironment.SetupGet(e => e.WebRootPath).Returns(_tempWebRoot);

        _sut = new InMemoryRunningService(_mockEnvironment.Object);
    }

    [Fact]
    public async Task GetAllActivitiesAsync_ReturnsActivities()
    {
        // Act
        var result = (await _sut.GetAllActivitiesAsync()).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(a =>
        {
            a.Id.Should().BeGreaterThan(0);
            a.Title.Should().NotBeNullOrWhiteSpace();
            a.Distance.Should().BeGreaterThan(0);
        });
    }

    [Fact]
    public async Task GetActivityByIdAsync_ExistingId_ReturnsActivity()
    {
        // Act
        var result = await _sut.GetActivityByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetActivityByIdAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var result = await _sut.GetActivityByIdAsync(99999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPersonalBestsAsync_ReturnsBestsForKnownDistances()
    {
        // Act
        var result = await _sut.GetPersonalBestsAsync();

        // Assert - seed data contains 5K, 10K and half marathon PBs
        result.Should().NotBeNull();
        result.Fastest5K.Should().NotBeNull();
        result.Fastest10K.Should().NotBeNull();
        result.FastestHalfMarathon.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRecentActivitiesAsync_ReturnsRequestedCount_OrderedByDateDescending()
    {
        // Act
        var result = (await _sut.GetRecentActivitiesAsync(count: 3)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeInDescendingOrder(a => a.Date);
    }

    [Fact]
    public async Task GetRecentImagesAsync_NoLocalImages_ReturnsPlaceholders()
    {
        // Act - temp web root has no images
        var result = (await _sut.GetRecentImagesAsync(count: 6)).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(url => url.Should().StartWith("http"));
    }

    [Fact]
    public async Task GetRecentImagesAsync_WithLocalImages_ReturnsLocalPaths()
    {
        // Arrange - create fake image files in the web root
        var imagesDir = Path.Combine(_tempWebRoot, "images", "running");
        Directory.CreateDirectory(imagesDir);
        File.WriteAllBytes(Path.Combine(imagesDir, "run1.jpg"), new byte[] { 1 });
        File.WriteAllBytes(Path.Combine(imagesDir, "run2.png"), new byte[] { 1 });

        // Act
        var result = (await _sut.GetRecentImagesAsync(count: 6)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(url => url.Should().StartWith("/images/running/"));
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_tempWebRoot, recursive: true);
        }
        catch
        {
            // best-effort cleanup
        }
    }
}
