using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Reflectensions;
using Scriban.Runtime;
using Scripter.Shared;

namespace Scripter.Module.Template
{
    public class TemplateModule: IScripterModule
    {


        public string Parse(string template, object data)
        {
            return Parse(template, new List<object> {data});
        }

        public string Parse(string template, params object[] data)
        {
            return Parse(template, data.ToList());
        }


        private string Parse(string template, IEnumerable<object> data)
        {
            JObject jobject = new JObject();
            data.Aggregate(jobject, (a, b) => {
                var json = Json.Converter.ToJson(b);
                var jo = Json.Converter.ToJObject(json);
                return Json.Converter.Merge(a, jo);
            });

            var dict = Json.Converter.ToDictionary(jobject);

            return ParseScriptObject(template, dict);
        }

        private string ParseScriptObject(string template, Dictionary<string, object> data)
        {
            var scriptObj = new ScriptObject(StringComparer.OrdinalIgnoreCase);
            scriptObj.Import(data, renamer: member => member.Name);
            var scribanTemplate = Scriban.Template.Parse(template);
            return scribanTemplate.Render(scriptObj);
        }

    }
}
