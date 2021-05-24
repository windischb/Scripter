using doob.Scripter.Engine.Javascript;
using doob.Scripter.Shared;

namespace doob.Scripter.Engine.TypeScript
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
