using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scripter.Shared;

namespace Scripter.Engine.TypeScript
{
    [ScripterModule(OnlyTypeDefinition = true)]
    public class BaseTypeDefinition: ScripterTypeDefinition, IScripterModule<BaseTypeDefinition>
    {
        public override string Language => "TypeScript";

        public override string GetTypeDefinitions()
        {

            var es5 = GetFromResources<BaseTypeDefinition>("lib.es5.ts");
            var es2015_core = GetFromResources<BaseTypeDefinition>("lib.es2015.core.d.ts");

            return string.Join(Environment.NewLine, es5, es2015_core);

        }

    }
}
