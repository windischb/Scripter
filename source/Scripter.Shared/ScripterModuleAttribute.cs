using System;

namespace Scripter.Shared
{
    public class ScripterModuleAttribute : Attribute
    {
        public string Name { get; }

        public bool OnlyTypeDefinition { get; set; }

        public ScripterModuleAttribute()
        {
            
        }

        public ScripterModuleAttribute(string name)
        {
            Name = name;
        }
    }
}