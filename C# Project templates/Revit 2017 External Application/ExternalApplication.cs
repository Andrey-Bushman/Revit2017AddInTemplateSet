// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* $projectname$
 * ExternalApplication.cs
 * $WebPage$
 * © $CompanyName$, $year$
 *
 * The external application. This is the entry point of the
 * '$projectname$' (Revit add-in).
 */
#region Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WPF = System.Windows;
using System.Linq;
using Bushman.RevitDevTools;
using RDT = Bushman.RevitDevTools.Properties;
#endregion

namespace $RootNamespace$.$safeprojectname${

	/// <summary>
	/// Revit external application.
	/// </summary>	
	sealed partial class ExternalApplication 
		: IExternalApplication{

        /// <summary>
        /// This method will be executed when Autodesk Revit 
        /// will be started.
        /// </summary>
        /// <param name="uic_app">A handle to the application 
        /// being started.</param>
        /// <returns>Indicates if the external application 
        /// completes its work successfully.</returns>
        Result IExternalApplication.OnStartup(
            UIControlledApplication uic_app) {

            // Fix the bug of Revit 2017.1.1
            // More info read here:
            // https://revit-addins.blogspot.ru/2017/01/revit-201711.html
            RevitPatches.PatchCultures(uic_app
                .ControlledApplication.Language);

            // Create the tabs, panels, and buttons
            UIBuilder.BuildUI(uic_app, Assembly
                .GetExecutingAssembly(), typeof(RDT.Resources))
                ;

            // TODO: put your code here.

            return Result.Succeeded;
        }

        /// <summary>
        /// This method will be executed when Autodesk Revit 
        /// shuts down.</summary>
        /// <param name="uic_app">A handle to the application 
        /// being shut down.</param>
        /// <returns>Indicates if the external application 
        /// completes its work successfully.</returns>
        Result IExternalApplication.OnShutdown(
            UIControlledApplication uic_app) {

            // TODO: put your code here (optional).

            return Result.Succeeded;
        }
	}
}
