﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Scripter.Module.Http
{
    public class HttpHandlerOptions
    {

        public Uri RequestUri { get; }
        public WebProxy Proxy { get; set; }
        public bool IgnoreProxy { get; set; }

        public List<X509Certificate2> ClientCertificates { get; set; } = new List<X509Certificate2>();

        public HttpHandlerOptions(Uri uri)
        {
            RequestUri = uri;
        }

    }
}