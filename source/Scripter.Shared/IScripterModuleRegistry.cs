using System;
using System.Collections.Generic;

namespace Scripter.Shared
{
    public interface IScripterModuleRegistry
    {
        IScripterModule BuildModuleInstance(string name, IServiceProvider serviceProvider,
            IScriptEngine currentScriptEngine, Dictionary<Type, Func<object>> instanceDictionary = null);
    }
}