namespace Scripter.Shared
{
    public interface IScripterModule
    {
       
    }

    public interface IScripterModule<T>: IScripterModule where T: ScripterTypeDefinition
    {

    }
}
