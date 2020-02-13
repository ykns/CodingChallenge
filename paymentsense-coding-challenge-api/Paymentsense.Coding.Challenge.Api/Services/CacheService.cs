using System;
using Microsoft.Extensions.Caching.Memory;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public class CacheEntry<T>
    {
        public bool IsAlive { get; }
        public T Value { get; }

        public CacheEntry(bool isAlive, T value)
        {
            this.IsAlive = isAlive;
            this.Value = value;
        }
    }
    public interface ICacheService
    {
        CacheEntry<T> GetEntry<T>(string key);
        void SetSingleEntryWithExpirationAt<T>(string key, T entry, DateTime absoluteExpirationDateTime);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache memoryCacheStore;
        public CacheService(IMemoryCache memoryCacheStore)
        {
            this.memoryCacheStore = memoryCacheStore;
        }

        public CacheEntry<T> GetEntry<T>(string key)
        {
            if (!this.memoryCacheStore.TryGetValue(key, out T value))
            {
                return new CacheEntry<T>(false, default(T));
            }

            return new CacheEntry<T>(true, value);
        }

        public void SetSingleEntryWithExpirationAt<T>(string key, T value, DateTime absoluteExpirationDateTime)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetAbsoluteExpiration(absoluteExpirationDateTime);
            this.memoryCacheStore.Set(key, value, cacheEntryOptions);
        }
    }
}