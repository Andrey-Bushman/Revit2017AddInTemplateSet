// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bushman.RevitAddInTemplateSet_Configurator {
    class Program {
        static void Main(string[] args) {

            Console.Title = Constants.WindowTitle;
            WriteCopyrightInfo();

            var config_file_name = GetConfigFileName();
            XElement xml = GetXmlData(config_file_name);
            Dictionary<string, string> dict = GetDictionary(
                xml);

            XElement xml_src = xml.Element("CodeSources");

            string projects_src = xml_src.Element(
                "ProjectTemplatesRootFolder").Value;

            string items_src = xml_src.Element(
                "ItemTemplatesRootFolder").Value;

            FileInfo[] vstemplate_project_files =
                GetVSTemplateFiles(projects_src);

            FileInfo[] vstemplate_item_files =
                GetVSTemplateFiles(items_src);

            ResourceManager res_mng = new ResourceManager(
                typeof(Program));

            List<string> vs_proj_tmps = new List<string>();
            List<string> vs_item_tmps = new List<string>();

            using (RegistryKey rk = Registry.CurrentUser
                .OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio")
                ) {

                if (rk == null) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(res_mng.GetString(
                        "ide_not_found"));
                    Console.ResetColor();
                }

                string[] vs_versions = new[] { "11.0", "12.0",
                    "14.0", "15.0" };

                foreach (var version in vs_versions) {
                    using (var vs_rk = rk.OpenSubKey(version)
                        ) {

                        if (vs_rk == null) {
                            continue;
                        }

                        var proj_tmp = vs_rk.GetValue(
                            "UserProjectTemplatesLocation",
                            string.Empty) as string;

                        if (proj_tmp != string.Empty) {

                            var value = Path.Combine(
                                Environment
                                .ExpandEnvironmentVariables(
                                proj_tmp), "Visual C#");

                            vs_proj_tmps.Add(value);
                        }

                        var item_tmp = vs_rk.GetValue(
                            "UserItemTemplatesLocation",
                            string.Empty) as string;

                        if (item_tmp != string.Empty) {

                            var value = Path.Combine(
                                Environment
                                .ExpandEnvironmentVariables(
                                item_tmp), "Visual C#");

                            vs_item_tmps.Add(value);
                        }
                        vs_rk.Close();
                    }
                }
                rk.Close();
            }

            var dirs1 = DoWork(dict, vstemplate_project_files,
                vs_proj_tmps.ToArray());

            var dirs2 = DoWork(dict, vstemplate_item_files,
                vs_item_tmps.ToArray());

            var dirs_common = dirs1.Union(dirs2).Distinct()
                .OrderBy(n => n).ToArray();

            if (dirs_common.Length > 0) {

                Console.ForegroundColor = ConsoleColor
                                .Yellow;
                Console.WriteLine(res_mng.GetString(
                    "skeeped_dirs"));

                foreach (var out_dir in dirs_common) {
                    Console.WriteLine(out_dir);
                }
            }
            Console.ResetColor();
            res_mng.ReleaseAllResources();
        }

        /// <summary>
        /// Get xml settings.
        /// </summary>
        /// <param name="config_file_name">Xml-file name.
        /// </param>
        /// <returns>It returns the XElement instance.
        /// </returns>
        static XElement GetXmlData(string config_file_name) {

            if (string.IsNullOrEmpty(config_file_name)) {

                throw new ArgumentNullException(nameof(
                    config_file_name));
            }

            ResourceManager res_mng = new ResourceManager(
                typeof(Program));

            bool xml_was_recovered = false;

            // if the xml-file doesn't exist then recovery him.
            if (!File.Exists(config_file_name)) {

                RecoveryConfigFile(config_file_name);
                xml_was_recovered = true;
            }

            XElement xml = XElement.Load(config_file_name);

            // if xml-file is invalid then recovery him.
            if (xml == null || !IsValidXmlStructure(xml)) {

                Console.WriteLine(res_mng.GetString(
                        "invalid_xml"), config_file_name);

                RecoveryConfigFile(config_file_name);
                xml_was_recovered = true;
            }

            // if the file was recovered then notice about it.
            if (xml_was_recovered) {

                Console.WriteLine(res_mng.GetString(
                    "xml_file_recovered"),
                    config_file_name);
            }

            xml = XElement.Load(config_file_name);
            res_mng.ReleaseAllResources();

            return xml;
        }

        /// <summary>
        /// Check wether is valid structure of the xml-file.
        /// </summary>
        /// <param name="xml">Target xml.</param>
        /// <returns>If structure of xml is valid then it 
        /// returns true otherwise false.</returns>
        private static Boolean IsValidXmlStructure(
            XElement xml) {

            if (xml == null) return false;

            var pt_xml = xml.Element("CodeSources")?.Element(
                    "ProjectTemplatesRootFolder");

            if (pt_xml == null || pt_xml.Elements().Count() > 0
                ) {

                return false;
            }

            var it_root = xml.Element("CodeSources").Element(
                   "ItemTemplatesRootFolder");

            if (it_root == null || it_root.Elements()
                .Count() > 0) {

                return false;
            }

            var names = new[]{ "$ClientId$", "$VendorId$",
                "$CompanyName$", "$RootNamespace$", "$WebPage$"
                , "$AppYear$" };

            if (xml.Element("CustomParameters") == null ||
                xml.Element("CustomParameters").Elements(
                "CustomParameter").Count() != names.Length) {

                return false;
            }

            string[] values = new string[names.Length];

            for (int i = 0; i < names.Length; i++) {

                var cust_param = xml.Element("CustomParameters"
                    ).Elements("CustomParameter").
                    FirstOrDefault(n => n.Attribute("Name")
                    .Value == names[i]);

                if (cust_param == null || cust_param.Attribute(
                    "Value") == null) {

                    return false;
                }
            }
            return true;
        }

        private static string GetConfigFileName() {

            string cur_dir = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            string file_name = Path.Combine(cur_dir, Constants
                .ConfigurationFileName);

            return file_name;
        }

        private static void WriteCopyrightInfo() {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nRevit2017AddInTemplateSet " +
                "Configurator\n(c) Andrey Bushman, 2017\n");
            Console.ResetColor();
        }

        private static string[] DoWork(
            Dictionary<String, String> dict,
            FileInfo[] vstemplate_files,
            String[] target_dirs) {

            List<string> skeeped_target_dirs =
                new List<string>();

            foreach (var item in vstemplate_files) {

                // edit xml
                XElement xml = XElement.Load(item.FullName);
                XNamespace ns = xml.GetDefaultNamespace();

                XElement parent = xml.Element(ns +
                    "TemplateContent").Element(ns +
                    "CustomParameters");

                int fixes_count = 0;

                foreach (var record in parent.Elements(ns +
                    "CustomParameter")) {

                    string key = record.Attribute("Name").Value
                        ;

                    if (dict.ContainsKey(key)) {
                        record.Attribute("Value").Value =
                            dict[key];
                        ++fixes_count;
                    }
                }
                xml.Save(item.FullName);

                Console.WriteLine("File: '{0}'.", item.FullName
                    );
                Console.WriteLine("Fixes count: '{0}'\n",
                    fixes_count);

                // zip
                foreach (var t_dir in target_dirs) {

                    string out_dir = Environment
                        .ExpandEnvironmentVariables(t_dir);

                    if (Directory.Exists(out_dir)) {

                        string zip_file = Path.Combine(out_dir,
                                item.Directory.Name + ".zip");

                        if (File.Exists(zip_file)) {
                            File.Delete(zip_file);
                        }

                        ZipFile.CreateFromDirectory(
                            item.DirectoryName, zip_file,
                            CompressionLevel.NoCompression,
                            false);

                        Console.WriteLine("Created: '{0}'.",
                            zip_file);
                    }
                    else {
                        skeeped_target_dirs.Add(out_dir);
                    }
                }

                Console.WriteLine();
            }
            return skeeped_target_dirs.ToArray();
        }

        private static FileInfo[] GetVSTemplateFiles(
            String root_dir) {

            if (string.IsNullOrEmpty(root_dir) || !Directory
                .Exists(root_dir)) {

                return new FileInfo[] { };
            }

            DirectoryInfo dir = new DirectoryInfo(Path
                .GetFullPath(root_dir));

            FileInfo[] result = dir.GetFiles("*.vstemplate",
                SearchOption.AllDirectories);

            return result;
        }

        static Dictionary<string, string> GetDictionary(
            XElement xml) {

            Dictionary<string, string> dict =
                new Dictionary<string, string>();

            foreach (var item in xml.Element("CustomParameters"
                ).Elements("CustomParameter")) {

                string key = item.Attribute("Name").Value;
                string value = item.Attribute("Value").Value;
                dict.Add(key, value);
            }
            return dict;
        }

        static void RecoveryConfigFile(string file_name) {
            string default_data =
@"<?xml version='1.0' encoding='utf-8' ?>
<!-- More info read here:
https://revit-addins.blogspot.ru/2017/02/revit-visual-studio.html

Code sources are here:
https://github.com/Andrey-Bushman/Revit2017AddInTemplateSet -->
<Settings>
  <!-- RevitAddInTemplateSet code sources. YOU DON'T NEED TO 
  EDIT THESE SETTINGS IF YOU DIDN'T CHANGE PLACEMENT OF THESE 
  DIRECTORIES.-->
  <CodeSources>
    <!-- Each subfolder of this folder contains the code 
    sources of the individual project template. -->
    <ProjectTemplatesRootFolder>..\..\C# Project templates
    </ProjectTemplatesRootFolder>
    <!-- Each subfolder of this folder contains the code 
    sources of the individual item template. -->
    <ItemTemplatesRootFolder>..\..\C# Item Templates
    </ItemTemplatesRootFolder>
  </CodeSources>
  
  <!-- Custom parameters which are to be updated in the 
  vstemplate-file of code sources of each template before they 
  will be builded and installed. YOU ARE TO EDIT THE VALUES OF 
  'VALUE' ATTRIBUTES. Set their values as you need. But you 
  aren't to edit the values of 'Name' attributes! Also, don't 
  delete the 'CustomParameter' xml-elements. -->
  <CustomParameters>
    <CustomParameter Name='$ClientId$' Value='BUSHMAN'/>
    <CustomParameter Name='$VendorId$'
			Value='ru.blogspot.revit-addins'/>
    <CustomParameter Name='$CompanyName$' Value='Bushman'/>
    <CustomParameter Name='$RootNamespace$'
			Value='Bushman'/>
    <CustomParameter Name='$WebPage$'
			Value='https://revit-addins.blogspot.ru'/>
    <CustomParameter Name='$AppYear$' Value='2017'/>
  </CustomParameters>
</Settings>";

            XElement xml = XElement.Parse(default_data);
            xml.Save(file_name);
        }
    }
}
