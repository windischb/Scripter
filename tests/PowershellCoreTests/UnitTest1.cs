using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using doob.Reflectensions;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Scripter;
using Scripter.Engine.PowerShellCore;
using Scripter.Engine.PowerShellCore.JsonConverter;
using Scripter.Module.ConsoleWriter;
using Scripter.Module.Http;
using Scripter.Shared;
using Xunit;
using Xunit.Abstractions;

namespace PowershellCoreTests
{
    public class UnitTest1
    {
        private IServiceProvider ServiceProvider { get; }
        private readonly ITestOutputHelper _output;
        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;

            Json.Converter.RegisterJsonConverter<PSObjectJsonConverter>();
            Json.Converter.JsonSerializer.Error += (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            };

            var sc = new ServiceCollection();
            sc.AddScripter(options => options
                .AddPowerShellCoreEngine()
                .AddScripterModule<ConsoleWriterModule>()
                .AddScripterModule<HttpModule>()
            );


            ServiceProvider = sc.BuildServiceProvider();
        }

        [Fact]
        public async Task Test1()
        {
            var psEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("PowerShellCore");

            var psScript = "$dt = get-date";

            await psEngine.ExecuteAsync(psScript);
            var dt = psEngine.GetValue<DateTime>("dt");

            Assert.Equal(dt.Minute, DateTime.Now.Minute);
        }

        [Fact]
        public async Task PsObjectConverterTest()
        {

            var psEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("PowerShellCore");

            var psScript = @"
$res = $PsVersionTable.PSVersion
";

            await psEngine.ExecuteAsync(psScript);
            var res = psEngine.GetValue<Version>("res");

            Assert.Equal(7, res.Major);
            var json = Json.Converter.ToJson(res, true);

            _output.WriteLine(json);

        }


        [Fact]
        public async Task GetProcessTest()
        {
            var proc = Process.GetCurrentProcess();
            var psEngine = ServiceProvider.GetRequiredNamedService<IScriptEngine>("PowerShellCore");

            var psScript = @"
$res = [System.Diagnostics.Process]::GetCurrentProcess()
";

            await psEngine.ExecuteAsync(psScript);
            //var j = psEngine.GetValueAsJson("res");
            var res = psEngine.GetValue<Process>("res");

            Assert.Equal(proc.Id, res.Id);
           
        }
    }
}
