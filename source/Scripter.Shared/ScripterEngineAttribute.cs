using System;

namespace Scripter.Shared
{
    public class ScripterEngineAttribute : Attribute
    {
        public string Language { get; }
        public ScripterEngineAttribute(string language)
        {
            Language = language;
        }
    }
}