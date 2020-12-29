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
        private readonly List<string> _useTaggedModules;
        internal Dictionary<Type, object> _instantiatedModules = new Dictionary<Type, object>();

        public ScripterModulesProvider(IServiceProvider serviceProvider, IScriptEngine scriptEngine,
            Dictionary<Type, Func<object>> providedTypeFactories, List<string> useTaggedModules)
        {
            _serviceProvider = serviceProvider;
            _scriptEngine = scriptEngine;
            _providedTypeFactories = providedTypeFactories;
            _useTaggedModules = useTaggedModules;
        }

        public object GetModule(string moduleName)
        {
            var scripterModuleRegistry = _serviceProvider.GetRequiredService<IScripterModuleRegistry>();
            var inst = scripterModuleRegistry.BuildModuleInstance(moduleName, _serviceProvider, _scriptEngine, _providedTypeFactories, _useTaggedModules);
            _instantiatedModules[inst.GetType()] = inst;
            return inst;
        }
    }
}
