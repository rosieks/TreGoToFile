﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace TreGoToFile
{
    internal interface IErrorParser
    {
        FilePoint GetError(TextPointer ponter);
    }

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

    internal class JSHintErrorParser : IErrorParser
    {
        private static readonly Regex fileError = new Regex(@"line (?<line>\d+)\s+col (?<column>\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public FilePoint GetError(TextPointer pointer)
        {
            var line = GetLine(pointer, 0);
            var match = fileError.Match(line);
            if (match.Success)
            {
                int lineNo = int.Parse(match.Groups["line"].Value);
                int colNo = int.Parse(match.Groups["column"].Value);
                int i = -1;
                do
                {
                    line = GetLine(pointer, i);
                    if (IsFilePath(line))
                    {
                        return new FilePoint
                        {
                            Path = line.Trim(),
                            Line = lineNo,
                            Column = colNo,
                        };
                    }

                    i--;
                }
                while (true);
            }

            return null;
        }

        private bool IsFilePath(string line)
        {
            try
            {
                return File.Exists(line);
            }
            catch
            {
                return false;
            }
        }

        private string GetLine(TextPointer pointer, int index)
        {
            var start = pointer.GetLineStartPosition(index);
            var end = pointer.GetLineStartPosition(index + 1) ?? pointer.DocumentEnd;
            var line = new TextRange(start, end);

            return line.Text.Trim();
        }
    }
}