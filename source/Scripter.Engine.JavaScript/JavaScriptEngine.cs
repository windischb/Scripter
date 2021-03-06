﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using doob.Reflectensions;
using doob.Scripter.Shared;
using Esprima;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Debugger;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace doob.Scripter.Engine.Javascript
{

    public class JavaScriptEngine : IScriptEngine
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IScripterModuleRegistry _scripterModuleRegistry;
        public Func<string, string> CompileScript => s => s;
        public bool NeedsCompiledScript => false;

        private Jint.Engine _engine;
        public const string StopExecutionIdentifier = "e06e73c8-67ec-411c-9761-c2f3b063f436";

        private readonly Dictionary<Type, Func<object>> _providedTypeFactories = new Dictionary<Type, Func<object>>();
        private readonly List<string> _useTaggedModules = new List<string>();
        private Dictionary<Type, object> _instantiatedModules = new Dictionary<Type, object>();


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
            _engine.SetValue("NewObject", new Func<string, object[], object?>(TypeHelper.CreateObject));
            managedExit = false;
            _engine.SetValue("managedExit", managedExit);
            //_engine.Global.FastAddProperty("middler", new NamespaceReference(_engine, "middler"), false, false, false );
            //_engine.Execute("var middler = importNamespace('middler')");
            _engine.DebugHandler.Step += EngineOnStep;
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
            var options = _serviceProvider.GetService<Options>();
            if (options == null)
            {
                options = new Options();
                options.CatchClrExceptions();
                options.DebugMode();
                options.AllowClr(AppDomain.CurrentDomain.GetAssemblies());
            }
            
            return options;
        }

        public void Stop()
        {
            managedExit = true;
            
        }

        public object? ConvertToDefaultObject(object? value)
        {
            if (value == null)
                return null;

            var json = JsonStringify(value);
            return JsonParse(json);
        }

        public object? JsonParse(string? json)
        {
            if (json == null)
                return null;

            var val = JsValue.FromObject(_engine, json);
            return _engine.Json.Parse(val, new JsValue[] { val });
        }

        public string JsonStringify(object? value)
        {

            

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

            value ??= JValue.CreateNull();

            return Json.Converter.ToJson(value);
        }

        public void AddModuleParameterInstance(Type type, Func<object> factory)
        {
            _providedTypeFactories[type] = factory;
        }

        public void AddTaggedModules(params string[] tags)
        {
            _useTaggedModules.AddRange(tags);
        }

        public T? GetModuleState<T>()
        {
            var type = typeof(T);
            if (_instantiatedModules.ContainsKey(type))
            {
                return (T)_instantiatedModules[type];
            }

            return default;
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

        public T? GetValue<T>(string name)
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

            var inst = _scripterModuleRegistry.BuildModuleInstance(value, _serviceProvider, this, _providedTypeFactories, _useTaggedModules);
            _instantiatedModules[inst.GetType()] = inst;
            return JsValue.FromObject(_engine, inst);
        }

        public void Dispose()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _engine = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
