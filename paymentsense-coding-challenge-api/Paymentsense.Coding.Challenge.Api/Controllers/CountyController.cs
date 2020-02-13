using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Paymentsense.Coding.Challenge.Api.Dtos;
using Paymentsense.Coding.Challenge.Api.Exceptions;
using Paymentsense.Coding.Challenge.Api.Extensions;
using Paymentsense.Coding.Challenge.Api.Services;

namespace Paymentsense.Coding.Challenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IWebClientService webClientService;
        private readonly ICacheService cacheService;
        private readonly IDateTimeService dateTimeService;
        private readonly IMapService mapService;
        public CountryController(IWebClientService webClientService, ICacheService cacheService, IDateTimeService dateTimeService, IMapService mapService)
        {
            this.mapService = mapService;
            this.cacheService = cacheService;
            this.webClientService = webClientService;
            this.dateTimeService = dateTimeService;
        }

        [HttpGet]
        public async IAsyncEnumerable<Responses.Country> GetAllCountries()
        {
            // TODO consider move uri to config
            var requestUri = "https://restcountries.eu/rest/v2/all";
            var cachedEntry = cacheService.GetEntry<IEnumerable<Country>>(requestUri);
            var countries = default(IEnumerable<Country>);

            if (cachedEntry.IsAlive)
            {
                countries = cachedEntry.Value;
            }
            else
            {
                var absoluteExpiration = this.dateTimeService.GetNow();
                var countriesResponse = await webClientService.GetResponseAsync(requestUri);
                if (countriesResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new CountryException("Unable to acquire countries from dependencies");
                }

                var countriesStream = await countriesResponse.Content.ReadAsStreamAsync();
                countries = await countriesStream.ReadAsAsync<IEnumerable<Country>>();

                if (countriesResponse.Headers.CacheControl != null
                    && countriesResponse.Headers.CacheControl.MaxAge.HasValue)
                {
                    absoluteExpiration = absoluteExpiration.Add(countriesResponse.Headers.CacheControl.MaxAge.Value);
                    cacheService.SetSingleEntryWithExpirationAt(requestUri, countries, absoluteExpiration);
                }
            }

            foreach (var country in countries)
            {
                yield return this.mapService.Map(country);
            }
        }
    }
}