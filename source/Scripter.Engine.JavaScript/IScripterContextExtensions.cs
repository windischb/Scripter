using doob.Scripter.Shared;

namespace doob.Scripter.Engine.Javascript
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddJavaScriptEngine(this IScripterContext services)
        {
            return services.AddScripterEngine<JavaScriptEngine>();
        }
    }
}
