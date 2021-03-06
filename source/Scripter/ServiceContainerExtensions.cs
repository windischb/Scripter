﻿using System;
using doob.Scripter.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace doob.Scripter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScripter(this IServiceCollection services, Action<ScripterContext> options)
        {
            options(new ScripterContext(services));
            services.TryAddTransient<EngineProvider>();
            services.TryAddTransient<IScripterModuleRegistry, ScripterModuleRegistry>();
            return services;
        }
    }
}
