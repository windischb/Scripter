using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Reflectensions;
using Scripter;
using Scripter.ConsoleWriter;
using Scripter.HttpModule;
using Scripter.JavaScript;
using Scripter.Shared;
using Scripter.TypeScript;
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
                .AddScripterModule<Http>()
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

       
    }
}

