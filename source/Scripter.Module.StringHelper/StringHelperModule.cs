using System.Text.RegularExpressions;
using doob.Scripter.Shared;

namespace doob.Scripter.Module.StringHelper
{
    public class StringHelperModule: IScripterModule
    {

        public string RemoveEmptyLines(string value)
        {
            var val = Regex.Replace(value, @"^\s*$\n|\r", "", RegexOptions.Multiline);
            return val;
        }

    }
}
