using System;
using System.Collections.Generic;
using Scripter.Shared;

namespace Scripter
{
    public interface IScripterModuleDefinition
    {
        Type ModuleType { get; }
        List<ScripterTypeDefinition> TypeDefinitions { get; }
    }
}