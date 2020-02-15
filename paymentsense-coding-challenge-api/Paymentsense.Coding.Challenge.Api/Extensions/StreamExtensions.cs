using System.IO;
using System.Threading.Tasks;

namespace Paymentsense.Coding.Challenge.Api.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this Stream stream) 
        {
            var sr = new StreamReader(stream);
            var objectSerialized = await sr.ReadToEndAsync();
            var result = objectSerialized.DeserialiseAs<T>();
            return result;        
        }
    }
}