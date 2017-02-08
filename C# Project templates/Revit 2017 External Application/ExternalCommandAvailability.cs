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
	
	class ExternalCommandAvailability :
        IExternalCommandAvailability {

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
