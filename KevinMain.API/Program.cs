using KevinMain.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Register CV data service
// To switch to database: Replace InMemoryCVDataService with DatabaseCVDataService
// e.g., builder.Services.AddScoped<ICVDataService, DatabaseCVDataService>();
builder.Services.AddSingleton<ICVDataService, InMemoryCVDataService>();

// Register contact form service
// To switch to real email: Replace LoggingContactService with SmtpContactService or SendGridContactService
// e.g., builder.Services.AddScoped<IContactService, SmtpContactService>();
builder.Services.AddScoped<IContactService, LoggingContactService>();

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

// Skip HTTPS redirection in development to avoid CORS issues
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
