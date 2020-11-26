﻿using System;

namespace Scripter.Shared
{
    public interface IScripterModuleRegistry
    {
        IScripterModuleDefinition GetModule(string name);

        IScripterModule BuildModuleInstance(string name, IServiceProvider serviceProvider,
            IScriptEngine currentScriptEngine);
    }
}