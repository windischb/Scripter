using System;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx.Synchronous;
using Reflectensions;

namespace Scripter.Module.Http
{
    public class GenericHttpContent : IDisposable
    {

        private readonly HttpContent _httpContent;

        private readonly string _text;

        private JToken _jToken;

        public string Type { get; set; }

        public bool IsArray
        {
            get
            {
                switch (Type)
                {
                    case "json":
                    {
                        return _jToken.Type == JTokenType.Array;
                    }
                }

                return false;
            }
        }

        public GenericHttpContent(HttpContent httpContent)
        {
            _httpContent = httpContent;
            _text = _text ?? _httpContent.ReadAsStringAsync().WaitAndUnwrapException();

            ProcessContent();

        }


        private void ProcessContent()
        {
            switch (_httpContent.Headers.ContentType.MediaType)
            {
                case "application/json":
                {
                    Type = "json";
                    _jToken = Json.Converter.ToJToken(_text);
                    break;
                }

                case "application/xml":
                {
                    Type = "xml";
                    break;
                }

            }
        }

        public string AsText()
        {
            return _text;
        }

        public object AsObject()
        {
            switch (Type)
            {
                case "json":
                {
                    return Converter.Json.ToObject<ExpandoObject>(_jToken);
                }

            }

            throw new NotImplementedException();
        }

        public object[] AsArray()
        {
            switch (Type)
            {
                case "json":
                {
                    return Json.Converter.ToBasicDotNetObjectEnumerable(_jToken as JArray).ToArray();
                }

            }
            throw new NotImplementedException();
        }
        
        

        public void Dispose()
        {
            _httpContent?.Dispose();
        }
    }
}