/* $projectname$
 * ExternalApplication.cs
 * $WebPage$
 * © $CompanyName$, $year$
 *
 * The external application. This is the entry point of the
 * '$projectname$' add-in.
 */
#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Resources;
using $RootNamespace$.$safeprojectname$.Properties;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.IO;
using WPF = System.Windows;
using System.Linq;
#endregion

namespace $RootNamespace$.$safeprojectname${
	
  sealed class ExternalDBApplication :
        IExternalDBApplication {

        ExternalDBApplicationResult IExternalDBApplication
            .OnStartup(ControlledApplication ctrl_app) {

            // Fix the bug of Revit 2017.1.1
	        RevitPatches.PatchCultures(ctrl_app.Language);

            // TODO: put your code here.

            return ExternalDBApplicationResult.Succeeded;
        }

        ExternalDBApplicationResult IExternalDBApplication
            .OnShutdown(ControlledApplication application) {

            // TODO: put your code here (optional).

            return ExternalDBApplicationResult.Succeeded;
        }
    }
}
