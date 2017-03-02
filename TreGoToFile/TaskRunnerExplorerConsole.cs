using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreGoToFile
{
    internal class TaskRunnerExplorerConsole
    {
        private static ErrorParser parser = new ErrorParser();

        internal static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var output = GetConsoleOutput(sender);
            if (output != null)
            {
                var errorLocation = parser.GetError(output.GetPositionFromPoint(e.GetPosition(output), true));
                if (File.Exists(errorLocation.Path))
                {
                    TreGoToFilePackage.DTE.OpenFileInPreviewTab(errorLocation.Path, errorLocation.Line, errorLocation.Column);
                }
            }
        }

        private static RichTextBox GetConsoleOutput(object sender)
        {
            var output = sender as RichTextBox;
            if (output != null && output.Name == "Output")
            {
                return output;
            }
            else
            {
                return null;
            }
        }
    }
}