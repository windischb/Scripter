

export declare function Client(url: string): Scripter.Module.Http.HttpRequestBuilder;
export declare function Client(url: string, options: ((builder: Scripter.Module.Http.HttpOptionsBuilder) => Scripter.Module.Http.HttpOptionsBuilder)): Scripter.Module.Http.HttpRequestBuilder;

declare namespace Scripter {

    namespace Module {

        namespace Http {

            export class HttpOptionsBuilder {

                UseProxy(proxy: string): HttpOptionsBuilder;
                UseProxy(proxy: string, credentials: Scripter.Module.HelperClasses.SimpleCredentials): HttpOptionsBuilder;
                IgnoreProxy(value?: boolean): HttpOptionsBuilder;
            }

            export class HttpRequestBuilder {
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

            export class HttpResponse {
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

            export class GenericHttpContent {
                Type: string;
                IsArray: boolean;
                AsText(): string;
                AsObject(): any;
                AsArray(): Array<any>;
            }

        }
    }


}