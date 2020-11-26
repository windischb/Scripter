using System;
using System.IO;
using System.Reflection;

namespace Scripter.Shared
{
    public abstract class ScripterTypeDefinition
    {
        public virtual string Language { get; } = "TypeScript";

        public string FileName { get; set; }

        public virtual string GetImports() => null;
        public virtual string GetTypeDefinitions() => null;

        
        protected static string GetFromResources<T>(string resourceName)
        {
            return GetFromResources(typeof(T), resourceName);
        }
        protected static string GetFromResources(Type namespaceType, string resourceName)
        {

            using (Stream stream = namespaceType.Assembly.GetManifestResourceStream($"{namespaceType.Namespace}.{resourceName}"))
            {
                
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }

            }
        }
    }
}