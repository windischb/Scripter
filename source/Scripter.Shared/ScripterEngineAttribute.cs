using System;

namespace Scripter.Shared
{
    public class ScripterEngineAttribute : Attribute
    {
        public string Name { get; }
        public ScripterEngineAttribute(string name)
        {
            Name = name;
        }
    }
}