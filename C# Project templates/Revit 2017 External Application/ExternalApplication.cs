// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
	
  sealed class ExternalApplication : IExternalApplication{
	  
    Result IExternalApplication.OnStartup(
        UIControlledApplication uic_app) {

        // Fix the bug of Revit 2017.1.1
        RevitPatches.PatchCultures(uic_app
            .ControlledApplication.Language);

        // Create the tabs, panels, and buttons
        Tools.BuildUI(uic_app, Assembly.GetExecutingAssembly(), 
            typeof(Resources));

        // TODO: put your code here.

        return Result.Succeeded;
    }

    Result IExternalApplication.OnShutdown(
		UIControlledApplication uic_app){
			
		// TODO: put your code here (optional).

      return Result.Succeeded;
    }
  }
}
