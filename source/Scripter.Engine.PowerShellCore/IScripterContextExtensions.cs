using Scripter.Shared;

namespace Scripter.Engine.PowerShellCore
{
    public static class IScripterContextExtensions
    {
        public static IScripterContext AddPowerShellCoreEngine(this IScripterContext services)
        {
            return services.AddScripterEngine<PowerShellCoreEngine>("PowerShellCore");
        }
    }
}
