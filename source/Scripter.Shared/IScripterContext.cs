using System;

namespace doob.Scripter.Shared
{
    public interface IScripterContext
    {
        IScripterContext AddScripterEngine<TEngine>() where TEngine : class, IScriptEngine;

        IScripterContext AddScripterEngine(Type engineType);

        IScripterContext AddScripterEngine<TEngine>(Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine;
        IScripterContext AddScripterEngine(Type engineType, Func<IServiceProvider, object> factory);
        IScripterContext AddScripterModule<TModule>() where TModule : class, IScripterModule;
        IScripterContext AddScripterModule(Type moduleType);
    }
}