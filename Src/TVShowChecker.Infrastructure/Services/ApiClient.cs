using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TVShowChecker.Infrastructure.Services;

internal static class ApiClient
{
    private const int MaxAttempts = 4;
    private const int InitialDelay = 50;

    private static readonly HttpClient httpClient = new();
    private static readonly Random random = new();

    public static async Task<string> Get(string requestUri)
    {
        if (string.IsNullOrWhiteSpace(requestUri))
        {
            return null;
        }

        var delay = InitialDelay;

        for (var attempt = 0; attempt < MaxAttempts; attempt++)
        {
            try
            {
                var response = await httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                if (response.StatusCode == HttpStatusCode.TooManyRequests && response.Headers?.RetryAfter != null)
                {
                    await Task.Delay(response.Headers.RetryAfter.Delta ?? TimeSpan.FromSeconds(delay));
                }
                else
                {
                    await Task.Delay(delay);
                    delay = delay * 2 + random.Next(0, 100);
                }
            }
            catch
            {
                await Task.Delay(delay);
                delay = delay * 2 + random.Next(0, 100);
            }
        }

        return null;
    }
}
