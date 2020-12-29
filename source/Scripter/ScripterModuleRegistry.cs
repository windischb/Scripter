using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Scripter.Shared;

namespace Scripter
{
    public class ScripterModuleRegistry : IScripterModuleRegistry
    {
        private static ConcurrentDictionary<string, Type> RegisteredModules { get; } = new ConcurrentDictionary<string, Type>();
        private static List<string> _registered = new List<string>();


        internal static void RegisterModule(Type moduleType)
        {

            if (_registered.Contains(moduleType.FullName))
                return;

            var name = TrimEnd(moduleType.Name, "Module");

            RegisteredModules.TryAdd(name, moduleType);

            _registered.Add(moduleType.FullName);
           
        }
        
        public IScripterModule BuildModuleInstance(string name, IServiceProvider serviceProvider,
            IScriptEngine currentScriptEngine, Dictionary<Type, Func<object>> instanceDictionary = null)
        {

            if (!RegisteredModules.TryGetValue(name, out var module))
            {
                throw new Exception($"A ScripterModule with name '{name}' is not registered!");
            }
            var constructor = module.GetConstructors().FirstOrDefault();
            if(constructor == null)
            {
                return (IScripterModule)ActivatorUtilities.CreateInstance(serviceProvider, module);
            }

            var parameterInfos = constructor.GetParameters();
            if (parameterInfos.Length == 0)
            {
                return (IScripterModule)ActivatorUtilities.CreateInstance(serviceProvider, module);
            }
            else
            {
                instanceDictionary = instanceDictionary ?? new Dictionary<Type, Func<object>>();
                instanceDictionary[typeof(IScriptEngine)] = () => currentScriptEngine;


                return (IScripterModule)Activator.CreateInstance(module, BuildConstructorParameters(parameterInfos, serviceProvider, instanceDictionary));
            }

        }


        private object[] BuildConstructorParameters(ParameterInfo[] parameterInfos, IServiceProvider serviceProvider, Dictionary<Type, Func<object>> instances)
        {
            var parameterInstances = new List<object>();
            foreach (var parameterInfo in parameterInfos)
            {
                if(instances.TryGetValue(parameterInfo.ParameterType, out var instance))
                {
                    parameterInstances.Add(instance());
                }
                else
                {
                    parameterInstances.Add(serviceProvider.GetService(parameterInfo.ParameterType));
                }
            }

            return parameterInstances.ToArray();
        }
        
        private static string TrimEnd(string value, string trim)
        {

            if (value.EndsWith(trim))
            {
                value = value.Substring(0, value.Length - trim.Length);
            }

            return value;
        }
    }
}
