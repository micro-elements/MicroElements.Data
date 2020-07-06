// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroElements.Functional;
using MicroElements.Metadata;
using Microsoft.Extensions.Logging;

namespace MicroElements.Data.Caching
{
    public static class CacheDataExtensions
    {
        public static IEnumerable<T> GetAllValues<T>(this CacheManager cacheManager, ICacheSectionDescriptor<T> sectionDescriptor)
        {
            var cacheSection = cacheManager.GetOrCreateSection(sectionDescriptor);

            var values = cacheSection
                .Keys
                .Select(key => cacheSection.Get(key))
                .Where(option => option.IsSome)
                .Select(option => option.GetValueOrDefault());

            return values;
        }

        public static IEnumerable<T> GetAllValues<T>(this CacheManager cacheManager, string sectionName)
        {
            var cacheSection = cacheManager.GetSection<T>(sectionName);
            if (cacheSection == null)
                return Array.Empty<T>();

            IEnumerable<T> values = cacheSection.Keys
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
}
