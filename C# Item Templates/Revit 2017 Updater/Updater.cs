// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* $itemname$.cs
 * $WebPage$
 * © $CompanyName$, $year$
 *
 * This updater is used to create an updater capable of reacting
 * to changes in the Revit model.
 */
#region Namespaces
using System;
using System.IO;
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

    /// <summary>
    /// This class implements the IUpdater interface used to 
    /// create an updater capable of reacting to changes in the
    /// Revit model.
    /// </summary>	
	[Transaction(TransactionMode.Manual)]
    sealed class $safeitemname$ : IUpdater {

        static AddInId addin_id;
        static UpdaterId updater_id;

        public $safeitemname$(AddInId id) {
            addin_id = id;
            updater_id = new UpdaterId(addin_id, new Guid(
                "$guid7$"));
        }

        /// <summary>
        /// The method that Revit will invoke to perform an 
        /// update.
        /// </summary>
        /// <param name="data">Provides all necessary data 
        /// needed to perform the update, including the 
        /// document and information about the changes that 
        /// triggered the update.</param>
        void IUpdater.Execute(UpdaterData data) {

            throw new NotImplementedException();
        }

        /// <summary>
        /// Auxiliary text that Revit will use to inform the 
        /// end user when the Updater is not loaded.
        /// </summary>
        /// <returns></returns>
        String IUpdater.GetAdditionalInformation() {

            ResourceManager res_mng = new ResourceManager(
                GetType());

            var message = res_mng.GetString("_Auxiliary_text");

            return message;
        }

        ChangePriority IUpdater.GetChangePriority() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Identifies the nature of the change the Updater 
        /// will be performing Used to identify order of 
        /// execution of updaters Called once during 
        /// registration of the updater.
        /// </summary>
        /// <returns></returns>
        UpdaterId IUpdater.GetUpdaterId() {
            return updater_id;
        }

        /// <summary>
        /// Returns a name that the Updater can be identified 
        /// by to the user.
        /// </summary>
        /// <returns></returns>
        String IUpdater.GetUpdaterName() {
            return nameof($safeitemname$);
        }
    }
}
