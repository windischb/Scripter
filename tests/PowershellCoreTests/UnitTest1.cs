using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using Reflectensions;
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
$res = ($PsVersionTable)[1]
";

            await psEngine.ExecuteAsync(psScript);
            var res = psEngine.GetValue("res");

            var json = Json.Converter.ToJson(res, true);

            _output.WriteLine(json);

        }
    }
}
