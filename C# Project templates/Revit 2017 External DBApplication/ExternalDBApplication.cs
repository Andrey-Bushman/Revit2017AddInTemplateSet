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
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.IO;
using WPF = System.Windows;
using System.Linq;
using Bushman.RevitDevTools;
using $RootNamespace$.$safeprojectname$.Properties;
using System.Xml.Linq;
#endregion

namespace $RootNamespace$.$safeprojectname${

    /// <summary>
    /// The DB-level external application for subscribing to 
    /// DB-level events and updaters.
    /// </summary>
    sealed partial class ExternalDBApplication
        : IExternalDBApplication {

        /// <summary>
        /// This method executes some tasks when Autodesk Revit
        /// starts. Typically, event handlers and updaters are
        /// registered in this method.
        /// 
        /// WARNING
        /// Don't use the RevitDevTools.dll features directly 
        /// in this method. You are to call other methods which
        /// do it instead of.
        /// </summary>
        /// <param name="ctrl_app">Handle to the Revit 
        /// Application object.</param>
        /// <returns>Indicates if the external db application 
        /// completes its work successfully.</returns>
        ExternalDBApplicationResult IExternalDBApplication
            .OnStartup(ControlledApplication ctrl_app) {

            ResourceManager res_mng = new ResourceManager(
                GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            ExternalDBApplicationResult result = 
                ExternalDBApplicationResult.Succeeded;

            try {

                AppDomain.CurrentDomain.AssemblyResolve +=
                    CurDom_AssemblyResolve;

                Initialize(ctrl_app);

                // TODO: put your code here.

            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);

                result = ExternalDBApplicationResult.Failed;
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return result;
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

            ResourceManager res_mng = new ResourceManager(
                GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            ExternalDBApplicationResult result =
                ExternalDBApplicationResult.Succeeded;

            try {

                // TODO: put your code here (optional).

                AppDomain.CurrentDomain.AssemblyResolve -=
                CurDom_AssemblyResolve;

            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);

                result = ExternalDBApplicationResult.Failed;
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return result;
        }

        void Initialize(ControlledApplication app) {
            // Fix the bug of Revit 2017.1.1
            // More info read here:
            // https://revit-addins.blogspot.ru/2017/01/revit-201711.html
            RevitPatches.PatchCultures(app.Language);
        }

        // It contains info from the AssemblyResolves.xml file.
        static Dictionary<string, string> asm_dict =
            GetResolves();

        /// <summary>
        /// This method reads info from the 
        /// AssemblyResolves.xml file.
        /// </summary>
        /// <returns>It returns a dictionary.</returns>
        private static Dictionary<string, string> GetResolves(
            ) {

            string asm_dir = Path.GetDirectoryName(Assembly
                .GetExecutingAssembly().Location);

            string xml_file = Path.Combine(asm_dir,
                "AssemblyResolves.xml");

            XElement xml = null;

            if (!File.Exists(xml_file) ||
                (xml = XElement.Load(xml_file)) == null) {

                RecoveryFile(xml_file);
                xml = XElement.Load(xml_file);
            }

            Dictionary<string, string> dict =
                new Dictionary<string, string>();

            foreach (var item in xml.Elements("Assembly")) {

                string key = item.Attribute("Name").Value;
                string value = Environment
                    .ExpandEnvironmentVariables(item.Attribute(
                        "Location").Value);

                if (!dict.ContainsKey(key)) {

                    dict.Add(key, value);
                }
            }

            return dict;
        }

        /// <summary>
        /// Recover the AssemblyResolves.xml file.
        /// </summary>
        /// <param name="xml_file"></param>
        private static void RecoveryFile(string xml_file) {

            if (string.IsNullOrEmpty(xml_file)) {
                throw new ArgumentException(nameof(xml_file));
            }

            string data =
@"<?xml version='1.0' encoding='utf-8' ?>
<!-- This file contains info about location of assemblies. -->
<Assemblies>
  <Assembly Name='Revit2017DevTools'
            Location='%AppData%\Bushman\Revit\2017\RevitDevTools\RevitDevTools.dll'/>
</Assemblies>";

            XElement xml = XElement.Parse(data);
            xml.Save(xml_file);
        }

        private Assembly CurDom_AssemblyResolve(object sender,
            ResolveEventArgs args) {

            string name = args.Name.Split(',').First();

            if (!asm_dict.ContainsKey(name)) {

                return null;
            }

            Assembly result = Assembly.LoadFrom(asm_dict[name])
                ;

            return result;
        }
    }
}
