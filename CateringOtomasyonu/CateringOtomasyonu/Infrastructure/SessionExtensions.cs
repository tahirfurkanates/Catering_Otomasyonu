using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace CateringOtomasyonu.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
            => session.SetString(key, JsonSerializer.Serialize(value));

        public static T? GetObject<T>(this ISession session, string key)
            => session.GetString(key) is { } json ? JsonSerializer.Deserialize<T>(json) : default;
    }
}
