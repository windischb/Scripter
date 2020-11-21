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

        private List<Type> _registered = new List<Type>();
        internal ScripterContext(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public IScripterContext AddScripterEngine<TEngine>() where TEngine : class, IScriptEngine
        {
            var engineType = typeof(TEngine);
            var language = engineType.GetCustomAttribute<ScripterEngineAttribute>()?.Name ?? TrimEnd(engineType.Name, "Engine");
            return AddScripterEngine<TEngine>(language);
        }

        private IScripterContext AddScripterEngine<TEngine>(string language) where TEngine : class, IScriptEngine
        {
            if (_registered.Contains(typeof(TEngine)))
                return this;

            _serviceCollection.TryAddTransient<TEngine>();
            _serviceCollection.TryAddNamedTransient<IScriptEngine, TEngine>(language);

            _registered.Add(typeof(TEngine));
            return this;
        }

        public IScripterContext AddScripterEngine<TEngine>(Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine
        {
            var engineType = typeof(TEngine);
            var language = engineType.GetCustomAttribute<ScripterEngineAttribute>()?.Name ?? TrimEnd(engineType.Name, "Engine");
            return AddScripterEngine<TEngine>(language, factory);
        }

        private IScripterContext AddScripterEngine<TEngine>(string language, Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine
        {
            if (_registered.Contains(typeof(TEngine)))
                return this;

            _serviceCollection.TryAddTransient<TEngine>(factory);
            _serviceCollection.TryAddNamedTransient<IScriptEngine, TEngine>(language, factory);

            _registered.Add(typeof(TEngine));
            return this;
        }

        public IScripterContext AddScripterModule<TModule>() where TModule : class, IScripterModule
        {
            var moduleType = typeof(TModule);
            if (_registered.Contains(moduleType))
                return this;

            var moduleAttribute = moduleType.GetCustomAttribute<ScripterModuleAttribute>();
            var onlyTypeDefinition = moduleAttribute?.OnlyTypeDefinition ?? false;
            var name = moduleAttribute?.Name ?? TrimEnd(moduleType.Name, "Module");

            if (!onlyTypeDefinition)
            {
                _serviceCollection.AddNamedTransient<IScripterModule, TModule>(name);
            }


            var interfaces = moduleType.GetInterfaces();
            
            foreach (var i in interfaces)
            {
                var ist = i.IsGenericType && typeof(IScripterModule).IsAssignableFrom(i);
                if (ist)
                {
                    var defType = i.GenericTypeArguments[0];
                    var tdInstance = (IScripterTypeDeclaration)Activator.CreateInstance(defType);
                    _serviceCollection.AddSingleton<IScripterTypeDeclaration>(tdInstance);
                }
            }
            
            _registered.Add(moduleType);
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