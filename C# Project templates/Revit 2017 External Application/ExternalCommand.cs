/* $projectname$
 * ExternalCommand.cs
 * $WebPage$
 * © $CompanyName$, $year$
 *
 * The external command.
 */
#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Resources;
using $RootNamespace$.$safeprojectname$.Properties;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WPF = System.Windows;
using System.Linq;
#endregion

namespace $RootNamespace$.$safeprojectname${
	
	[Transaction(TransactionMode.Manual)]
	sealed class ExternalCommand : IExternalCommand {
	  
        Result IExternalCommand.Execute(
        	ExternalCommandData commandData, ref string message,
            ElementSet elements) {

            UIApplication ui_app = commandData?.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Application app = ui_app?.Application;
            Document doc = ui_doc?.Document;

            ResourceManager res_mng = new ResourceManager(
              GetType());

            // ============================================
            // TODO: delete these code rows and put your code 
            // here.
            TaskDialog.Show(res_mng.GetString("_message"), 
            	string.Format(res_mng.GetString(
            		"_command_message"), GetType().Name));
            // ============================================

            res_mng.ReleaseAllResources();

            return Result.Succeeded;
        }
  }
}
