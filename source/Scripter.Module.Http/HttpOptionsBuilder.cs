﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Scripter.Module.Http
{
    public class HttpOptionsBuilder
    {
        private readonly HttpHandlerOptions _httpHandlerOptions;

        public HttpOptionsBuilder(string url)
        {
            _httpHandlerOptions = new HttpHandlerOptions(UriHelper.BuildUri(url));
        }
        

        public HttpOptionsBuilder UseProxy(WebProxy proxy)
        {
            _httpHandlerOptions.IgnoreProxy = false;
            _httpHandlerOptions.Proxy = proxy;
            return this;
        }

        public HttpOptionsBuilder UseProxy(Uri proxy)
        {
            return UseProxy(new WebProxy(proxy));
        }

        public HttpOptionsBuilder UseProxy(Uri proxy, ICredentials credentials)
        {
            var webProxy = new WebProxy(proxy)
            {
                Credentials = credentials
            };
            return UseProxy(webProxy);
        }

        //public HttpOptionsBuilder UseProxy(Uri proxy, SimpleCredentials credentials)
        //{
        //    return UseProxy(proxy, (NetworkCredential)credentials);
        //}


        public HttpOptionsBuilder UseProxy(string proxy)
        {
            var uri = UriHelper.BuildUri(proxy);
            return UseProxy(uri);
        }

        public HttpOptionsBuilder UseProxy(string proxy, ICredentials credentials)
        {
            var uri = UriHelper.BuildUri(proxy);
            return UseProxy(uri, credentials);
        }

        //public HttpOptionsBuilder UseProxy(string proxy, SimpleCredentials credentials)
        //{
        //    var uri = UriHelper.BuildUri(proxy);
        //    return UseProxy(uri, (NetworkCredential)credentials);
        //}

        public HttpOptionsBuilder IgnoreProxy()
        {
            return IgnoreProxy(true);
        }

        public HttpOptionsBuilder IgnoreProxy(bool value)
        {
            _httpHandlerOptions.IgnoreProxy = value;
            return this;
        }

        public HttpOptionsBuilder AddClientCertificate(byte[] bytes)
        {
            var cert = new X509Certificate2(bytes);

            _httpHandlerOptions.ClientCertificates.Add(cert);
            return this;
        }

        public HttpOptionsBuilder AddClientCertificate(byte[] bytes, string password)
        {
            var cert = new X509Certificate2(bytes, password);
            _httpHandlerOptions.ClientCertificates.Add(cert);
            return this;
        }


        public static implicit operator HttpHandlerOptions(HttpOptionsBuilder optionsOptionsBuilder)
        {
            return optionsOptionsBuilder._httpHandlerOptions;
        }
    }
}