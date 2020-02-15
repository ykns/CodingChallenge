using System.Text.Json;

namespace Paymentsense.Coding.Challenge.Api.Extensions
{
    public static class StringExtensions
    {
        public static T DeserialiseAs<T>(this string s) 
        {
            var result = JsonSerializer.Deserialize<T>(s, ObjectExtensions.SerializerOptions);
            return result;
        }
    }
}