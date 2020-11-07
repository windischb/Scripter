using System;

namespace Scripter.Shared
{
    public class ScripterModuleAttribute : Attribute
    {
        public string Name { get; }
        public ScripterModuleAttribute(string name)
        {
            Name = name;
        }
    }
}