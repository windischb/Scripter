using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Scripter.Shared;

namespace Scripter
{
    public class ScripterContext : IScripterContext
    {
        private readonly IServiceCollection _serviceCollection;

        private List<string> _registered = new List<string>();
        internal ScripterContext(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public IScripterContext AddScripterEngine<TEngine>() where TEngine : class, IScriptEngine
        {
            return AddScripterEngine(typeof(TEngine));
        }

        public IScripterContext AddScripterEngine(Type engineType)
        {
            if (_registered.Contains(engineType.FullName))
                return this;

            var language = engineType.GetCustomAttribute<ScripterEngineAttribute>()?.Name ?? TrimEnd(engineType.Name, "Engine");

            _serviceCollection.TryAddTransient(engineType);
            _serviceCollection.TryAddNamedTransient(typeof(IScriptEngine), language, engineType);

            _registered.Add(engineType.FullName);
            return this;
        }
        
        public IScripterContext AddScripterEngine<TEngine>(Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine
        {
            return AddScripterEngine(typeof(TEngine), factory);
        }

        public IScripterContext AddScripterEngine(Type engineType, Func<IServiceProvider, object> factory)
        {
            if (_registered.Contains(engineType.FullName))
                return this;

            var language = engineType.GetCustomAttribute<ScripterEngineAttribute>()?.Name ?? TrimEnd(engineType.Name, "Engine");

            _serviceCollection.TryAddTransient(engineType);
            _serviceCollection.TryAddNamedTransient(typeof(IScriptEngine), language, factory);

            _registered.Add(engineType.FullName);
            return this;
        }

       

        public IScripterContext AddScripterModule<TModule>() where TModule : class, IScripterModule
        {
            return AddScripterModule(typeof(TModule));
        }
        public IScripterContext AddScripterModule(Type moduleType)
        {
            
            if (_registered.Contains(moduleType.FullName))
                return this;

            var moduleAttribute = moduleType.GetCustomAttribute<ScripterModuleAttribute>();
            var onlyTypeDefinition = moduleAttribute?.OnlyTypeDefinition ?? false;
            var name = moduleAttribute?.Name ?? TrimEnd(moduleType.Name, "Module");

            if (!onlyTypeDefinition)
            {
                _serviceCollection.AddNamedTransient(typeof(IScripterModule), name, moduleType);
            }


            var interfaces = moduleType.GetInterfaces();
            
            foreach (var i in interfaces)
            {
                var ist = i.IsGenericType && typeof(IScripterModule).IsAssignableFrom(i);
                if (ist)
                {
                    var defType = i.GenericTypeArguments[0];
                    var tdInstance = (ScripterTypeDefinition)Activator.CreateInstance(defType);
                    tdInstance.FileName = name;
                    _serviceCollection.AddSingleton<ScripterTypeDefinition>(tdInstance);
                }
            }
            
            _registered.Add(moduleType.FullName);
            return this;
        }

        private string TrimEnd(string value, string trim)
        {
            
            if (value.EndsWith(trim))
            {
                value = value.Substring(0, value.Length - trim.Length);
            }

            return value;
        }
    }
}