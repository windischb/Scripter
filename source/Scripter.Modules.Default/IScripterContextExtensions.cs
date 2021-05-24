using doob.Scripter.Module.ConsoleWriter;
using doob.Scripter.Module.Http;
using doob.Scripter.Module.Smtp;
using doob.Scripter.Module.Template;
using doob.Scripter.Shared;

namespace Scripter.Modules.Default
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddDefaultScripterModules(this IScripterContext services)
        {
            return services
                .AddScripterModule<ConsoleWriterModule>()
                .AddScripterModule<HttpModule>()
                .AddScripterModule<SmtpModule>()
                .AddScripterModule<TemplateModule>();
        }
    }
}
