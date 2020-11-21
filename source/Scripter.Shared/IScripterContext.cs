using System;

namespace Scripter.Shared
{
    public interface IScripterContext
    {
        IScripterContext AddScripterEngine<TEngine>() where TEngine : class, IScriptEngine;
        //IScripterContext AddScripterEngine<TEngine>(string language) where TEngine: class, IScriptEngine;
        IScripterContext AddScripterEngine<TEngine>(Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine;
        //IScripterContext AddScripterEngine<TEngine>(string language, Func<IServiceProvider, TEngine> factory) where TEngine : class, IScriptEngine;
        IScripterContext AddScripterModule<TModule>() where TModule : class, IScripterModule;
    }
}