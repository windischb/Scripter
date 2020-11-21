using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Reflectensions;
using Scripter;
using Scripter.Engine.TypeScript;
using Scripter.Module.ConsoleWriter;
using Scripter.Module.Http;
using Scripter.Shared;
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
                .AddTypeScriptEngine()
                .AddScripterModule<ConsoleWriterModule>()
                .AddScripterModule<HttpModule>()
            );


            ServiceProvider = sc.BuildServiceProvider();
        }

        [Fact]
        public async Task Test1()
        {
            var tsEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("TypeScript");

            var tsScript = @"
import * as con from 'Console';

con.WriteLine('Hello')

";

            var jsScript = tsEngine.CompileScript(tsScript);

            await tsEngine.ExecuteAsync(jsScript);
           
        }

        [Fact]
        public async Task HttpResponseAsObject()
        {
            var tsEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("TypeScript");


            var tds = ServiceProvider.GetServices<IScripterTypeDeclaration>();
            

            var tsScript = @"
import * as http from 'Http';
import * as variables from 'Variables';


var cl = http.Client('http://10.0.0.21:8123/api/states/switch.buero_fan_buero_ventilator')
                    .SetBearerToken('eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiI5M2VmNDdiNDc4ODg0MmI1YjFkYzM0OThjNjM0MWRiNyIsImlhdCI6MTYwMzkxNTA3OCwiZXhwIjoxOTE5Mjc1MDc4fQ.vTY4JseQEpmOkJw1UOkTWiyjALuewgtUR7HvaEqglKA'));

                    var resp = cl.Get().Content.AsObject();

                    

";

            var jsScript = tsEngine.CompileScript(tsScript);

            await tsEngine.ExecuteAsync(jsScript);

        }


    }
}

