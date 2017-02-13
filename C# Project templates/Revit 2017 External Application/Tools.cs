// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* $projectname$
 * Tools.cs
 * $WebPage$
 * © $CompanyName$, $year$
 *
 * The internal tools of the '$projectname$' add-in.
 */
#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Resources;
using $RootNamespace$.$safeprojectname$.Properties;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.IO;
using WPF = System.Windows;
using System.Linq;
#endregion

namespace $RootNamespace$.$safeprojectname${
	
    /// <summary>
    /// This class is for internal using by the Revit add-ins. 
    /// </summary>
    static class Tools {

        /// <summary>
        /// Build ribbon tabs, panels, and buttons for the 
        /// commands which are defined in an assembly.
        /// </summary>
        /// <param name="uic_app">A handle to the application 
        /// being started.</param>
        /// <param name="asm">Target assembly.</param>
        /// <param name="default_resources_type">The type which
        /// contains the default values of necessary resources.
        /// </param>
        public static void BuildUI(
            UIControlledApplication uic_app, Assembly asm,
            Type default_resources_type) {

            if (uic_app == null) {
                throw new ArgumentNullException(nameof(uic_app)
                    );
            }

            if (asm == null) {
                throw new ArgumentNullException(nameof(asm));
            }

            if (default_resources_type == null) {
                throw new ArgumentNullException(nameof(
                    default_resources_type));
            }

            var cmd_interface_name = typeof(IExternalCommand)
                        .FullName;

            // get all commands which are defined in this 
            // assembly
            var commands = asm.GetTypes().Where(n =>
                    n.GetInterface(cmd_interface_name) != null)
                    ;

            // Create the button for each command
            foreach (var cmd in commands) {

                bool result = false;


                if (bool.TryParse(GetResourceString(cmd,
                        default_resources_type,
                        "_auto_location"), out result) &&
                        result) {

                    // Add the button on the ribbon panel.
                    Tools.AddButton(uic_app, cmd,
                        default_resources_type);
                }
            }
        }

        /* Through the using of this dictionary I try to 
         * minimize the exceptions count. */
        static Dictionary<string, string> tabs_dict =
            new Dictionary<string, string>();

        /// <summary>
        /// Add the button to Revit UI for an external command.
        /// </summary>
        /// <param name="uic_app">A handle to the application 
        /// being started.</param>
        /// <param name="cmd_type">The command type. It must to
        /// implement the IExternalCommand interface.</param>
        /// <param name="default_resources_type">The type which
        /// contains the default values of necessary resources.
        /// </param>
        public static void AddButton(
            UIControlledApplication uic_app, Type cmd_type,
            Type default_resources_type) {

            if (uic_app == null) {
                throw new ArgumentNullException(nameof(uic_app)
                    );
            }

            if (cmd_type == null) {
                throw new ArgumentNullException(nameof(
                    cmd_type));
            }

            if (cmd_type.GetInterface(typeof(IExternalCommand)
                        .FullName) == null) {
                throw new ArgumentException(nameof(cmd_type));
            }

            if (default_resources_type == null) {
                throw new ArgumentNullException(nameof(
                    default_resources_type));
            }

            // The target ribbon tab name.
            string tab_name = GetResourceString(cmd_type,
                default_resources_type, "_Ribbon_tab_name");

            try {
                /* Through the using of the tabs_dict 
                 * dictionary I try to minimize the exceptions 
                 * count. */
                if (!tabs_dict.ContainsKey(tab_name)) {

                    uic_app.CreateRibbonTab(tab_name);
                    tabs_dict.Add(tab_name, tab_name);
                }
            }
            catch { }

            // The target ribbon panel name
            string panel_name = GetResourceString(cmd_type,
                default_resources_type, "_Ribbon_panel_name");

            RibbonPanel panel = uic_app.GetRibbonPanels(
                tab_name).FirstOrDefault(
                n => n.Name.Equals(panel_name, StringComparison
                .InvariantCulture));

            if (panel == null) {
                panel = uic_app.CreateRibbonPanel(tab_name,
                    panel_name);
            }

            string this_assembly_path = Assembly
                .GetExecutingAssembly().Location;

            // Get localized help file name
            string help_file = Path.Combine(Path
                .GetDirectoryName(this_assembly_path),
                GetResourceString(cmd_type,
                default_resources_type, "_Help_file_name"));

            ContextualHelp help = new ContextualHelp(
                ContextualHelpType.ChmFile, help_file);

            // Help topic id (inside of help file).
            help.HelpTopicUrl = GetResourceString(cmd_type,
                default_resources_type, "_Help_topic_Id");

            string cmd_name = cmd_type.Name;

            string cmd_text = GetResourceString(cmd_type,
                default_resources_type, "_Button_caption");

            string cmd_tooltip = GetResourceString(cmd_type,
                default_resources_type, "_Button_tooltip_text")
                ;

            string long_description = GetResourceString(
                cmd_type, default_resources_type,
                "_Button_long_description");

            PushButtonData button_data = new PushButtonData(
                cmd_name, cmd_text,
                this_assembly_path, cmd_type.FullName);

            // Get corresponding class name which implements 
            // the IExternalCommandAvailability interface
            var aviability_type = GetResourceString(cmd_type,
                default_resources_type, "_aviability_type");

            var av_type = Type.GetType(aviability_type);

            if (av_type != null && av_type.GetInterface(typeof(
                    IExternalCommandAvailability).FullName)
                    != null) {

                button_data.AvailabilityClassName = av_type
                    .FullName;
            }

            PushButton push_button = panel.AddItem(
                button_data) as PushButton;

            push_button.ToolTip = cmd_tooltip;

            BitmapSource btn_src_img = GetResourceImage(
                cmd_type, default_resources_type,
                "_Button_image");

            push_button.LargeImage = btn_src_img;

            BitmapSource ttp_bitmap_src = GetResourceImage(
                cmd_type, default_resources_type,
                "_Button_tooltip_image");

            push_button.ToolTipImage = ttp_bitmap_src;
            push_button.LongDescription = long_description;
            push_button.SetContextualHelp(help);
        }

