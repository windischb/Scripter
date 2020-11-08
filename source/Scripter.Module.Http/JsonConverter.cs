using System;
using Reflectensions.JsonConverters;

namespace Scripter.Module.Http
{
    public class Converter
    {
        private static readonly Lazy<Reflectensions.Json> lazyJson = new Lazy<Reflectensions.Json>(() => new Reflectensions.Json()
            .RegisterJsonConverter<ExpandoObjectConverter>(0)
        );

        public static Reflectensions.Json Json => lazyJson.Value;

    }
}
