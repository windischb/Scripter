﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Scripter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScripter(this IServiceCollection services, Action<ScripterContext> options)
        {
            options(new ScripterContext(services));
            services.TryAddTransient<EngineProvider>();
            return services;
        }
    }
}
