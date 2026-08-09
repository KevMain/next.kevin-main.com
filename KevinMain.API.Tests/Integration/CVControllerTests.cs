using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using KevinMain.API.Models;
using Xunit;

namespace KevinMain.API.Tests.Integration;

/// <summary>
/// Integration tests for CVController endpoints.
/// </summary>
public class CVControllerTests : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client;

    public CVControllerTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCV_ReturnsOkWithCompleteData()
    {
        // Act
        var response = await _client.GetAsync("/api/cv", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cv = await response.Content.ReadFromJsonAsync<CVData>(TestContext.Current.CancellationToken);
        cv.Should().NotBeNull();
        cv!.PersonalInfo.Name.Should().NotBeNullOrWhiteSpace();
        cv.WorkExperience.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetPersonal_ReturnsOkWithPersonalInfo()
    {
        // Act
        var response = await _client.GetAsync("/api/cv/personal", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var info = await response.Content.ReadFromJsonAsync<PersonalInfo>(TestContext.Current.CancellationToken);
        info.Should().NotBeNull();
        info!.Name.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetProfile_ReturnsOkWithProfileData()
    {
        // Act
        var response = await _client.GetAsync("/api/cv/profile", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var profile = await response.Content.ReadFromJsonAsync<ProfileData>(TestContext.Current.CancellationToken);
        profile.Should().NotBeNull();
        profile!.Profile.Should().NotBeNullOrWhiteSpace();
        profile.KeySkills.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetExperience_ReturnsOkWithWorkExperience()
    {
        // Act
        var response = await _client.GetAsync("/api/cv/experience", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var experience = await response.Content.ReadFromJsonAsync<List<WorkExperience>>(TestContext.Current.CancellationToken);
        experience.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetEducation_ReturnsOkWithEducation()
    {
        // Act
        var response = await _client.GetAsync("/api/cv/education", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var education = await response.Content.ReadFromJsonAsync<Education>(TestContext.Current.CancellationToken);
        education.Should().NotBeNull();
    }

    [Fact]
    public async Task GetLeisure_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/cv/leisure", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
