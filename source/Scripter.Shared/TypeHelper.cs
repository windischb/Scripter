using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
