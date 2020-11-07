using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reflectensions;
using Reflectensions.ExtensionMethods;
using Scripter.Shared;

namespace Scripter.PowershellCore
{
    public class PowerShellCoreEngine: IScriptEngine
    {
        private readonly IServiceProvider _serviceProvider;
        
        public Func<string, string> CompileScript => null;
        public bool NeedsCompiledScript => false;
       

        private PsRunspace _psEngine;


        public PowerShellCoreEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _psEngine = new PsRunspace();
            Initialize();
        }
        
        private void Initialize()
        {
            _psEngine.SetVariable("ModulesProvider", new ScripterModulesProvider(_serviceProvider));

            
        }

        public void Stop()
        {
            
            _psEngine.Stop();
        }
        
        public void SetValue(string name, object value)
        {
            _psEngine.SetVariable(name, value);
        }

        public string GetValueAsJson(string name)
        {
            var value = _psEngine.GetVariable(name);
            return Json.Converter.ToJson(value);
        }

        public T GetValue<T>(string name)
        {
            var value = _psEngine.GetVariable(name);
            return value.To<T>();


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
