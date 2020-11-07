using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using Scripter.HttpModule.ExtensionMethods;

namespace Scripter.HttpModule
{
    public class HttpResponse : IDisposable
    {

        private readonly HttpResponseMessage _httpResponseMessage;

        public Version Version => _httpResponseMessage.Version;

        private GenericHttpContent _content;
        public GenericHttpContent Content => _content = _content ?? new GenericHttpContent(_httpResponseMessage.Content);

        private ExpandoObject _contentHeaders;
        public ExpandoObject ContentHeaders => _contentHeaders = _contentHeaders ?? _httpResponseMessage.Content?.Headers?.ToExpandoObject();

        public HttpStatusCode StatusCode => _httpResponseMessage.StatusCode;
        public string ReasonPhrase => _httpResponseMessage.ReasonPhrase;

        private ExpandoObject _headers;
        public ExpandoObject Headers => _headers = _headers ?? _httpResponseMessage.Headers.ToExpandoObject();

#if NETCOREAPP3_1
        private ExpandoObject _trailingheaders;
        public ExpandoObject TrailingHeaders => _trailingheaders = _trailingheaders ?? _httpResponseMessage.TrailingHeaders.ToExpandoObject();
#endif
        public bool IsSuccessStatusCode => _httpResponseMessage.IsSuccessStatusCode;
        public HttpResponse EnsureSuccessStatusCode()
        {
            _httpResponseMessage.EnsureSuccessStatusCode();
            return this;
        }

        public override string ToString() => _httpResponseMessage.ToString();


        internal HttpResponse(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
            //Content = new GenericHttpContent(_httpResponseMessage.Content);
        }


        public void Dispose()
        {
            _httpResponseMessage?.Dispose();
        }

        
    }
}