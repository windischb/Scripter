using System;
using System.Collections.Generic;
using System.Text;
using Scripter.Shared;

namespace Scripter
{
    public class ScripterModuleDefinition: IScripterModuleDefinition
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public Type ModuleType { get; }

        public ScripterModuleDefinition(string name, Type moduleType)
        {
            Name = name;
            ModuleType = moduleType;
        }
    }
}
