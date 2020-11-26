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
        private static ConcurrentDictionary<string, ScripterModuleDefinition> RegisteredModules { get; } = new ConcurrentDictionary<string, ScripterModuleDefinition>();
        private static List<string> _registered = new List<string>();


        internal static void RegisterModule(Type moduleType)
        {

            if (_registered.Contains(moduleType.FullName))
                return;

            var moduleAttribute = moduleType.GetCustomAttribute<ScripterModuleAttribute>();
            var onlyTypeDefinition = moduleAttribute?.OnlyTypeDefinition ?? false;
            var name = moduleAttribute?.Name ?? TrimEnd(moduleType.Name, "Module");

           
            var interfaces = moduleType.GetInterfaces();

            var modDefinition = new ScripterModuleDefinition(moduleType);

            foreach (var i in interfaces)
            {
                var ist = i.IsGenericType && typeof(IScripterModule).IsAssignableFrom(i);
                if (ist)
                {
                    var defType = i.GenericTypeArguments[0];
                    var tdInstance = (ScripterTypeDefinition)Activator.CreateInstance(defType);
                    tdInstance.FileName = name;
                    modDefinition.AddScripterTypeDefinition(tdInstance);
                }
            }

            RegisteredModules.TryAdd(name, modDefinition);

            _registered.Add(moduleType.FullName);
           
        }
        
        public IScripterModuleDefinition GetModule(string name)
        {
            return RegisteredModules.TryGetValue(name, out var def) ? def : null;
        }

        public IScripterModule BuildModuleInstance(string name, IServiceProvider serviceProvider,
            IScriptEngine currentScriptEngine)
        {

            var module = GetModule(name);
            if (module == null)
            {
                throw new Exception($"A ScripterModule with name '{name}' is not registered!");
            }
            var constructor = module.ModuleType.GetConstructors().FirstOrDefault();
            if(constructor == null)
            {
                return (IScripterModule)ActivatorUtilities.CreateInstance(serviceProvider, module.ModuleType);
            }

            var parameterInfos = constructor.GetParameters();
            if (parameterInfos.Length == 0)
            {
                return (IScripterModule)ActivatorUtilities.CreateInstance(serviceProvider, module.ModuleType);
            }
            else
            {
                var instanceDictionary = new Dictionary<Type, object>
                {
                    [typeof(IScriptEngine)] = currentScriptEngine
                };
                return (IScripterModule)Activator.CreateInstance(module.ModuleType, BuildConstructorParameters(parameterInfos, serviceProvider, instanceDictionary));
            }

        }

        private object[] BuildConstructorParameters(ParameterInfo[] parameterInfos, IServiceProvider serviceProvider, Dictionary<Type, object> instances)
        {
            var parameterInstances = new List<object>();
            foreach (var parameterInfo in parameterInfos)
            {
                if(instances.TryGetValue(parameterInfo.ParameterType, out var instance))
                {
                    parameterInstances.Add(instance);
                    
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

    public class ScripterModuleDefinition : IScripterModuleDefinition
    {
        public Type ModuleType { get; }
        public List<ScripterTypeDefinition> TypeDefinitions { get; } = new List<ScripterTypeDefinition>();

        public ScripterModuleDefinition(Type moduleType)
        {
            ModuleType = moduleType;
        }

        internal void AddScripterTypeDefinition(ScripterTypeDefinition typeDefinition)
        {
            TypeDefinitions.Add(typeDefinition);
        }
    }
}
