using System;
using System.Threading.Tasks;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Scripter.Shared;

namespace Scripter
{
    public class EngineProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public EngineProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public IScriptEngine GetEngine(string scriptLanguage)
        {
            return _serviceProvider.GetRequiredNamedService<IScriptEngine>(scriptLanguage);
        }

    }
}
