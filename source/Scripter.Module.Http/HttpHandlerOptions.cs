using System;
using System.Net;

namespace Scripter.Module.Http
{
    public class HttpHandlerOptions
    {

        public Uri RequestUri { get; }
        public WebProxy Proxy { get; set; }
        public bool IgnoreProxy { get; set; }

        public HttpHandlerOptions(Uri uri)
        {
            RequestUri = uri;
        }

    }
}