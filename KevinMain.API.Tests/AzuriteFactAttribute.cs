namespace KevinMain.API.Tests;

/// <summary>
/// Fact that only runs when the RUN_AZURITE_TESTS environment variable is set to "true".
/// Explicit opt-in rather than probe-and-skip: once enabled, a broken Azurite setup
/// is a test failure, not a silent skip.
/// </summary>
public sealed class AzuriteFactAttribute : FactAttribute
{
    public AzuriteFactAttribute()
    {
        if (!string.Equals(
                Environment.GetEnvironmentVariable("RUN_AZURITE_TESTS"),
                "true",
                StringComparison.OrdinalIgnoreCase))
        {
            Skip = "Azurite integration tests are opt-in. Set RUN_AZURITE_TESTS=true and start Azurite to run them.";
        }
    }
}
