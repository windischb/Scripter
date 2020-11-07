using System;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using Utf8Json;

namespace HttpModule.ExtensionMethods
{
    public static class ObjectExtensions
    {
        
        public static ExpandoObject ToExpandoObject(this object @object)
        {
            return JsonSerializer.Deserialize<ExpandoObject>(JsonSerializer.Serialize(@object));
        }

        public static ExpandoObject ToExpandoObject(this HttpHeaders headers)
        {
            return headers.ToDictionary(h => h.Key, h => String.Join("; ", h.Value)).ToExpandoObject();
        }
    }
}
