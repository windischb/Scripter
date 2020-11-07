using Scripter.JavaScript;
using Scripter.Shared;

namespace Scripter.TypeScript
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddTypeScriptEngine(this IScripterContext scripterContext)
        {
            
            return scripterContext
                .AddJavaScriptEngine()
                .AddScripterEngine<TypeScriptEngine>();
        }
    }
}
