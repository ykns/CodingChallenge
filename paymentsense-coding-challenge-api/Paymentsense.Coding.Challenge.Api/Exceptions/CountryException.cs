using System;

namespace Paymentsense.Coding.Challenge.Api.Exceptions
{
    public class CountryException : Exception
    {
        public CountryException(string message) : base(message)
        {
            
        }

        public CountryException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}