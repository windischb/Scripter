using System;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Scripter.Shared;

namespace Scripter.PowerShellCore
{
    public class ScripterModulesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ScripterModulesProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetModule(string moduleName)
        {
            return _serviceProvider.GetRequiredNamedService<IScripterModule>(moduleName);
        }
    }
}
