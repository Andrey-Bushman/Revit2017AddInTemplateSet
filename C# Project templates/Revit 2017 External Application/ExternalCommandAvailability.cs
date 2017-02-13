// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* $projectname$
 * ExternalCommandAvailability.cs
 * $WebPage$
 * © $CompanyName$, $year$
 *
 * The 'ExternalCommand' command availability.
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

    /// <summary>
    /// This class provides an accessibility checking for an 
    /// external command of Revit.
    /// </summary>	
	class ExternalCommandAvailability :
        IExternalCommandAvailability {

        /// <summary>
        /// This method provides the control over whether an 
        /// external command is enabled or disabled.
        /// </summary>
        /// <param name="applicationData">An 
        /// ApplicationServices.Application object which 
        /// contains reference to Application needed by 
        /// external command.</param>
        /// <param name="selectedCategories">An list of 
        /// categories of the elements which have been selected
        /// in Revit in the active document, or an empty set if
        /// no elements are selected or there is no active 
        /// document.</param>
        /// <returns>Indicates whether Revit should enable or 
        /// disable the corresponding external command.
        /// </returns>
        bool IExternalCommandAvailability.IsCommandAvailable(
            UIApplication applicationData,
            CategorySet selectedCategories) {

            if (applicationData.ActiveUIDocument != null &&
                selectedCategories.IsEmpty) {
                return true;
            }
            else {
                return false;
            }
        }
	}
}
