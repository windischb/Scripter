using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scripter.Shared;

namespace Scripter.Engine.TypeScript
{
    [ScripterModule(OnlyTypeDefinition = true)]
    public class BaseTypeDefinition: IScripterTypeDeclaration, IScripterModule<BaseTypeDefinition>
    {
        public string Language => "TypeScript";
        public string FileImport => "BaseTypes";
        public string GetImports()
        {
            return null;
        }

        public string GetTypeDefinitions()
        {

            var es5 = GetFromResources("lib.es5.ts");
            var es2015_core = GetFromResources("lib.es2015.core.d.ts");

            return string.Join(Environment.NewLine, es5, es2015_core);

        }

        public static string GetFromResources(string resourceName)
        {
            var type = typeof(BaseTypeDefinition);

            using (Stream stream = type.Assembly.GetManifestResourceStream($"{type.Namespace}.{resourceName}"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }

            }
        }
    }
}
