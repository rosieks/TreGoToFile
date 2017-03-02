using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace TreGoToFile
{
    internal static class DTEExtensions
    {
        public static void OpenFileInPreviewTab(this DTE dte, string file, int? line, int? column)
        {
            IVsNewDocumentStateContext newDocumentStateContext = null;

            try
            {
                var openDoc3 = Package.GetGlobalService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument3;

                Guid reason = VSConstants.NewDocumentStateReason.Navigation;
                newDocumentStateContext = openDoc3.SetNewDocumentState((uint)__VSNEWDOCUMENTSTATE.NDS_Provisional, ref reason);

                dte.ItemOperations.OpenFile(file);
                if (line.HasValue)
                {
                    ((TextSelection)dte.ActiveDocument.Selection).MoveTo(line.Value, column.Value);
                }
            }
            finally
            {
                if (newDocumentStateContext != null)
                {
                    newDocumentStateContext.Restore();
                }
            }
        }
    }
}
