using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Paymentsense.Coding.Challenge.Api.Controllers;
using Paymentsense.Coding.Challenge.Api.Dtos;
using Paymentsense.Coding.Challenge.Api.Services;
using Paymentsense.Coding.Challenge.Api.Extensions;
using Xunit;
using System;
using System.Collections.Generic;
using Paymentsense.Coding.Challenge.Api.Exceptions;

namespace Paymentsense.Coding.Challenge.Api.Tests.Controllers
{
    public class CountryControllerTests
    {
        [Fact]
        public async Task GetAllCountries__When_Cache_Entry_Is_Not_Alive__Then__Returns_Calls_Rest_Countries_Api()
        {
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(d => d.GetNow()).Returns(DateTime.Now);
            var countries = GetCountryDtos();
            var webClientService = new Mock<IWebClientService>();
            var maxAge = TimeSpan.FromMinutes(5);
            var response = CreateResponse(HttpStatusCode.OK, countries.Serialize());
            response.Headers.Add("cache-control", new [] { $"public, max-age={maxAge.TotalSeconds}"});
            webClientService.Setup(w => w.GetResponseAsync(It.IsAny<string>()))
                .ReturnsAsync(response);
            var cacheService = new Mock<ICacheService>();
            cacheService.Setup(c => c.GetEntry<IEnumerable<Country>>(It.IsAny<string>()))
                .Returns(new CacheEntry<IEnumerable<Country>>(false, null));
            var controller = new CountryController(webClientService.Object, cacheService.Object, dateTimeService.Object, new MapService());

            var results = await controller.GetAllCountries().ToListAsync();
            
            results.Should().NotBeEmpty();
            foreach (var result in results)
            {
                countries.Any(c => c.Name == result.Name).Should().BeTrue();
            }
            webClientService.Verify(_ => _.GetResponseAsync(It.IsAny<string>()), Times.Once);
            cacheService.Verify(c => c.SetSingleEntryWithExpirationAt(It.IsAny<string>(), It.IsAny<IEnumerable<Country>>(), It.IsAny<DateTime>()));
        }

        [Fact]
        public async Task GetAllCountries__When_Cache_Entry_Is_Not_Alive__Then__Cache_Rest_Countries_Api_Response_With_Valid_Expiry()
        {
            var dateTimeService = new Mock<IDateTimeService>();
            var now = new DateTime(2000, 1, 1);
            dateTimeService.Setup(d => d.GetNow()).Returns(now);
            var countries = GetCountryDtos();
            var maxAge = TimeSpan.FromMinutes(5);
            var response = CreateResponse(HttpStatusCode.OK, countries.Serialize());
            response.Headers.Add("cache-control", new [] { $"public, max-age={maxAge.TotalSeconds}"});
            var webClientService = new Mock<IWebClientService>();
            webClientService.Setup(w => w.GetResponseAsync(It.IsAny<string>()))
                .ReturnsAsync(response);
            var cacheService = new Mock<ICacheService>();
            cacheService.Setup(c => c.GetEntry<IEnumerable<Country>>(It.IsAny<string>()))
                .Returns(new CacheEntry<IEnumerable<Country>>(false, null));
            var controller = new CountryController(webClientService.Object, cacheService.Object, dateTimeService.Object, new MapService());

            await controller.GetAllCountries().ToListAsync();
            
            cacheService.Verify(c => c.SetSingleEntryWithExpirationAt(It.IsAny<string>(), 
                It.IsAny<IEnumerable<Country>>(), 
                It.Is<DateTime>(dt => dt == now.Add(maxAge))));
        }

        [Fact]
        public async Task GetAllCountries__When_Cache_Entry_Is_Alive__Then__Does_Not_Call_Rest_Countries_Api()
        {
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(d => d.GetNow()).Returns(DateTime.Now);
            Country[] countries = GetCountryDtos();
            var webClientService = new Mock<IWebClientService>();
            var cacheService = new Mock<ICacheService>();
            cacheService.Setup(c => c.GetEntry<IEnumerable<Country>>(It.IsAny<string>()))
                .Returns(new CacheEntry<IEnumerable<Country>>(true, countries));
            var controller = new CountryController(webClientService.Object, cacheService.Object, dateTimeService.Object, new MapService());

            var results = await controller.GetAllCountries().ToListAsync();

            results.Should().NotBeEmpty();
            foreach (var result in results)
            {
                countries.Any(c => c.Name == result.Name).Should().BeTrue();
            }
            webClientService.Verify(_ => _.GetResponseAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetAllCountries__When_Rest_Countries_Fails__Then__Throw_Countries_Exception()
        {
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(d => d.GetNow()).Returns(DateTime.Now);
            var webClientService = new Mock<IWebClientService>();
            var maxAge = TimeSpan.FromMinutes(5);
            var response = CreateResponse(HttpStatusCode.Forbidden, string.Empty);
            response.Headers.Add("cache-control", new [] { $"public, max-age={maxAge.TotalSeconds}"});
            webClientService.Setup(w => w.GetResponseAsync(It.IsAny<string>()))
                .ReturnsAsync(response);
            var cacheService = new Mock<ICacheService>();
            cacheService.Setup(c => c.GetEntry<IEnumerable<Country>>(It.IsAny<string>()))
                .Returns(new CacheEntry<IEnumerable<Country>>(false, null));
            var controller = new CountryController(webClientService.Object, cacheService.Object, dateTimeService.Object, new MapService());

            Func<Task> task = async () => await controller.GetAllCountries().ToListAsync();
            
            task.Should().Throw<CountryException>();
        }

        private static Dtos.Country[] GetCountryDtos() => new[]
            {
                new Country()
                {
                    Name = "Afghanistan",
                    Population = 27657145,
                    Timezones = new [] {"UTC+04:30"},
                    Currencies = new []
                    {
                        new Dtos.Currency()
                        {
                            Code = "AFN",
                            Name = "Afghan afghani",
                            Symbol = "؋",
                        }
                    },
                    Capital = "Kabul",
                    Borders = new []
                    {
                        "IRN",
                        "PAK",
                        "TKM",
                        "UZB",
                        "TJK",
                        "CHN"
                    },
                    Flag = "https://restcountries.eu/data/afg.svg"
                },
                new Country()
                {
                    Name = "Åland Islands",
                    Population = 28875,
                    Timezones = new [] {"UTC+02:00"},
                    Currencies = new []
                    {
                        new Dtos.Currency()
                        {
                            Code = "EUR",
                            Name = "Euro",
                            Symbol = "€",
                        }
                    },
                    Capital = "Mariehamn",
                    Borders = Enumerable.Empty<string>(),
                    Flag = "https://restcountries.eu/data/ala.svg"
                }
            };

        private HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string jsonContent)
        {
            var response = new HttpResponseMessage(statusCode);
            response.Content = new StringContent(jsonContent);
            return response;
        }
    }
}