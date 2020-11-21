using Scripter.Engine.JavaScript;
using Scripter.Shared;

namespace Scripter.Engine.TypeScript
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddTypeScriptEngine(this IScripterContext scripterContext)
        {
            
            return scripterContext
                .AddJavaScriptEngine()
                .AddScripterEngine<TypeScriptEngine>("TypeScript");
        }
    }
}
