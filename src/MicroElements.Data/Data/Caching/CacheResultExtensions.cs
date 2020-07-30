// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// CacheResult extensions.
    /// </summary>
    public static class CacheResultExtensions
    {
        /// <summary>
        /// Gets <see cref="CacheResult{TValue}.Value"/>.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="cacheResultTask">The source task.</param>
        /// <returns>Value or default.</returns>
        public static async Task<T> GetValueAsync<T>(this Task<CacheResult<T>> cacheResultTask)
        {
            var cacheResult = await cacheResultTask;
            return cacheResult.GetValue();
        }

        /// <summary>
        /// Gets <see cref="CacheResult{TValue}.Value"/> if <see cref="CacheResult{TValue}.IsSuccess"/> or default if error.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="cacheResultTask">The source task.</param>
        /// <returns>Value or default.</returns>
        public static async Task<T> GetValueOrDefaultAsync<T>(this Task<CacheResult<T>> cacheResultTask)
        {
            var cacheResult = await cacheResultTask;
            return cacheResult.GetValueOrDefault();
        }

        /// <summary>
        /// Gets <see cref="CacheResult{TValue}.Value"/> if <see cref="CacheResult{TValue}.IsSuccess"/> or throws <see cref="CacheException"/> if in error state.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="cacheResultTask">The source task.</param>
        /// <returns>Value or throws exception.</returns>
        public static async Task<T> GetValueOrThrowAsync<T>(this Task<CacheResult<T>> cacheResultTask)
        {
            var cacheResult = await cacheResultTask;
            return cacheResult.GetValueOrThrow();
        }

        /// <summary>
        /// Logs <see cref="CacheResult{TValue}"/> advanced info: CACHE HIT, CACHE MISS, CACHE ERROR.
        /// </summary>
        public static async Task<CacheResult<T>> WriteToLog<T>(this Task<CacheResult<T>> cacheResultTask, ILogger logger)
        {
            var cacheResult = await cacheResultTask;
            return cacheResult.WriteToLog(logger);
        }

        /// <summary>
        /// Logs <see cref="CacheResult{TValue}"/> advanced info: CACHE HIT, CACHE MISS, CACHE ERROR.
        /// </summary>
        public static CacheResult<T> WriteToLog<T>(this in CacheResult<T> cacheResult, ILogger logger)
        {
            string elapsed = $"Elapsed: {(int)cacheResult.GetElapsed().TotalMilliseconds} ms.";
            if (cacheResult.HitMiss == CacheHitMiss.Hit)
            {
                logger.LogInformation($"CACHE HIT : {cacheResult.SectionName}.{cacheResult.Key}. Data was found in Cache, {elapsed}");
            }
            else if (cacheResult.HitMiss == CacheHitMiss.Miss)
            {
                if (cacheResult.Error == null)
                    logger.LogInformation($"CACHE MISS: {cacheResult.SectionName}.{cacheResult.Key}. Data was successfully retrieved from {cacheResult.Settings.DataSource}, {elapsed}.");
                else
                    logger.LogInformation($"CACHE ERROR: {cacheResult.SectionName}.{cacheResult.Key}. An error during attempt to get data from {cacheResult.Settings.DataSource}. Error: {cacheResult.Error.FormattedMessage}, {elapsed}.");
            }

            return cacheResult;
        }
    }
}
