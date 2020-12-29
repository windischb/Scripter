using System;
using System.Collections.Generic;
using System.Text;

namespace Scripter.Shared
{
    public interface IScripterModuleDefinition
    {

        List<string> Tags { get; }

        Type ModuleType { get; }
    }
}
