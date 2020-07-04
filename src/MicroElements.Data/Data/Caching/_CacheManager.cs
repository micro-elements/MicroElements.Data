using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroElements.Functional;
using MicroElements.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MicroElements.Data.Caching
{


    public interface ICacheContext
    {
        ICacheEntry CacheEntry { get; }

        IMutablePropertyContainer Metadata { get; }

        Action OnHit { get; set; }
        Action OnMiss { get; set; }
        Action OnExpiration { get; set; }
    }

    internal class CacheContext : ICacheContext
    {
        /// <inheritdoc />
        public ICacheEntry CacheEntry { get; }

        /// <inheritdoc />
        public IMutablePropertyContainer Metadata { get; }

        /// <inheritdoc />
        public Action OnHit { get; set; }

        /// <inheritdoc />
        public Action OnMiss { get; set; }

        /// <inheritdoc />
        public Action OnExpiration { get; set; }

        public CacheContext(ICacheEntry cacheEntry)
        {
            CacheEntry = cacheEntry;
            Metadata = new MutablePropertyContainer();
        }
    }

    internal class CacheItem<TValue>
    {
        public TValue Value { get; }

        public Message Error { get; }

        public IPropertyContainer Metadata { get; }

        public ICacheContext CacheContext { get; }

        public CacheItem(TValue value, Message error, CacheContext cacheContext)
        {
            Value = value;
            Error = error;
            CacheContext = cacheContext;
            Metadata = cacheContext.Metadata;
        }
    }

    public enum CacheHitMiss
    {
        Hit,
        Miss
    }

    public static class CacheDataExtensions
    {
        public static IEnumerable<T> GetAllValues<T>(this CacheManager cacheManager, ICacheSettings<T> settings)
        {
            var cacheSection = cacheManager.GetSection(settings);

            var values = cacheSection
                .Keys
                .Select(key => cacheSection.Get(key))
                .Where(option => option.IsSome)
                .Select(option => option.GetValueOrDefault());

            return values;
        }

        public static TimeSpan Elapsed(this IMetadataProvider provider) => provider.Metadata.GetValue(CacheManager.Elapsed);

        public static async Task<CacheResult<T>> WithLogging<T>(this Task<CacheResult<T>> cacheResultTask, ILogger logger)
        {
            var cacheResult = await cacheResultTask;
            return cacheResult.WithLogging(logger);
        }

        public static CacheResult<T> WithLogging<T>(this in CacheResult<T> cacheResult, ILogger logger)
        {
            if (cacheResult.HitMiss == CacheHitMiss.Hit)
            {
                logger.LogInformation($"CACHE HIT : {cacheResult.SectionName}.{cacheResult.Key}. Data was found in Cache.");
            }
            else if (cacheResult.HitMiss == CacheHitMiss.Miss)
            {
                if (cacheResult.Error == null)
                    logger.LogInformation($"CACHE MISS: {cacheResult.SectionName}.{cacheResult.Key}. Data was successfully retrieved from {cacheResult.Settings.DataSource}. Elapsed: {(int)cacheResult.Elapsed().TotalMilliseconds} ms.");
                else
                    logger.LogInformation($"CACHE ERROR: {cacheResult.SectionName}.{cacheResult.Key}. An error during attempt to get data from {cacheResult.Settings.DataSource}. Error: {cacheResult.Error.FormattedMessage}. Elapsed: {(int)cacheResult.Elapsed().TotalMilliseconds} ms.");
            }

            return cacheResult;
        }
    }

    internal static class AwaitableLock
    {
        internal readonly struct LockLease : IDisposable
        {
            private readonly SemaphoreSlim _lock;
            public LockLease(SemaphoreSlim @lock) => _lock = @lock;
            public void Dispose() => _lock.Release();
        }

        public static async ValueTask<LockLease> WaitAsyncAndGetLockReleaser(this SemaphoreSlim semaphoreSlim)
        {
            await semaphoreSlim.WaitAsync();
            return new LockLease(semaphoreSlim);
        }

        public static LockLease WaitAndGetLockReleaser(this SemaphoreSlim semaphoreSlim)
        {
            semaphoreSlim.Wait();
            return new LockLease(semaphoreSlim);
        }
    }
}
