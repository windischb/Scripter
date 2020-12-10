

declare namespace Scripter {

    namespace Module {

        namespace Http {

            interface HttpOptionsBuilder {

                UseProxy(proxy: string): HttpOptionsBuilder;
                UseProxy(proxy: string, credentials: System.Net.NetworkCredential): HttpOptionsBuilder;
                IgnoreProxy(value?: boolean): HttpOptionsBuilder;
                AddClientCertificate(bytes: Uint8Array): HttpOptionsBuilder;
                AddClientCertificate(bytes: Uint8Array, password: string): HttpOptionsBuilder;
            }

            interface HttpRequestBuilder {
                UsePath(url: string): HttpRequestBuilder;
                AddHeader(key: string, ...value: string[]): HttpRequestBuilder;
                SetHeader(key: string, ...value: string[]): HttpRequestBuilder;
                SetContentType(vlue: string): HttpRequestBuilder;
                AddQueryParam(key: string, ...value: string[]): HttpRequestBuilder;
                SetQueryParam(key: string, ...value: string[]): HttpRequestBuilder;
                SetBearerToken(token: string): HttpRequestBuilder;
                SetBasicAuthentication(username: string, password: string): HttpRequestBuilder;

                Send(method: string, body?: any): HttpResponse;
                Get(): HttpResponse;
                Post(body: any): HttpResponse
                Put(body: any): HttpResponse;
                Patch(body: any): HttpResponse;
                Delete(body?: any): HttpResponse;

            }

            interface HttpResponse {
                Version: string;
                Content: GenericHttpContent;
                ContentHeaders: any;
                StatusCode: any;
                ReasonPhrase: string;
                Headers: any;
                TrailingHeaders: any;
                IsSuccessStatusCode: boolean;
                EnsureSuccessStatusCode(): HttpResponse;

            }

            interface GenericHttpContent {
                Type: string;
                IsArray: boolean;
                AsText(): string;
                AsObject(): any;
                AsArray(): Array<any>;
            }

        }
    }


}