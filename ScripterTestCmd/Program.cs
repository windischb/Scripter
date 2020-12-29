using System;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Reflectensions;
using Reflectensions.ExtensionMethods;
using Scripter;
using Scripter.Engine.JavaScript;
using Scripter.Engine.TypeScript;
using Scripter.Module.Common;
using Scripter.Module.ConsoleWriter;
using Scripter.Shared;

namespace ScripterTestCmd
{
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        
        static async Task Main(string[] args)
        {
            var sc = new ServiceCollection();
            sc.AddScripter(options => options
                .AddJavaScriptEngine()
                .AddTypeScriptEngine()
                .AddScripterModule<ConsoleWriterModule>()
                .AddScripterModule<CommonModule>()
                .AddScripterModule<GlobalVariablesModule>()
            );

            sc.AddSingleton<IVariablesRepository, VariableRepository>();

            sc.AddScoped<Options>(provider =>
            {
                var opts = new Options();
                opts.AddExtensionMethods(typeof(StringExtensions));
                return opts;
            });
            ServiceProvider = sc.BuildServiceProvider();

            await Execute();
        }


        private static async Task Execute()
        {


            var tsEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("TypeScript");
           
            var variable = Json.Converter.ToJToken("{\r\n    \"VKZ\": \"BMI\",\r\n    \"BereichsKennung\": \"urn:publicid:gv.at:cdid+ZP\"\r\n}");

           

            var tsScript = @"

import { GetString, GetAny } from 'GlobalVariables';
let data = GetAny(""BPK/DefaultRequestValues"");

let d = {
    a: 123,
    ...data
}

let z = JSON.stringify(d)

let y = d.VKZ.ToNullableInt();
";

           

            var jsScript = tsEngine.CompileScript(tsScript);

            await tsEngine.ExecuteAsync(jsScript);


            var z = tsEngine.GetValue<string>("z");
            var y = tsEngine.GetValue("y");
            Console.WriteLine(z);

        }
    }
}
