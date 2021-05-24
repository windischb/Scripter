using System;
using doob.Scripter.Shared;

namespace doob.Scripter.Module.ConsoleWriter
{

    public class ConsoleWriterModule: IScripterModule
    {

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }


        public void SetCurrentEngine(IScriptEngine scriptEngine)
        {
            
        }

    }
}
