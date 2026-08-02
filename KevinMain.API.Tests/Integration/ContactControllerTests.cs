using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using KevinMain.API.Models;
using Xunit;

namespace KevinMain.API.Tests.Integration;

/// <summary>
/// Integration tests for ContactController including model validation.
/// </summary>
public class ContactControllerTests : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client;

    public ContactControllerTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    private static ContactRequest CreateValidRequest() => new()
    {
        Name = "Integration Tester",
        Email = "tester@example.com",
        Subject = "Integration test subject",
        Message = "This is a valid test message with more than ten characters."
    };

    [Fact]
    public async Task SubmitContact_ValidRequest_ReturnsOk()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/contact", CreateValidRequest(), TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SubmitContact_MissingName_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidRequest();
        request.Name = string.Empty;

        // Act
        var response = await _client.PostAsJsonAsync("/api/contact", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SubmitContact_InvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidRequest();
        request.Email = "not-an-email";

        // Act
        var response = await _client.PostAsJsonAsync("/api/contact", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SubmitContact_MessageTooShort_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidRequest();
        request.Message = "short";

        // Act
        var response = await _client.PostAsJsonAsync("/api/contact", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
