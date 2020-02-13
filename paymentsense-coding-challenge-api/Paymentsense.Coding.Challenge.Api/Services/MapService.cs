using System.Linq;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public interface IMapService
    {
        Responses.Country Map(Dtos.Country dto);
    }

    public class MapService : IMapService
    {
        public Responses.Country Map(Dtos.Country dto)
        {
            var country = new Responses.Country()
            {
                Name = dto.Name,
                Population = dto.Population,
                Timezones = string.Join(",", dto.Timezones),
                Currencies = string.Join(",", dto.Currencies.Select(_ => _.Name)),
                CapitalCity = dto.Capital,
                BorderingCountries = string.Join(",", dto.Borders),
                FlagUri = dto.Flag,
            };
            return country;
        }
    }
}