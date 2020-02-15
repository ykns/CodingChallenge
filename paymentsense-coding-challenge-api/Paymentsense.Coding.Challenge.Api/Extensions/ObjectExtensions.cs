using System.Text.Json;

namespace Paymentsense.Coding.Challenge.Api.Extensions
{
    public static class ObjectExtensions
    {
        public static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions
        {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
        };
        
        public static string Serialize(this object o)
        {
            var result = JsonSerializer.Serialize(o, SerializerOptions);
            return result;
        }
    }
}