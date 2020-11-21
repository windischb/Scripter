using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scripter.Shared;

namespace Scripter.Module.Http
{
    public class HttpModuleTypeDefinition: ScripterTypeDefinition
    {
        public string Language => "TypeScript";
       
        public override string GetImports()
        {
            return GetFromResources("http.ts");
        }
        

        public static string GetFromResources(string resourceName)
        {
            var type = typeof(HttpModuleTypeDefinition);

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
