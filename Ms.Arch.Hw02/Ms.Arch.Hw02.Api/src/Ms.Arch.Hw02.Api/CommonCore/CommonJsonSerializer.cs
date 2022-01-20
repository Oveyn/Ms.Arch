using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ms.Arch.Hw02.Api.CommonCore
{
    internal static class CommonJsonSerializer
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(),
            }
        };

        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }
    }
}
