using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using TreGoToFile.Extensions;

namespace TreGoToFile.Parsers
{
    internal class TypeScriptParser : IErrorParser
    {
        private static readonly Regex lineRegex = new Regex(@"(?<path>(?>\w\:\\)?(?>[\w\.\-]+(?>\\|\/))*[\w\.\-]+\.\w+)(?>((:|\s|)(?<line>\d+)(?>:(?<column>\d+))?)|(\((?<line1>\d+)\,(?<column1>\d+)\)))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public FilePoint GetError(TextPointer pointer)
        {
            var match = lineRegex.Match(pointer.GetLine(0));
            if (match.Success)
            {
                return new FilePoint
                {
                    Path = match.Groups["path"].Value,
                    Line = ToNullableInt(match.Groups["line"]) ?? ToNullableInt(match.Groups["line1"]),
                    Column = ToNullableInt(match.Groups["column"]) ?? ToNullableInt(match.Groups["column1"]),
                };
            }
            else
            {
                return null;
            }
        }

        private static int? ToNullableInt(Group group)
        {
            return group.Success ? int.Parse(group.Value) : (int?)null;
        }
    }
}
