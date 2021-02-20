using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Reflectensions.ExtensionMethods;

namespace Scripter.Shared
{
    public static class TypeHelper
    {

        private static Dictionary<string, string> TypeMapping = new Dictionary<string, string>
        {
            ["number"] = "double",
            ["any"] = "object"
        };

        public static object CreateObject(string typeName, object[] parameters)
        {
            typeName = typeName.Replace("$", "`");
            typeName = RemoveGenericMarker(typeName);

            var type = Reflectensions.Helper.TypeHelper.FindType(typeName, TypeMapping);

            if (parameters?.Any() == true)
            {
                return Activator.CreateInstance(type, parameters);
            }
            else
            {
                return Activator.CreateInstance(type);
            }

        }

        public static Type FindType(string typeName)
        {
            typeName = typeName.Replace("$", "`");
            typeName = RemoveGenericMarker(typeName);
            return Reflectensions.Helper.TypeHelper.FindType(typeName, TypeMapping);
        }

        private static string RemoveGenericMarker(string typeName)
        {
            var regex = new Regex(@"`[\d+|\?]");
            var match = regex.Match(typeName);

            if (match.Success)
            {
                typeName = typeName.Replace(match.Value, "");
            }

            return typeName;
        }
    }
}
