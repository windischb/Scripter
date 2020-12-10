using Scripter.Shared;

namespace Scripter.Module.StringHelper
{
    public class TypeDefinitions : ScripterTypeDefinition
    {
        public override string GetImports()
        {
            return GetFromResources<TypeDefinitions>("imports.d.ts");
        }


    }
}
