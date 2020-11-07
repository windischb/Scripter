using Microsoft.Extensions.DependencyInjection;
using Scripter.Shared;

namespace Scripter.JavaScript
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddJavaScriptEngine(this IScripterContext services)
        {
            return services.AddScripterEngine<JavaScriptEngine>();
        }
    }
}
