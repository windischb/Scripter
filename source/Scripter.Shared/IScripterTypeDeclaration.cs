namespace Scripter.Shared
{
    public interface IScripterTypeDeclaration
    {
        string Language { get; }
        string FileImport { get; }
        string GetImports();
        string GetTypeDefinitions();
    }
}