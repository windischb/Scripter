﻿using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using doob.Scripter.Engine.Javascript;
using doob.Scripter.Shared;
using Esprima;
using Jint;

namespace doob.Scripter.Engine.TypeScript
{
    public class TypeScriptEngine : IScriptEngine
    {
        public Func<string, string> CompileScript => CompileScriptInternal;
        public bool NeedsCompiledScript { get; } = true;


        private JavaScriptEngine _javascriptEngine;

        public static ParserOptions EsprimaOptions = new ParserOptions
        {
            Tolerant = true
        };


        public TypeScriptEngine(JavaScriptEngine javaScriptEngine)
        {
            _javascriptEngine = javaScriptEngine;
        }

        public void Stop()
        {
            _javascriptEngine.Stop();
        }

        public object? ConvertToDefaultObject(object? value)
        {
            return _javascriptEngine.ConvertToDefaultObject(value);
        }

        public object? JsonParse(string? json)
        {
            return _javascriptEngine.JsonParse(json);
        }

        public string JsonStringify(object? value)
        {
            return _javascriptEngine.JsonStringify(value);
        }

        public void AddModuleParameterInstance(Type type, Func<object> factory)
        {
            _javascriptEngine.AddModuleParameterInstance(type, factory);
        }

        public void AddTaggedModules(params string[] tags)
        {
            _javascriptEngine.AddTaggedModules(tags);
        }

        public T? GetModuleState<T>()
        {
            return _javascriptEngine.GetModuleState<T>();
        }


        public void SetValue(string name, object value)
        {
            _javascriptEngine.SetValue(name, value);

        }

        public string GetValueAsJson(string name)
        {
            return _javascriptEngine.GetValueAsJson(name);
        }

        public T? GetValue<T>(string name)
        {
            return _javascriptEngine.GetValue<T>(name);
        }

        public object GetValue(string name)
        {
            return _javascriptEngine.GetValue(name);
        }


        public Task ExecuteAsync(string script)
        {
            return _javascriptEngine.ExecuteAsync(script);

        }


        private static Esprima.Ast.Script? TypeScriptScript;
        private string CompileScriptInternal(string sourceCode)
        {
            if (String.IsNullOrWhiteSpace(sourceCode))
                return "";


            Regex regex = new Regex(@"new\s(?<typeName>[a-zA-Z0-9_\.\s<>\[\]$,]+)\((?<parameters>[a-zA-Z0-9_\.,\s<>\[\]$'""]+)?\)(;)?(?<ignore>//ignore)?");

            //var match = regex.Match(sourceCode);

            var matches = regex.Matches(sourceCode);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    if (match.Groups["ignore"].Success)
                    {
                        continue;
                    }

                    var typeName = match.Groups["typeName"].Value;

                    var type = TypeHelper.FindConstructorReplaceType(typeName);
                    if (type != null)
                    {
                        var parameters = match.Groups["parameters"].Value;

                        var constuctorParameters = "";
                        if (!String.IsNullOrWhiteSpace(parameters))
                        {
                            constuctorParameters = $", [{parameters}]";
                        }

                        var replaceText = $"NewObject('{typeName}'{constuctorParameters})";

                        sourceCode = sourceCode.Replace(match.Value, replaceText);

                    }
                }
            }
            
            if (TypeScriptScript == null)
            {
                var tsLib = GetFromResources("typescript.min.js");
                var parser = new JavaScriptParser(tsLib, EsprimaOptions);

                TypeScriptScript = parser.ParseScript();
            }



            var _engine = new Jint.Engine();

            _engine.Execute(TypeScriptScript);



            _engine.SetValue("src", sourceCode);


            var transpileOtions = "{\"compilerOptions\": {\"target\":\"ES5\"}}";

            var output = _engine.Execute($"ts.transpileModule(src, {transpileOtions})", EsprimaOptions).GetCompletionValue().AsObject();
            return output.Get("outputText").AsString();

        }

        public static string GetFromResources(string resourceName)
        {
            var type = typeof(TypeScriptEngine);

            using (Stream? stream = type.Assembly.GetManifestResourceStream($"{type.Namespace}.{resourceName}"))
            {
                if (stream == null)
                    throw new Exception($"Can't find Resource named '{type.Namespace}.{resourceName}'");

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }

            }
        }


        public void Dispose()
        {
            _javascriptEngine.Dispose();
        }
    }
}
