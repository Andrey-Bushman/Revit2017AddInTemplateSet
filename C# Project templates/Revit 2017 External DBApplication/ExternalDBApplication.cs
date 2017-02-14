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

    /// <summary>
    /// The DB-level external application for subscribing to 
    /// DB-level events and updaters.
    /// </summary>
    sealed class ExternalDBApplication :
          IExternalDBApplication {

        /// <summary>
        /// This method executes some tasks when Autodesk Revit
        /// starts. Typically, event handlers and updaters are
        /// registered in this method.
        /// </summary>
        /// <param name="ctrl_app">Handle to the Revit 
        /// Application object.</param>
        /// <returns>Indicates if the external db application 
        /// completes its work successfully.</returns>
        ExternalDBApplicationResult IExternalDBApplication
            .OnStartup(ControlledApplication ctrl_app) {

            // Fix the bug of Revit 2017.1.1
            // More info read here:
            // https://revit-addins.blogspot.ru/2017/01/revit-201711.html
            RevitPatches.PatchCultures(ctrl_app.Language);

            // TODO: put your code here.

            return ExternalDBApplicationResult.Succeeded;
        }

        /// <summary>
        /// This method executes some tasks when Autodesk Revit
        /// shuts down.
        /// </summary>
        /// <param name="application">Handle to the Revit 
        /// Application object.</param>
        /// <returns>Indicates if the external db application 
        /// completes its work successfully.</returns>
        ExternalDBApplicationResult IExternalDBApplication
            .OnShutdown(ControlledApplication application) {

            // TODO: put your code here (optional).

            return ExternalDBApplicationResult.Succeeded;
        }
    }
}
