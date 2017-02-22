// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* $projectname$
 * ExternalCommand_Work.cs
 * $WebPage$
 * © $CompanyName$, $year$
 *
 * This file contains the methods which are used by the 
 * command.
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
using $RootNamespace$.$safeprojectname$.Properties;
#endregion


namespace $RootNamespace$.$safeprojectname${
	
    public sealed partial class ExternalCommand {

        private bool DoWork(ExternalCommandData commandData,
            ref String message, ElementSet elements) {

            if (null == commandData) {

                throw new ArgumentNullException(nameof(
                    commandData));
            }

            if (null == message) {

                throw new ArgumentNullException(nameof(message)
                    );
            }

            if (null == elements) {

                throw new ArgumentNullException(nameof(elements
                    ));
            }

            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            UIApplication ui_app = commandData.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Application app = ui_app?.Application;
            Document doc = ui_doc?.Document;

            var tr_name = res_mng.GetString("_transaction_name"
                );

            try {
                using (var tr = new Transaction(doc, tr_name)
                    ) {

                    if (TransactionStatus.Started == tr.Start()
                        ) {

                        // ====================================
                        // TODO: delete these code rows and put
                        // your code here.
                        TaskDialog.Show(res_mng.GetString(
                            ResourceKeyNames.TaskDialogTitle),
                            string.Format(res_mng.GetString(
                                ResourceKeyNames
                                .TaskDialogMessage), GetType()
                                .Name));
                        // ====================================

                        return TransactionStatus.Committed ==
                            tr.Commit();
                    }
                }
            }
            catch (Exception ex) {
                /* TODO: Handle the exception here if you need 
                 * or throw the exception again if you need. */
                throw ex;
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return false;
        }
    }
}
