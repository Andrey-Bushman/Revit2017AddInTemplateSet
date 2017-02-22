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
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WPF = System.Windows;
using System.Linq;
using Bushman.RevitDevTools;
using $rootnamespace$.Properties;
#endregion

namespace $rootnamespace${

    /// <summary>
    /// This class implements the IUpdater interface used to 
    /// create an updater capable of reacting to changes in the
    /// Revit model.
    /// </summary>	
	[Transaction(TransactionMode.Manual)]
    public sealed partial class $safeitemname$ 
    	: IUpdater {

        static AddInId addin_id;
        static UpdaterId updater_id;

        public $safeitemname$(AddInId id) {

            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));
            try {
                addin_id = id;
                updater_id = new UpdaterId(addin_id, new Guid(
                    "$guid7$"));

                // TODO: put your code here.

            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }
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
            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));
            try {

                // TODO: put your code here.
            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }
        }

        /// <summary>
        /// Auxiliary text that Revit will use to inform the 
        /// end user when the Updater is not loaded.
        /// </summary>
        /// <returns></returns>
        String IUpdater.GetAdditionalInformation() {

            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            string message = "The exception happend in the " +
                "'IUpdater.GetAdditionalInformation()' method"
                + " code.";

            try {

                message = res_mng.GetString(ResourceKeyNames
                .UpdaterAuxiliaryText);
            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }
            return message;
        }

        ChangePriority IUpdater.GetChangePriority() {

            // TODO: delete this code row!
            throw new NotImplementedException();

            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            ChangePriority cp = default(ChangePriority);

            try {

                // TODO: Put your code here. Don't forget to 
                // change the ChangePriority value.
            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return cp;
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
