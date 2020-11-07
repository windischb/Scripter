using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Scripter.Shared;

namespace Scripter
{
    public class ScripterContext : IScripterContext
    {
        private readonly IServiceCollection _serviceCollection;

        internal ScripterContext(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public IScripterContext AddScripterEngine<TEngine>() where TEngine : class, IScriptEngine
        {
            var engineType = typeof(TEngine);
            var language = engineType.GetCustomAttribute<ScripterEngineAttribute>()?.Language ?? TrimEnd(engineType.Name, "Engine");
            return AddScripterEngine<TEngine>(language);
        }

        public IScripterContext AddScripterEngine<TEngine>(string language) where TEngine: class, IScriptEngine
        {
            _serviceCollection.AddTransient<TEngine>();
            _serviceCollection.AddNamedTransient<IScriptEngine, TEngine>(language);
            return this;
        }

        public IScripterContext AddScripterEngine<TEngine>(Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine
        {
            var engineType = typeof(TEngine);
            var language = engineType.GetCustomAttribute<ScripterEngineAttribute>()?.Language ?? TrimEnd(engineType.Name, "Engine");
            return AddScripterEngine<TEngine>(language, factory);
        }

        public IScripterContext AddScripterEngine<TEngine>(string language, Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine
        {
            _serviceCollection.AddTransient<TEngine>(factory);
            _serviceCollection.AddNamedTransient<IScriptEngine, TEngine>(language, factory);
            return this;
        }

        public IScripterContext AddScripterModule<TModule>() where TModule : class, IScripterModule
        {
            var moduleType = typeof(TModule);
            var moduleAttribute = moduleType.GetCustomAttribute<ScripterModuleAttribute>();
            var name = moduleAttribute?.Name ?? TrimEnd(moduleType.Name, "Module");

            _serviceCollection.AddNamedTransient<IScripterModule, TModule>(name);
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