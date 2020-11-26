using Scripter.Shared;

namespace Scripter.Module.Template
{
    public class TypeDefinitions : ScripterTypeDefinition
    {
        public override string GetImports()
        {
            return GetFromResources<TypeDefinitions>("imports.d.ts");
        }


    }
}
