using System;
using Scripter.Shared;

namespace Scripter.ConsoleWriter
{
    [ScripterModule("Console")]
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
    }
}
