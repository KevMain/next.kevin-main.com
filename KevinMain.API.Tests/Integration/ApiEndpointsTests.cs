using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using KevinMain.API.Models;
using Xunit;

namespace KevinMain.API.Tests.Integration;

/// <summary>
/// Integration tests for RunningController, ServicesController and health endpoints.
/// </summary>
public class ApiEndpointsTests : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client;

    public ApiEndpointsTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRunningActivities_ReturnsOkWithActivities()
    {
        // Act
        var response = await _client.GetAsync("/api/running/activities", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var activities = await response.Content.ReadFromJsonAsync<List<RunningActivity>>(TestContext.Current.CancellationToken);
        activities.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetRunningActivityById_ExistingId_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/running/activities/1", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetRunningActivityById_UnknownId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/running/activities/99999", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPersonalBests_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/running/pbs", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetServices_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/services", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Be("Healthy");
    }

    [Fact]
    public async Task DetailedHealthEndpoint_ReturnsJsonWithChecks()
    {
        // Act
        var response = await _client.GetAsync("/health/detailed", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("strava_api");
        body.Should().Contain("smtp_server");
    }

    [Fact]
    public async Task Responses_IncludeSecurityHeaders()
    {
        // Act
        var response = await _client.GetAsync("/api/cv", TestContext.Current.CancellationToken);

        // Assert
        response.Headers.Should().ContainKey("X-Content-Type-Options");
        response.Headers.Should().ContainKey("X-Frame-Options");
        response.Headers.Should().ContainKey("Content-Security-Policy");
    }
}
