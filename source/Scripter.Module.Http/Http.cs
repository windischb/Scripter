using System;
using Scripter.Shared;

namespace Scripter.Module.Http
{
    
    public class HttpModule: IScripterModule<HttpModuleTypeDefinition>
    {

        public HttpRequestBuilder Client(string url, Action<HttpOptionsBuilder> options)
        {
            var builder = new HttpOptionsBuilder(url);
            options?.Invoke(builder);
            
            return new HttpRequestBuilder(builder);
        }

        public HttpRequestBuilder Client(string url, HttpHandlerOptions options)
        {
            return new HttpRequestBuilder(options);
        }


        public HttpRequestBuilder Client(string url)
        {
            var builder = new HttpOptionsBuilder(url);
            return new HttpRequestBuilder(builder);
        }

    }
}
