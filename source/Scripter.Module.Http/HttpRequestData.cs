﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using doob.Reflectensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace doob.Scripter.Module.Http
{
    public class HttpRequestData
    {
        public ConcurrentDictionary<string, StringValues> QueryParameters { get; } = new ConcurrentDictionary<string, StringValues>();
        public ConcurrentDictionary<string, List<string>> Headers { get; } = new ConcurrentDictionary<string, List<string>>();

        public string? ContentType { get; set; }

        public string? Path { get; set; }

        public HttpRequestMessage BuildHttpRequestMessage(HttpHandlerOptions httpHandlerOptions, HttpMethod httpMethod, object? content)
        {


            var requestUri = String.IsNullOrWhiteSpace(Path)
                ? httpHandlerOptions.RequestUri
                : new Uri(httpHandlerOptions.RequestUri, Path);

            

            if(QueryParameters.Any())
            {
                
                var qb = new QueryBuilder();
                if (!String.IsNullOrWhiteSpace(requestUri.Query))
                {
                    var query = QueryHelpers.ParseQuery(requestUri.Query);
                    foreach (var kv in query)
                    {
                        qb.Add(kv.Key, kv.Value.ToArray());
                    }
                }
                foreach (var kv in QueryParameters)
                {
                    qb.Add(kv.Key, kv.Value.ToArray());
                }

                var uriBuilder = new UriBuilder(requestUri);
                uriBuilder.Query = qb.ToQueryString().ToString();

                requestUri = uriBuilder.Uri;

            }

           

            var message = new HttpRequestMessage(httpMethod, requestUri);

            if (content != null)
            {
                message.Content = CreateHttpContent(content);
            }

            if (Headers.Any())
            {
                foreach (var kv in Headers)
                {
                    if (message.Headers.Contains(kv.Key))
                    {
                        message.Headers.Remove(kv.Key);
                    }
                    message.Headers.Add(kv.Key, kv.Value);

                }
            }


            if (!String.IsNullOrWhiteSpace(ContentType) && message.Content != null)
            {
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            }

            return message;
        }


        private HttpContent? CreateHttpContent(object? content)
        {
            

            if (content == null)
                return null;

            var ms = new MemoryStream();
            HttpContent httpContent;

            if (content is string str)
            {
                httpContent = new StringContent(str, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return httpContent;
            }

            CreateJsonHttpContent(content, ms);
            ms.Seek(0, SeekOrigin.Begin);
            httpContent = new StreamContent(ms);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpContent;
        }

        private void CreateJsonHttpContent(object content, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                Json.Converter.JsonSerializer.Serialize(sw, content);
                
            }

        }
    }
}