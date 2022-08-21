using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mnemo.Tests.Utils
{
    public static class JsonProvider
    {
        private static readonly Lazy<JsonSerializerSettings> _settings = new Lazy<JsonSerializerSettings>(() =>
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            return settings;
        }, true);

        public static string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, _settings.Value);
    }
}
