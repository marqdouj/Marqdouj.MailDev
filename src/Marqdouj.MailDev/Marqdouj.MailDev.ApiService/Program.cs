using MailKit.Net.Smtp;
using Marqdouj.MailDev.Client;
using MimeKit;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddMailKitClient("maildev");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/newsletter/subscribe",
    async (MailKitClientFactory factory, string email) =>
    {
        ISmtpClient client = await factory.GetSmtpClientAsync();

        using var message = new MailMessage("newsletter@yourcompany.com", email)
        {
            Subject = "Welcome to our newsletter!",
            Body = "Thank you for subscribing to our newsletter!"
        };

        await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
    });

app.MapPost("/newsletter/unsubscribe",
    async (MailKitClientFactory factory, string email) =>
    {
        ISmtpClient client = await factory.GetSmtpClientAsync();

        using var message = new MailMessage("newsletter@yourcompany.com", email)
        {
            Subject = "You are unsubscribed from our newsletter!",
            Body = "Sorry to see you go. We hope you will come back soon!"
        };

        await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
    });

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
