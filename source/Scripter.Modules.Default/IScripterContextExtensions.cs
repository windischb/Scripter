using Scripter.Module.ConsoleWriter;
using Scripter.Module.Http;
using Scripter.Module.Smtp;
using Scripter.Module.Template;
using Scripter.Shared;

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
