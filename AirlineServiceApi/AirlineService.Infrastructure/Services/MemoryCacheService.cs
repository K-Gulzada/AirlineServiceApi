using AirlineService.Application.Common;
using AirlineService.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AirlineService.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly HashSet<string> _cacheKeys = new();
    private readonly object _lock = new();

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions();
        
        if (expiration.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiration;
        }
        else
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Constants.Cache.DefaultCacheMinutes);
        }

        lock (_lock)
        {
            _cacheKeys.Add(key);
        }

        _cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        lock (_lock)
        {
            _cacheKeys.Remove(key);
        }

        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern)
    {
        var keysToRemove = new List<string>();
        var patternWithoutWildcard = pattern.TrimEnd('*');

        lock (_lock)
        {
            keysToRemove = _cacheKeys.Where(key => key.StartsWith(patternWithoutWildcard, StringComparison.OrdinalIgnoreCase)).ToList();
           
            foreach (var key in keysToRemove)
            {
                _cacheKeys.Remove(key);
                _cache.Remove(key);
            }
        }

        return Task.CompletedTask;
    }
}