        /// <summary>
        /// Get a string from the localized resources.
        /// </summary>
        /// <param name="cmd_type">The command type. It must to
        /// implement the IExternalCommand interface.</param>
        /// <param name="default_resources_type">The type which
        /// contains the default values of necessary resources.
        /// </param>
        /// <param name="key">Resource key.</param>
        /// <returns>It returns the localized string or null if
        /// found nothing.</returns>
        public static string GetResourceString(
            Type cmd_type,
            Type default_resources_type,
            string key) {

            if (cmd_type == null) {
                throw new ArgumentNullException(nameof(
                    cmd_type));
            }

            if (default_resources_type == null) {
                throw new ArgumentNullException(nameof(
                    default_resources_type));
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentException(nameof(
                    cmd_type));
            }

            ResourceManager res_mng = new ResourceManager(
                cmd_type);

            ResourceManager default_res_mng =
                new ResourceManager(default_resources_type);

            string value = res_mng.GetString(key);

            if (string.IsNullOrEmpty(value)) {

                value = default_res_mng.GetString(key);
            }

            res_mng.ReleaseAllResources();

            if (default_res_mng != res_mng) {
                default_res_mng.ReleaseAllResources();
            }

            return value;
        }

        /// <summary>
        /// Get an image from the localized resources.
        /// </summary>
        /// <param name="cmd_type">The command type. It must to
        /// implement the IExternalCommand interface.</param>
        /// <param name="default_resources_type">The type which
        /// contains the default values of necessary resources.
        /// </param>
        /// <param name="key">Resource key.</param>
        /// <returns>It returns the image or null if found 
        /// nothing.</returns>
        public static BitmapSource GetResourceImage(
            Type cmd_type,
            Type default_resources_type,
            string key) {

            if (cmd_type == null) {
                throw new ArgumentNullException(nameof(
                    cmd_type));
            }

            if (default_resources_type == null) {
                throw new ArgumentNullException(nameof(
                    default_resources_type));
            }

            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentException(nameof(
                    cmd_type));
            }

            ResourceManager res_mng = new ResourceManager(
                cmd_type);

            ResourceManager default_res_mng =
                new ResourceManager(default_resources_type);

            Bitmap ttp_image = res_mng.GetObject(key) as Bitmap
                ;

            if (ttp_image == null) {

                ttp_image = default_res_mng.GetObject(key) as
                    Bitmap;
            }

            res_mng.ReleaseAllResources();

            if (default_res_mng != res_mng) {

                default_res_mng.ReleaseAllResources();
            }

            if (ttp_image == null) {

                return null;
            }
            else {

                BitmapSource ttp_bitmap_src = Imaging
                    .CreateBitmapSourceFromHBitmap(
                    ttp_image.GetHbitmap(), IntPtr.Zero,
                    WPF.Int32Rect.Empty, BitmapSizeOptions
                    .FromEmptyOptions());

                return ttp_bitmap_src;
            }
        }
    }
}
