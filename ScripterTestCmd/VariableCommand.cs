using System;
using System.Linq;
using System.Net;
using Reflectensions;
using Scripter.Shared;

namespace ScripterTestCmd
{
    public class GlobalVariablesModule: IScripterModule
    {
        private readonly IVariablesRepository _variablesStore;
        private readonly IScriptEngine _scriptEngine;


        public GlobalVariablesModule(IVariablesRepository variablesStore, IScriptEngine scriptEngine)
        {
            _variablesStore = variablesStore;
            _scriptEngine = scriptEngine;
        }

        public ITreeNode GetVariable(string path)
        {
            path = path.Replace(".", "/");

            string parent = null;
            string name = path;
            if (path.Contains("/"))
            {
                var parts = path.Split('/');
                parent = String.Join("/", parts.Take(parts.Length - 1));
                name = parts.Last();
            }

            return _variablesStore.GetVariable(parent, name);
        }

        public T GetVariableContent<T>(string path)
        {
            var variable = this.GetVariable(path);
            return Json.Converter.ToObject<T>(variable.Content);
        }

        public object GetAny(string path)
        {
            var variable = this.GetVariable(path);
            return _scriptEngine.ConvertToDefaultObject(variable.Content);
            
        }

        public string GetString(string path)
        {
            var variable = this.GetVariable($"{path}");
            return (string)variable.Content;
        }

        public decimal GetNumber(string path)
        {
            var variable = this.GetVariable($"{path}");
            return (decimal)variable.Content;
        }

        public bool GetBoolean(string path)
        {
            var variable = this.GetVariable($"{path}");
            return (bool)variable.Content;
        }

       
    }
}
