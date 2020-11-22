using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scripter.Shared;

namespace Scripter.Module.Http
{
    public class HttpModuleTypeDefinition: ScripterTypeDefinition
    {
        public override string GetImports()
        {
            return GetFromResources<HttpModuleTypeDefinition>("http.ts");
        }

        public override string GetTypeDefinitions()
        {
            return GetFromResources<HttpModuleTypeDefinition>("http.d.ts");
        }
    }
}
