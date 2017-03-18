using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TreGoToFile.Parsers
{
    internal class ErrorParser
    {
        IErrorParser[] parsers = new[]
        {
            new JSHintErrorParser(),
        };

        internal FilePoint GetError(TextPointer pointer)
        {
            foreach (var parser in parsers)
            {
                try
                {
                    var point = parser.GetError(pointer);
                    if (point != null)
                    {
                        return point;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return null;
        }
    }
}
