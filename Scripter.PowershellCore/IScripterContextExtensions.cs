using Scripter.Shared;

namespace Scripter.PowershellCore
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddPowerShellCoreEngine(this IScripterContext services)
        {
            return services.AddScripterEngine<PowerShellCoreEngine>();
        }
    }
}
