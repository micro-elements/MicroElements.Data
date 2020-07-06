using System;
using System.Threading.Tasks;
using FluentAssertions;
using MicroElements.Data.Caching;
using MicroElements.Functional;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace MicroElements.Data.Tests
{
    public class DataTests
    {
        [Fact]
        public async Task Test1()
        {
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheManager = new CacheManager(memoryCache);

            double d1 = await cacheManager.GetOrCreateAsync(Cache.Section1, "Value1", context => Task.FromResult(1.0));
            double d2 = await cacheManager.GetOrCreateAsync(Cache.Section1, "Value2", context => Task.FromResult(2.0));

            ICacheSection<double> cacheSection = cacheManager.GetOrCreateSection(Cache.Section1);
        }

        [Fact]
        public async Task factory_method_for_one_key_should_be_called_only_once()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheManager = new CacheManager(memoryCache);

            var cacheSectionDescriptor = new CacheSectionDescriptor<TestData>("Section1");

            async Task<TestData> GetDataAsync()
            {
                await Task.Delay(200);
                return new TestData("test");
            }

            //CacheSection.ForType<TestData>("Section1");

            var getData1 = cacheManager.GetOrCreateAsync(cacheSectionDescriptor, "Value1", context => GetDataAsync());
            var getData2 = cacheManager.GetOrCreateAsync(cacheSectionDescriptor, "Value1", context => GetDataAsync());

            // run in parallel
            await Task.WhenAll(getData1, getData2);

            TestData data1 = getData1.Result.Value;
            TestData data2 = getData2.Result.Value;

            data1.Should().BeSameAs(data2);
        }
        
        [Fact]
        public async Task TODO()
        {
            // cache error
            // cache item customize
        }

    }

    public static class Cache
    {
        public static ICacheSectionDescriptor<double> Section1 = new CacheSectionDescriptor<double>("Section1", new CacheSettings<double>()
        {
            CacheErrorValue = false,
            Validate = d => d < 0? new Message("Should be greater then 0.", MessageSeverity.Error) : null
        });
    }

    public class TestData : IEquatable<TestData>
    {
        public string Name { get; }

        public TestData(string name)
        {
            Name = name;
        }

        /// <inheritdoc />
        public bool Equals(TestData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestData) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(TestData left, TestData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TestData left, TestData right)
        {
            return !Equals(left, right);
        }
    }
}
