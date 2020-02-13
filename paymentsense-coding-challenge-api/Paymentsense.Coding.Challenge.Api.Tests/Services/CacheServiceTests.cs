using System;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Paymentsense.Coding.Challenge.Api.Services;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests.Services
{
    public class CacheServiceTests
    {
        private ICacheService CreateCacheService()
        {
            // preferred using actual implementation, as the mock code would be
            // be brittle and complicated
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return new CacheService(memoryCache);
        }

        [Fact]
        public void Get__When__Entry_Exists_And_Not_Expired__Then__Returns_True_And_Entry()
        {
            var cacheService = CreateCacheService();
            var firstName = "bob";
            var lastName = "loblaw";
            cacheService.SetSingleEntryWithExpirationAt(firstName, lastName, DateTime.Now.AddDays(1));

            var entry = cacheService.GetEntry<string>(firstName);

            entry.IsAlive.Should().Be(true);
            entry.Value.Should().Be(lastName);
        }

        [Fact]
        public void Get__When__Entry_Exists_And_Expired__Then__Returns_False()
        {
            var cacheService = CreateCacheService();
            var firstName = "bob";
            var lastName = "loblaw";
            cacheService.SetSingleEntryWithExpirationAt(firstName, lastName, DateTime.Now.AddDays(-1));

            var entry = cacheService.GetEntry<string>(firstName);

            entry.IsAlive.Should().Be(false);
        }
    }
}