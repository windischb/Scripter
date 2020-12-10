using System;
using System.Text.RegularExpressions;
using Scripter.Shared;

namespace Scripter.Module.StringHelper
{
    public class StringHelperModule: IScripterModule<TypeDefinitions>
    {

        public string RemoveEmptyLines(string value)
        {
            var val = Regex.Replace(value, @"^\s*$\n|\r", "", RegexOptions.Multiline);
            return val;
        }
    }
}
