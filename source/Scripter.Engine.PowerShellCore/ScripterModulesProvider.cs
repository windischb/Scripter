using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Scripter.Shared;

namespace Scripter.Engine.PowerShellCore
{
    public class ScripterModulesProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IScriptEngine _scriptEngine;
        private readonly Dictionary<Type, Func<object>> _providedTypeFactories;


        public ScripterModulesProvider(IServiceProvider serviceProvider, IScriptEngine scriptEngine, Dictionary<Type, Func<object>> providedTypeFactories)
        {
            _serviceProvider = serviceProvider;
            _scriptEngine = scriptEngine;
            _providedTypeFactories = providedTypeFactories;
        }

        public object GetModule(string moduleName)
        {
            var scripterModuleRegistry = _serviceProvider.GetRequiredService<IScripterModuleRegistry>();
            var inst = scripterModuleRegistry.BuildModuleInstance(moduleName, _serviceProvider, _scriptEngine, _providedTypeFactories);

            return inst;
        }
    }
}
