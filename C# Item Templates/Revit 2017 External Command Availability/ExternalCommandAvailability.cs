/* $projectname$
 * $itemname$.cs
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
using $rootnamespace$.Properties;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WPF = System.Windows;
#endregion

namespace $rootnamespace${
	
	[Transaction(TransactionMode.Manual)]
	sealed class $safeitemname$ : IExternalCommandAvailability {
	  
        bool IExternalCommandAvailability.IsCommandAvailable(
            UIApplication applicationData,
            CategorySet selectedCategories) {

        	// ============================================
            // TODO: delete these code rows and put your code 
            // here.
            if (applicationData.ActiveUIDocument != null &&
                selectedCategories.IsEmpty) {
                return true;
            }
            else {
                return false;
            }
            // ============================================
        }
  	}
}
