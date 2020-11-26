using System;
using System.Collections.Generic;
using System.Text;
using Scripter.Shared;

namespace Scripter.Module.Common
{
    public class CommonModuleTypeDefinitions : ScripterTypeDefinition
    {
        public override string GetImports()
        {
            return GetFromResources<CommonModuleTypeDefinitions>("imports.d.ts");
        }

        public override string GetTypeDefinitions()
        {
            return GetFromResources<CommonModuleTypeDefinitions>("typings.d.ts");
        }

    }
}
