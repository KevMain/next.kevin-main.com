using KevinMain.API.Models;
using KevinMain.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy => policy.WithOrigins("https://localhost:5173", "http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Register CV data service
// To switch to database: Replace InMemoryCVDataService with DatabaseCVDataService
// e.g., builder.Services.AddScoped<ICVDataService, DatabaseCVDataService>();
builder.Services.AddSingleton<ICVDataService, InMemoryCVDataService>();

// Configure SMTP settings from appsettings.json
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>() ?? new SmtpSettings();
builder.Services.AddSingleton(smtpSettings);

// Register contact form service
// Automatically uses SMTP if Enabled=true in appsettings.json, otherwise falls back to logging
if (smtpSettings.Enabled)
{
    builder.Services.AddScoped<IContactService, SmtpContactService>();
}
else
{
    builder.Services.AddScoped<IContactService, LoggingContactService>();
}

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowVueApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
