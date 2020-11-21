using System;
using System.Collections.Generic;
using System.Text;
using Scripter.Shared;

namespace Scripter.Module.Common
{
    public class CommonModuleTypeDefinitions : IScripterTypeDeclaration
    {
        public string Language => "TypeScript";
        public string FileImport => "Common";
        public string GetImports()
        {
            return null;
        }

        public string GetTypeDefinitions()
        {
            return null;
        }
    }
}
