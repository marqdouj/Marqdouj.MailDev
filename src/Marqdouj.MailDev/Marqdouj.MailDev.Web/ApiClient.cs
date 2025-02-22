using Microsoft.AspNetCore.WebUtilities;

namespace Marqdouj.MailDev.Web;

public class ApiClient(HttpClient httpClient)
{
    public async Task<IQueryable<WeatherForecast>?> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.AsQueryable();
    }

    public async Task SubscribeToNewsletter(string email, CancellationToken cancellationToken = default)
    {
        var values = new Dictionary<string, string?>
            {
                { nameof(email), email }
            };

        var q = QueryHelpers.AddQueryString("/newsletter/subscribe", values);
        await httpClient.PostAsync(q, null, cancellationToken);
    }

    public async Task UnSubscribeToNewsletter(string email, CancellationToken cancellationToken = default)
    {
        var values = new Dictionary<string, string?>
            {
                { nameof(email), email }
            };

        var q = QueryHelpers.AddQueryString("/newsletter/unsubscribe", values);
        await httpClient.PostAsync(q, null, cancellationToken);
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
