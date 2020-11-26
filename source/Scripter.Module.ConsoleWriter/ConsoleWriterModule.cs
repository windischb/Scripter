﻿using System;
using Scripter.Shared;

namespace Scripter.Module.ConsoleWriter
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


        public void SetCurrentEngine(IScriptEngine scriptEngine)
        {
            
        }
    }
}
