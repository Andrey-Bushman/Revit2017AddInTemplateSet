using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bushman.RevitAddInTemplateSet_Configurator {
    class Program {
        static void Main(string[] args) {

            Console.Title =
                "RevitAddInTemplateSet Configurator";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nRevitAddInTemplateSet " +
                "Configurator");
            Console.WriteLine("(c) Andrey Bushman, 2017\n");
            Console.ResetColor();

            string cur_dir = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            string xml_file = Path.Combine(cur_dir,
                "CustomParameters.xml");

            if (!File.Exists(xml_file)) {
                RecoveryCustomParameters(xml_file);
            }
            XElement xml = XElement.Load(xml_file);

            Dictionary<string, string> dict = GetDictionary(
                xml);

            XElement xml_src = xml.Element(
                "RevitAddInTemplateSet_Src");

            XElement xml_projects_src = xml_src.Element(
                "ProjectTemplatesRootFolder");

            XElement xml_items_src = xml_src.Element(
                "ItemTemplatesRootFolder");

            FileInfo[] vstemplate_project_files =
                GetVSTemplateFiles(xml_projects_src.Value);

            FileInfo[] vstemplate_item_files =
                GetVSTemplateFiles(xml_items_src.Value);

            XElement vs = xml.Element(
                "VisualStudioTemplateFolders");

            string[] vs_proj_tmps = vs.Element(
                "CustomProjectTemplatesLocations").Elements(
                "Path").Select(n => n.Value).ToArray();

            string[] vs_item_tmps = vs.Element(
                "CustomItemTemplatesLocations").Elements(
                "Path").Select(n => n.Value).ToArray();

            var dirs1 = DoWork(dict, vstemplate_project_files,
                vs_proj_tmps);

            var dirs2 = DoWork(dict, vstemplate_item_files,
                vs_item_tmps);

            var dirs_common = dirs1.Union(dirs2).Distinct()
                .OrderBy(n => n).ToArray();

            if (dirs_common.Length > 0) {

                Console.ForegroundColor = ConsoleColor
                                .Yellow;
                Console.WriteLine("Skeeped target directories:"
                    );

                foreach (var out_dir in dirs_common) {
                    Console.WriteLine(out_dir);
                }
            }
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

            DirectoryInfo dir = new DirectoryInfo(root_dir);

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

        static void RecoveryCustomParameters(string file_name) {
            string default_data =
@"<?xml version='1.0' encoding='utf-8' ?>
<Settings>
  <!-- RevitAddInTemplateSet code sources. -->
  <RevitAddInTemplateSet_Src>
    <ProjectTemplatesRootFolder>..\..\C# Project templates</ProjectTemplatesRootFolder>
    <ItemTemplatesRootFolder>..\..\C# Item Templates</ItemTemplatesRootFolder>
  </RevitAddInTemplateSet_Src>
  <!-- The target directories for ZIP-files copying. -->
  <VisualStudioTemplateFolders>
    <!-- The locations of Visual Studio Project Templates. -->
    <CustomProjectTemplatesLocations>
      <Path>%USERPROFILE%\Documents\Visual Studio 2013\Templates\ProjectTemplates\Visual C#</Path>
      <Path>%USERPROFILE%\Documents\Visual Studio 2015\Templates\ProjectTemplates\Visual C#</Path>
      <Path>%USERPROFILE%\Documents\Visual Studio 2017\Templates\ProjectTemplates\Visual C#</Path>
    </CustomProjectTemplatesLocations>
    <!-- The locations of Visual Studio Item Templates. -->
    <CustomItemTemplatesLocations>
      <Path>%USERPROFILE%\Documents\Visual Studio 2013\Templates\ItemTemplates\Visual C#</Path>
      <Path>%USERPROFILE%\Documents\Visual Studio 2015\Templates\ItemTemplates\Visual C#</Path>
      <Path>%USERPROFILE%\Documents\Visual Studio 2017\Templates\ItemTemplates\Visual C#</Path>
    </CustomItemTemplatesLocations>
  </VisualStudioTemplateFolders>
  <!-- Custom parameters which are to be updated for each vstemplate-file. -->
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
