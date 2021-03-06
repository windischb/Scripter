﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using doob.Reflectensions.Common;
using Microsoft.Extensions.Primitives;
using Nito.AsyncEx.Synchronous;

namespace doob.Scripter.Module.Http
{
    public class HttpRequestBuilder : IHttpRequestBuilder
    {

        private readonly HttpHandlerOptions _httpHandlerOptions;

        private readonly HttpRequestData _requestData = new HttpRequestData();


        public HttpRequestBuilder(HttpHandlerOptions httpHandlerOptions)
        {
            _httpHandlerOptions = httpHandlerOptions;
        }

        public HttpRequestBuilder SetPath(params string[] url)
        {            
            _requestData.PathSegments = url.Where(u => String.IsNullOrEmpty(u)).SelectMany(u => u.Split('/')).ToList();
            return this;
        }

        public HttpRequestBuilder AddPath(params string[] url)
        {
            _requestData.PathSegments.AddRange(url.Where(u => String.IsNullOrEmpty(u)).SelectMany(u => u.Split('/')).ToList());
            return this;
        }


        public HttpRequestBuilder AddHeader(string key, params string[] value)
        {
            if (String.IsNullOrWhiteSpace(key))
                return this;

            key = key.ToLower();

            _requestData.Headers.AddOrUpdate(key, 
                _=> value.ToList(),
                (s, list) =>
                {
                    list.AddRange(value);
                    return list.Distinct().ToList();
                });

            return this;

        }

        public HttpRequestBuilder SetHeader(string key, params string[] value)
        {
            if (String.IsNullOrWhiteSpace(key))
                return this;

            key = key.ToLower();
            _requestData.Headers.TryRemove(key, out var _);
            _requestData.Headers.TryAdd(key, value.ToList());

            return this;
        }

        public HttpRequestBuilder SetContentType(string contentType)
        {
            _requestData.ContentType = contentType;
            return this;
        }

        public HttpRequestBuilder AddQueryParam(string key, params string[] value)
        {
            if (String.IsNullOrWhiteSpace(key))
                return this;

            key = key.ToLower();

            _requestData.QueryParameters.AddOrUpdate(key, s => {
                return new StringValues(value);
            }, (s, sv) => {
                var temp = sv.ToList();
                temp.AddRange(value);
                return new StringValues(temp.ToArray());

            });

            return this;
        }

        public HttpRequestBuilder SetQueryParam(string key, params string[] value)
        {
            if (String.IsNullOrWhiteSpace(key))
                return this;

            key = key.ToLower();

            _requestData.QueryParameters.TryRemove(key, out var _);
            _requestData.QueryParameters.TryAdd(key, new StringValues(value));
            
            return this;
        }

        public HttpRequestBuilder SetBearerToken(string token)
        {
            return SetHeader("authorization", $"Bearer {token}");
        }

        public HttpRequestBuilder SetBasicAuthentication(string username, string password)
        {
            var cred = $"{username}:{password}".EncodeToBase64();
            return SetHeader("authorization", $"Basic {cred}");
        }


        public async Task<HttpResponse> SendRequestMessageAsync(HttpRequestMessage httpRequestMessage)
        {
            var cl = new HttpClient(HttpHandlerFactory.Build(_httpHandlerOptions));
            
            var respMsg = await cl.SendAsync(httpRequestMessage);
            return new HttpResponse(respMsg);
        }

        public Task<HttpResponse> SendAsync(string httpMethod, object? content = null)
        {
            return SendAsync(new HttpMethod(httpMethod), content);
        }

        public async Task<HttpResponse> SendAsync(HttpMethod httpMethod, object? content = null)
        {
            var httpRequestMessage = _requestData.BuildHttpRequestMessage(_httpHandlerOptions, httpMethod, content);
            Console.WriteLine($"SendAsync: {content}");
            try
            {
                return await SendRequestMessageAsync(httpRequestMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("----------------");
                Console.WriteLine(e.StackTrace);
                throw;
            }
            
        }

        public Task<HttpResponse> GetAsync()
        {
            return SendAsync(HttpMethod.Get);
        }

        public Task<HttpResponse> PostAsync(object content)
        {
            return SendAsync(HttpMethod.Post, content);
        }

        public Task<HttpResponse> PutAsync(object content)
        {
            return SendAsync(HttpMethod.Put, content);
        }

        public Task<HttpResponse> PatchAsync(object content)
        {
            return SendAsync("Patch", content);
        }

        
        public Task<HttpResponse> DeleteAsync(object? content)
        {
            return SendAsync(HttpMethod.Delete, content);
        }

        public Task<HttpResponse> DeleteAsync()
        {
            return DeleteAsync(null);
        }

        public HttpResponse Send(string httpMethod, object? content = null)
        {
            return SendAsync(httpMethod, content).WaitAndUnwrapException();
        }

        public HttpResponse Send(HttpMethod httpMethod, object? content = null)
        {
            return SendAsync(httpMethod, content).WaitAndUnwrapException();
        }

        public HttpResponse SendRequestMessage(HttpRequestMessage httpRequestMessage)
        {
            return SendRequestMessageAsync(httpRequestMessage).WaitAndUnwrapException();
        }

        public HttpResponse Get()
        {
            return GetAsync().WaitAndUnwrapException();
        }

        public HttpResponse Post(object content)
        {
            Console.WriteLine($"Post: {content}");
            return PostAsync(content).WaitAndUnwrapException();
        }

        public HttpResponse Put(object content)
        {
            return PutAsync(content).WaitAndUnwrapException();
        }

        public HttpResponse Patch(object content)
        {
            return PatchAsync(content).WaitAndUnwrapException();
        }

        public HttpResponse Delete(object? content)
        {
            return DeleteAsync(content).WaitAndUnwrapException();
        }
        public HttpResponse Delete()
        {
            return Delete(null);
        }

    }

    
}