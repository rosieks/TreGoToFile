using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TreGoToFile.Extensions
{
    internal static class TextPointerExtensions
    {
        public static string GetLine(this TextPointer pointer, int index)
        {
            var start = pointer.GetLineStartPosition(index);
            var end = pointer.GetLineStartPosition(index + 1) ?? pointer.DocumentEnd;
            var line = new TextRange(start, end);

            return line.Text.Trim();
        }
    }
}
