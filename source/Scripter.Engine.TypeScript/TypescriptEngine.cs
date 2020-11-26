using System;
using System.IO;
using System.Threading.Tasks;
using Esprima;
using Jint;
using Scripter.Engine.JavaScript;
using Scripter.Shared;

namespace Scripter.Engine.TypeScript
{
    public class TypeScriptEngine: IScriptEngine
    {
        public Func<string, string> CompileScript => CompileScriptInternal;
        public bool NeedsCompiledScript { get; } = true;


        private JavaScriptEngine _javascriptEngine;
        
        public static ParserOptions EsprimaOptions = new ParserOptions {
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

        public object ConvertToDefaultObject(object value)
        {
            return _javascriptEngine.ConvertToDefaultObject(value);
        }

        public object JsonParse(string json)
        {
            return _javascriptEngine.JsonParse(json);
        }

        public string JsonStringify(object value)
        {
            return _javascriptEngine.JsonStringify(value);
        }


        public void SetValue(string name, object value)
        {
            _javascriptEngine.SetValue(name, value);

        }

        public string GetValueAsJson(string name)
        {
            return _javascriptEngine.GetValueAsJson(name);
        }

        public T GetValue<T>(string name)
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


        private static Esprima.Ast.Script TypeScriptScript;
        private string CompileScriptInternal(string sourceCode)
        {
            if (String.IsNullOrWhiteSpace(sourceCode))
                return null;

            if (TypeScriptScript == null) {
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

            using (Stream stream = type.Assembly.GetManifestResourceStream($"{type.Namespace}.{resourceName}"))
            {
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
