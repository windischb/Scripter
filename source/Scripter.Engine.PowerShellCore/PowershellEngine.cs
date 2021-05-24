using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using doob.Reflectensions;
using doob.Reflectensions.ExtensionMethods;
using Microsoft.PowerShell.Commands;
using Scripter.Shared;

namespace Scripter.Engine.PowerShellCore
{
    public class PowerShellCoreEngine: IScriptEngine
    {
        private readonly IServiceProvider _serviceProvider;
        
        public Func<string, string> CompileScript => null;
        public bool NeedsCompiledScript => false;
       

        private PsRunspace _psEngine;
        private Dictionary<Type, Func<object>> ProvidedTypeFactories = new Dictionary<Type, Func<object>>();
        private List<string> UseTaggedModules = new List<string>();
        
        private ScripterModulesProvider _scripterModulesProvider;

        public PowerShellCoreEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _psEngine = new PsRunspace();
            Initialize();
        }
        
        private void Initialize()
        {
            _scripterModulesProvider =
                new ScripterModulesProvider(_serviceProvider, this, ProvidedTypeFactories, UseTaggedModules);

            _psEngine.SetVariable("ModulesProvider", _scripterModulesProvider);

            
        }

        public void Stop()
        {
            
            _psEngine.Stop();
        }

        public object ConvertToDefaultObject(object value)
        {
            var json = JsonStringify(value);
            return JsonParse(json);
        }

        public object JsonParse(string json)
        {
            return JsonObject.ConvertFromJson(json, false, null, out var err);

            //return Json.Converter.ToObject<ExpandoObject>(json);
        }

        public string JsonStringify(object value)
        {
            var _json_context = new JsonObject.ConvertToJsonContext(maxDepth: 99, enumsAsStrings: true, compressOutput: true);
            string json_result = JsonObject.ConvertToJson(value, _json_context);
            return json_result; // Json.Converter.ToJson(value);
        }

        public void AddModuleParameterInstance(Type type, Func<object> factory)
        {
            ProvidedTypeFactories[type] = factory;
        }

        public void AddTaggedModules(params string[] tags)
        {
            UseTaggedModules.AddRange(tags);
        }

        public T GetModuleState<T>()
        {
            var type = typeof(T);
            if (_scripterModulesProvider._instantiatedModules.ContainsKey(type))
            {
                return (T)_scripterModulesProvider._instantiatedModules[type];
            }

            return default;
        }

        public void SetValue(string name, object value)
        {
            _psEngine.SetVariable(name, value);
        }

        public string GetValueAsJson(string name)
        {
            var value = _psEngine.GetVariable(name);
            return JsonStringify(value);
        }

        public T GetValue<T>(string name)
        {
            var value = _psEngine.GetVariable(name);
            if (LanguagePrimitives.TryConvertTo(value, out T val))
            {
                return val;
            }

            if (value.Reflect().TryTo(out T val2))
            {
                return val2;
            }
            var json = JsonStringify(value);

            return Json.Converter.ToObject<T>(json);


        }

        public object GetValue(string name)
        {
            return _psEngine.GetVariable(name);
        }

        public Task ExecuteAsync(string script)
        {
            _psEngine.Invoke(script);
            return Task.CompletedTask;
        }

        public string Invoke(string script)
        {
            
            var results = _psEngine.Invoke(script).ToList();

            

            if (results.Count == 0)
            {
                return null;
            }

            if (results.Count == 1)
            {
                return Json.Converter.ToJson(results[0]);
            }

            return Json.Converter.ToJson(results);

        }

        public void Dispose()
        {
            _psEngine?.Dispose();
        }
    }
}
