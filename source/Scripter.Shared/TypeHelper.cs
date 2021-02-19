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
        public static object CreateObject(string type, object[] parameters)
        {
            type = type.Replace("$", "`");

            object[] constructorParameters;

            if (!type.Contains("`"))
            {
                return CreateObject(type, null, parameters);
            }
            else
            {
                var splitted = type.Split("`");
                var genericTypeParametersCount = splitted[1].ToInt();

                var genericTypeParameters = parameters.Take(genericTypeParametersCount).Select(o => o.ToString()).ToArray();
                constructorParameters = parameters.Skip(genericTypeParametersCount).ToArray();

                return CreateObject(splitted[0], genericTypeParameters, constructorParameters);
            }
        }

        public static object CreateObject(string type, string[] genericTypeParameters, object[] constructorArguments)
        {
            
            if (genericTypeParameters?.Any() == true){
                genericTypeParameters = genericTypeParameters.Select(o => Reflectensions.Helper.TypeHelper.FindType(o.ToString()).FullName).ToArray();
                type = $"{type}`{genericTypeParameters.Length}[{String.Join(",", genericTypeParameters)}]";
            }

            var t = Reflectensions.Helper.TypeHelper.FindType(type);

            if (constructorArguments?.Any() == true)
            {
                return Activator.CreateInstance(t, constructorArguments);
            }
            else
            {
                return Activator.CreateInstance(t);
            }

        }
    }
}
