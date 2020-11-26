using System;
using System.Collections.Generic;
using System.Text;
using Scripter.Shared;

namespace Scripter.Module.Smtp
{
    public class TypeDefinitions: ScripterTypeDefinition
    {
        public override string GetTypeDefinitions()
        {
            return GetFromResources<TypeDefinitions>("typings.d.ts");
        }

        public override string GetImports()
        {
            return GetFromResources<TypeDefinitions>("imports.d.ts");
        }
    }
}
