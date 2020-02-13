using System.Collections.Generic;

namespace Paymentsense.Coding.Challenge.Api.Dtos
{
    public class Currency 
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public Currency()
        {
            
        }
    }
    public class Country 
    {
        // population, time zones, currencies, language, capital city and bordering countries
        public string Name { get; set; }
        public int Population { get; set; }
        public IEnumerable<string> Timezones { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
        public string Capital { get; set; }
        public IEnumerable<string> Borders { get; set; }
        public string Flag { get; set; }
        public Country()
        {
            
        }
    }
}