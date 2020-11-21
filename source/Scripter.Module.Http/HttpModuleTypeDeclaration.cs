using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scripter.Shared;

namespace Scripter.Module.Http
{
    public class HttpModuleTypeDeclaration: IScripterTypeDeclaration
    {
        public string Language => "TypeScript";
        public string FileImport => "Http";

        public string GetImports()
        {
            return GetFromResources("http.ts");
        }

        public string GetTypeDefinitions()
        {
            return null;
        }


        public static string GetFromResources(string resourceName)
        {
            var type = typeof(HttpModuleTypeDeclaration);

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
