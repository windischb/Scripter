using Scripter.Shared;

namespace Scripter.Engine.JavaScript
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddJavaScriptEngine(this IScripterContext services)
        {
            return services.AddScripterEngine<JavaScriptEngine>("JavaScript");
        }
    }
}
