using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doob.Scripter;
using doob.Scripter.Engine.Javascript;
using doob.Scripter.Engine.TypeScript;
using doob.Scripter.Module.Common;
using doob.Scripter.Module.ConsoleWriter;
using doob.Scripter.Module.Http;
using doob.Scripter.Shared;
using Jint;
using Jint.Native;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Scripter;
using Xunit;
using Xunit.Abstractions;

namespace TypeScriptTests
{
    public class UnitTest1
    {
        private IServiceProvider ServiceProvider { get; }
        private readonly ITestOutputHelper _output;
        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;

           
            var sc = new ServiceCollection();
            sc.AddScripter(options => options
                .AddJavaScriptEngine()
                .AddTypeScriptEngine()
                .AddScripterModule<ConsoleWriterModule>()
                .AddScripterModule<HttpModule>()
                .AddScripterModule<CommonModule>()
            );


            ServiceProvider = sc.BuildServiceProvider();
        }

        [Fact]
        public async Task Test1()
        {
            var tsEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("TypeScript");

            var tsScript = @"
import * as con from 'ConsoleWriter';

con.WriteLine('Hello')

";

            var jsScript = tsEngine.CompileScript(tsScript);

            await tsEngine.ExecuteAsync(jsScript);
           
        }

//        [Fact]
//        public async Task HttpResponseAsObject()
//        {
//            var tsEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("TypeScript");


//            //var declarations = ServiceProvider.GetServices<ScripterTypeDefinition>();

//            //var imports = declarations.ToDictionary(d => $"{d.FileName}.d.ts", d => d.GetImports()).Where(kv => !String.IsNullOrWhiteSpace(kv.Value)).ToList();
//            //var tds = declarations.ToDictionary(d => $"{d.FileName}.d.ts", d => d.GetTypeDefinitions()).Where(kv => !String.IsNullOrWhiteSpace(kv.Value)).ToList();



//            var tsScript = @"
//import * as http from 'Http';
//import * as com from 'Common';



//var cl = http.Client('http://10.0.0.21:8123/api/states/switch.buero_fan_buero_ventilator')
//                    .SetBearerToken('eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiI5M2VmNDdiNDc4ODg0MmI1YjFkYzM0OThjNjM0MWRiNyIsImlhdCI6MTYwMzkxNTA3OCwiZXhwIjoxOTE5Mjc1MDc4fQ.vTY4JseQEpmOkJw1UOkTWiyjALuewgtUR7HvaEqglKA'));

//                    var cont = cl.Get().Content;
//var resp = cont.AsText();

//var z = com.Json.Parse(resp)                    
//var zt = z.state;

//var json = com.Json.Stringify(cont.AsObject());

//";

//            var jsScript = tsEngine.CompileScript(tsScript);

//            await tsEngine.ExecuteAsync(jsScript);

//            var zt = (JsValue)tsEngine.GetValue("zt");
//            var state = zt.AsString();

//            var json = tsEngine.GetValue<string>("json");
//        }


    }
}

