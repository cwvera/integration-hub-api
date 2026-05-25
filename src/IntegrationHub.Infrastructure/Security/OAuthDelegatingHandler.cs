using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;

namespace IntegrationHub.Infrastructure.Security;

/// <summary>Handler para gestionar tokens OAuth de forma transparente inyectando el encabezado Bearer.</summary>
public class OAuthDelegatingHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;
    private const string TokenCacheKey = "OAuthAccessToken";

    public OAuthDelegatingHandler(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetOrRefreshTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _cache.Remove(TokenCacheKey);
            token = await GetOrRefreshTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }

    private async Task<string> GetOrRefreshTokenAsync(CancellationToken ct)
    {
        return await _cache.GetOrCreateAsync(TokenCacheKey, async entry =>
        {
            await Task.Delay(100, ct); 
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(50);
            return "mock-access-token-" + Guid.NewGuid();
        }) ?? string.Empty;
    }
}
