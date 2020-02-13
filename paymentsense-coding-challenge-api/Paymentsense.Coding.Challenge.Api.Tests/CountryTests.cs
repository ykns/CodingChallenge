using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Paymentsense.Coding.Challenge.Api.Dtos;
using Paymentsense.Coding.Challenge.Api.Extensions;
using Paymentsense.Coding.Challenge.Api.Tests.Helpers;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests
{
    public class CountryTests
    {
        [Fact]
        public async Task GetAllCountries__Then__Returns_Countries()
        {
            var response = await TestHarness.CreateClient().GetAsync("/country");

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            var stream = await response.Content.ReadAsStreamAsync();
            var countries = await stream.ReadAsAsync<IEnumerable<Responses.Country>>();
            countries.Should().NotBeEmpty();
        }
    }
}