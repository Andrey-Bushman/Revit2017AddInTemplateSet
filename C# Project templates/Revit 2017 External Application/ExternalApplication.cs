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
using $RootNamespace$.$safeprojectname$.Properties;
using Bushman.RevitDevTools;
using System.Xml.Linq;
#endregion

namespace $RootNamespace$.$safeprojectname${

    /// <summary>
    /// Revit external application.
    /// </summary>  
    sealed partial class ExternalApplication
        : IExternalApplication {

        /// <summary>
        /// This method will be executed when Autodesk Revit 
        /// will be started.
        /// 
        /// WARNING
        /// Don't use the RevitDevTools.dll features directly 
        /// in this method. You are to call other methods which
        /// do it instead of.
        /// </summary>
        /// <param name="uic_app">A handle to the application 
        /// being started.</param>
        /// <returns>Indicates if the external application 
        /// completes its work successfully.</returns>
        Result IExternalApplication.OnStartup(
            UIControlledApplication uic_app) {

            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            Result result = Result.Succeeded;

            try {

                AppDomain.CurrentDomain.AssemblyResolve +=
                CurDom_AssemblyResolve;

                Initialize(uic_app);

                // TODO: put your code here.
            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);

                result = Result.Failed;
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return result;
        }

        void Initialize(UIControlledApplication uic_app) {
            // Fix the bug of Revit 2017.1.1
            // More info read here:
            // https://revit-addins.blogspot.ru/2017/01/revit-201711.html
            RevitPatches.PatchCultures(uic_app
                .ControlledApplication.Language);



            // Create the tabs, panels, and buttons
            UIBuilder.BuildUI(uic_app, Assembly
                .GetExecutingAssembly(), typeof(Resources))
                ;
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

            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            Result result = Result.Succeeded;

            try {

                AppDomain.CurrentDomain.AssemblyResolve -=
                CurDom_AssemblyResolve;

            }
            catch (Exception ex) {

                TaskDialog.Show(def_res_mng.GetString("_Error")
                    , ex.Message);

                result = Result.Failed;
            }
            finally {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return result;
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
