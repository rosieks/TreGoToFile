using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TreGoToFile.Extensions;
using TreGoToFile.Parsers;

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
                if (errorLocation != null)
                {
                    string path = errorLocation.Path;
                    if (!File.Exists(path))
                    {
                        var console = GetTaskRunnerConsole(output);
                        var workingDirectory = GetWorkingDirectory(console);
                        path = Path.Combine(workingDirectory, path);
                    }

                    if (File.Exists(path))
                    {                                  
                        TreGoToFilePackage.DTE.OpenFileInPreviewTab(path, errorLocation.Line, errorLocation.Column);
                    }
                }               
            }
        }

        private static RichTextBox GetConsoleOutput(object sender)
        {
            if (sender is RichTextBox output && output.Name == "Output")
            {
                return output;
            }
            else
            {
                return null;
            }
        }

        private static object GetTaskRunnerConsole(FrameworkElement element)
        {
            while (element != null && element.GetType().Name != "TaskRunnerConsoleUserControl")
            {
                element = element.Parent as FrameworkElement;
            }

            if (element.GetType().Name == "TaskRunnerConsoleUserControl")
            {
                var console = element.GetType().GetProperty("Console").GetValue(element);

                return console;
            }
            else
            {
                return null;
            }
        }

        private static string GetWorkingDirectory(object console)
        {
            var task = console.GetType().GetProperty("Task").GetValue(console);
            var command = task.GetType().GetProperty("Command").GetValue(task);
            var workingDirectory = command.GetType().GetProperty("WorkingDirectory").GetValue(command);
            return (string)workingDirectory;
        }
    }
}
