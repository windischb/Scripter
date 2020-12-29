using System;
using System.Collections.Generic;
using System.Text;

namespace Scripter.Shared
{
    public class ScripterModuleAttribute : Attribute
    {
        public string Name { get; set; }

        public string[] Tags { get; set; }
        
        public ScripterModuleAttribute(params string[] tags )
        {
            Tags = tags;
        }
    }
}
