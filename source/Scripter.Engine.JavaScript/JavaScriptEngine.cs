using System;
using System.Threading.Tasks;
using Esprima;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Debugger;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Reflectensions;
using Scripter.Shared;

namespace Scripter.Engine.JavaScript
{

    public class JavaScriptEngine : IScriptEngine
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IScripterModuleRegistry _scripterModuleRegistry;
        public Func<string, string> CompileScript => null;
        public bool NeedsCompiledScript => false;

        private Jint.Engine _engine;
        public const string StopExecutionIdentifier = "e06e73c8-67ec-411c-9761-c2f3b063f436";

        public static ParserOptions EsprimaOptions = new ParserOptions
        {
            Tolerant = true
        };

        private bool managedExit = false;



        public JavaScriptEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _scripterModuleRegistry = serviceProvider.GetRequiredService<IScripterModuleRegistry>();
            _engine = new Jint.Engine(GetOptions());
            Initialize();
        }



        private void Initialize()
        {
            _engine.Execute($"function exit() {{ setManagedExit(true); throw \"{StopExecutionIdentifier}\";}}", EsprimaOptions);
            _engine.Execute("var exports = {};", EsprimaOptions);


            void SetManagedExit(bool value)
            {
                managedExit = value;
            }


            _engine.SetValue("setManagedExit", new Action<bool>(SetManagedExit));
            managedExit = false;
            _engine.SetValue("managedExit", managedExit);
            //_engine.Global.FastAddProperty("middler", new NamespaceReference(_engine, "middler"), false, false, false );
            //_engine.Execute("var middler = importNamespace('middler')");
            _engine.Step += EngineOnStep;
            _engine.SetValue("require", new Func<string, JsValue>(Require));

        }



        private StepMode EngineOnStep(object sender, DebugInformation e)
        {
            if (managedExit)
            {
                throw new OperationCanceledException();
            }
            return StepMode.Over;
        }

        private Options GetOptions()
        {
            var options = _serviceProvider.GetService<Options>() ?? new Options();
            options.CatchClrExceptions();
            options.DebugMode();
            options.AllowClr(AppDomain.CurrentDomain.GetAssemblies());
            return options;
        }

        public void Stop()
        {
            managedExit = true;
        }

        public object ConvertToDefaultObject(object value)
        {
            string json = JsonStringify(value);
            return JsonParse(json);
        }

        public object JsonParse(string json)
        {
            if (json == null)
                return null;

            var val = JsValue.FromObject(_engine, json);
            return _engine.Json.Parse(val, new JsValue[] { val });
        }

        public string JsonStringify(object value)
        {

            if (value == null)
                return null;

            switch (value)
            {
                case JsValue jsValue:
                    {
                        return _engine.Json.Stringify(jsValue, new JsValue[] { jsValue }).AsString();

                    }
                case JToken jToken:
                    {
                        return jToken.ToString();
                    }
            }

            return Json.Converter.ToJson(value);
        }


        public void SetValue(string name, object value)
        {
            switch (value)
            {
                case string str:
                    _engine.SetValue(name, str);
                    break;
                case double dbl:
                    _engine.SetValue(name, dbl);
                    break;
                case bool _bool:
                    _engine.SetValue(name, _bool);
                    break;
                default:
                    var obj = JsValue.FromObject(_engine, value);
                    _engine.SetValue(name, obj);
                    break;
            }

        }

        public string GetValueAsJson(string name)
        {
            var value = _engine.GetValue(name);
            return _engine.Json.Stringify(value, new JsValue[] { value }).AsString();
        }

        public T GetValue<T>(string name)
        {
            return Json.Converter.ToObject<T>(GetValueAsJson(name));
        }

        public object GetValue(string name)
        {
            return _engine.GetValue(name);
        }

        public Task ExecuteAsync(string script)
        {
            return Task.Run(() => InternalExecute(script));

        }

        private void InternalExecute(string script)
        {
            managedExit = false;
            if (String.IsNullOrWhiteSpace(script))
                return;


            try
            {
                _engine.Execute(script, EsprimaOptions);
            }
            catch (Exception exception)
            {
                if (!managedExit)
                {
                    var ex = exception.GetBaseException();
                    if (ex is JavaScriptException jsex)
                    {
                        if (jsex.Message != StopExecutionIdentifier)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }


            }

        }


        private JsValue Require(string value)
        {
            var inst = _scripterModuleRegistry.BuildModuleInstance(value, _serviceProvider, this);
            return JsValue.FromObject(_engine, inst);
        }

        public void Dispose()
        {
            _engine = null;
        }
    }
}
