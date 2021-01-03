using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Scripter.Module.Http
{
    public static class HttpHandlerFactory
    {
        private static readonly ConcurrentDictionary<ReadonlyHttpHandlerOptions, HttpMessageHandler> HttpHandlers = new ConcurrentDictionary<ReadonlyHttpHandlerOptions, HttpMessageHandler>();

        public static HttpMessageHandler Build(HttpHandlerOptions handlerOptions)
        {
            var roHttpHandlerOptions = new ReadonlyHttpHandlerOptions(handlerOptions);
            return HttpHandlers.GetOrAdd(roHttpHandlerOptions, _ => ValueFactory(handlerOptions));
        }

        private static HttpMessageHandler ValueFactory(HttpHandlerOptions handlerOptions)
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(60),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(20),
                MaxConnectionsPerServer = 2
            };

            if (handlerOptions.Proxy != null)
            {
                socketsHandler.Proxy = handlerOptions.Proxy;
            }

            if (handlerOptions.IgnoreProxy)
            {
                socketsHandler.UseProxy = false;
            }

            if (handlerOptions.ClientCertificates != null)
            {
                foreach (var handlerOptionsClientCertificate in handlerOptions.ClientCertificates)
                {
                    if (socketsHandler.SslOptions.ClientCertificates == null)
                    {
                        socketsHandler.SslOptions.ClientCertificates = new X509CertificateCollection();
                    }
                    socketsHandler.SslOptions.ClientCertificates.Add(handlerOptionsClientCertificate);
                }
            }
           

            return socketsHandler;

        }

    }
}