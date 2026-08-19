using KevinMain.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KevinMain.API.Tests;

/// <summary>
/// Verifies the startup options-validation rules registered in Program.cs:
/// TableName is always required; ConnectionString is required in Development;
/// an absolute https ServiceUri is required outside Development.
/// </summary>
public class BlogStorageSettingsValidationTests
{
    private static IOptions<BlogStorageSettings> BuildOptions(Dictionary<string, string?> config, bool isDevelopment)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(config).Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services
            .AddOptions<BlogStorageSettings>()
            .BindConfiguration("BlogStorage")
            .ValidateDataAnnotations()
            .Validate(s => isDevelopment
                    ? !string.IsNullOrWhiteSpace(s.ConnectionString)
                    : Uri.TryCreate(s.ServiceUri, UriKind.Absolute, out var serviceUri)
                        && serviceUri.Scheme == Uri.UriSchemeHttps,
                "BlogStorage requires ConnectionString in Development or an absolute https ServiceUri otherwise.");

        return services.BuildServiceProvider().GetRequiredService<IOptions<BlogStorageSettings>>();
    }

    [Fact]
    public void MissingTableName_FailsValidation()
    {
        var options = BuildOptions(new()
        {
            ["BlogStorage:TableName"] = "",
            ["BlogStorage:ConnectionString"] = "UseDevelopmentStorage=true"
        }, isDevelopment: true);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void Development_MissingConnectionString_FailsValidation()
    {
        var options = BuildOptions(new()
        {
            ["BlogStorage:TableName"] = "blogposts"
        }, isDevelopment: true);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void Production_MissingServiceUri_FailsValidation()
    {
        var options = BuildOptions(new()
        {
            ["BlogStorage:TableName"] = "blogposts",
            ["BlogStorage:ConnectionString"] = "UseDevelopmentStorage=true"
        }, isDevelopment: false);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void Production_PlaceholderServiceUri_FailsValidation()
    {
        // The appsettings.json default placeholder is not a valid URI and must fail
        // at startup validation, not with a UriFormatException at TableClient construction.
        var options = BuildOptions(new()
        {
            ["BlogStorage:TableName"] = "blogposts",
            ["BlogStorage:ServiceUri"] = "https://{your-storage-account}.table.core.windows.net"
        }, isDevelopment: false);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void Production_NonHttpsServiceUri_FailsValidation()
    {
        var options = BuildOptions(new()
        {
            ["BlogStorage:TableName"] = "blogposts",
            ["BlogStorage:ServiceUri"] = "http://account.table.core.windows.net"
        }, isDevelopment: false);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void Development_WithConnectionString_Passes()
    {
        var options = BuildOptions(new()
        {
            ["BlogStorage:TableName"] = "blogposts",
            ["BlogStorage:ConnectionString"] = "UseDevelopmentStorage=true"
        }, isDevelopment: true);

        Assert.Equal("blogposts", options.Value.TableName);
    }

    [Fact]
    public void Production_WithServiceUri_Passes()
    {
        var options = BuildOptions(new()
        {
            ["BlogStorage:TableName"] = "blogposts",
            ["BlogStorage:ServiceUri"] = "https://account.table.core.windows.net"
        }, isDevelopment: false);

        Assert.Equal("https://account.table.core.windows.net", options.Value.ServiceUri);
    }
}
