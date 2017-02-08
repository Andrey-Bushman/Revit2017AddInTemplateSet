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
	
  static class Tools {

        public static string GetResourceString(
        	Type cmd_type,
        	Type default_resources_type,
        	string key){

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

        	string value = res_mng?.GetString(key);

        	if (string.IsNullOrEmpty(value)){

        		value = default_res_mng?.GetString(key);
        	}

        	res_mng?.ReleaseAllResources();

        	if (default_res_mng != res_mng) {
                default_res_mng?.ReleaseAllResources();
            }

        	return value;
        }

        public static BitmapSource GetResourceImage(
        	Type cmd_type,
        	Type default_resources_type,
        	string key){

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

        	Bitmap ttp_image = null;

        	if (res_mng != null){
	        	ttp_image = (Bitmap)res_mng?.GetObject(key);
	        }

        	if (ttp_image == null){

        		ttp_image = (Bitmap)default_res_mng?.GetObject(
        			key);
        	}

        	res_mng?.ReleaseAllResources();

        	if (default_res_mng != res_mng) {
                default_res_mng?.ReleaseAllResources();
            }

        	if (ttp_image == null){
        		return null;
        	}
        	else{
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
