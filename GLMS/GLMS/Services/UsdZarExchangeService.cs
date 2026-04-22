using System.Text.Json;

namespace GLMS.Services;

// External API (POE section 3): assisted by DeepSeek AI — i reviewed and wired it into GLMS.
// HttpClient GETs Frankfurter JSON; returns ZAR per 1 USD or null if it fails.
public class UsdZarExchangeService
{
    private readonly HttpClient _http;

    public UsdZarExchangeService(HttpClient http) => _http = http;

    public async Task<decimal?> GetUsdToZarAsync(CancellationToken ct = default)
    {
        try
        {
            using var response = await _http.GetAsync("latest?from=USD&to=ZAR", ct);
            if (!response.IsSuccessStatusCode)
                return null;

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            if (!doc.RootElement.TryGetProperty("rates", out var rates) || !rates.TryGetProperty("ZAR", out var zar))
                return null;

            return zar.GetDecimal();
        }
        catch
        {
            return null;
        }
    }
}

// References (accessed 20 Apr 2026):
// - Frankfurter API docs: https://www.frankfurter.app/docs/
// - IHttpClientFactory in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
// - System.Text.Json overview: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview
