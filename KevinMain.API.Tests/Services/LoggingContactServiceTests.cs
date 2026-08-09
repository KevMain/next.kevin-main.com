using FluentAssertions;
using KevinMain.API.Models;
using KevinMain.API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KevinMain.API.Tests.Services;

/// <summary>
/// Unit tests for LoggingContactService (fallback contact handler).
/// </summary>
public class LoggingContactServiceTests
{
    private readonly Mock<ILogger<LoggingContactService>> _mockLogger;
    private readonly LoggingContactService _sut;

    public LoggingContactServiceTests()
    {
        _mockLogger = new Mock<ILogger<LoggingContactService>>();
        _sut = new LoggingContactService(_mockLogger.Object);
    }

    private static ContactRequest CreateValidRequest() => new()
    {
        Name = "Jane Smith",
        Email = "jane@example.com",
        Subject = "Enquiry about services",
        Message = "Hello, I'd like to discuss a potential project.",
        SubmittedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task ProcessContactRequestAsync_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = await _sut.ProcessContactRequestAsync(request);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        result.ErrorDetails.Should().BeNull();
    }

    [Fact]
    public async Task ProcessContactRequestAsync_LogsTheSubmission()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        await _sut.ProcessContactRequestAsync(request);

        // Assert - an Information-level log entry was written
        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains(request.Email)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ProcessContactRequestAsync_MultipleRequests_AllSucceed()
    {
        // Arrange & Act
        var results = new List<ContactResult>();
        for (var i = 0; i < 3; i++)
        {
            results.Add(await _sut.ProcessContactRequestAsync(CreateValidRequest()));
        }

        // Assert
        results.Should().AllSatisfy(r => r.Success.Should().BeTrue());
    }
}
