using System;
using Scripter.Shared;

namespace Scripter.Module.Common
{
    
    public class CommonModule: IScripterModule
    {

        public GuidHelper Guid { get; }
        public JsonConverter Json { get; }

        public CommonModule(IScriptEngine scriptEngine)
        {
            Guid = new GuidHelper();
            Json = new JsonConverter(scriptEngine);
        }

    }

    public class GuidHelper
    {
        public Guid Parse(string guid) => Guid.Parse(guid);
        public Guid New() => Guid.NewGuid();
        public Guid Empty() => Guid.Empty;
    }

    public class JsonConverter
    {
        private readonly IScriptEngine _scriptEngine;

        public JsonConverter(IScriptEngine scriptEngine)
        {
            _scriptEngine = scriptEngine;
        }

        public object Parse(string value)
        {
            return _scriptEngine.JsonParse(value);
        }

        public string Stringify(object value)
        {
            return _scriptEngine.JsonStringify(value);
        }
    }
}
