// <copyright file="OverlayManager.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Data management class for overlay objects.
    /// </summary>
    internal static class OverlayManager
    {
        //format can be IMAGE, VIDEO, GIF
        //green screen value is either a hex code for the color to remove or NULL 
        //by default images and gifs remain on screen but a video will play only once if the loop value is not set to TRUE
        private static List<ManifestEntry> manifestOverlays = StorageManager.GetAllOverlayEntries();

        /// <summary>
        /// Read all data for overlay assets.
        /// </summary>
        public static void InitAssets()
        {
            manifestOverlays = StorageManager.GetAllOverlayEntries();
        }

        /// <summary>
        /// Formats path for program without having to make it worry about IO.
        /// </summary>
        /// <param name="overlay">Overlay object.</param>
        /// <returns>formatted file path.</returns>
        public static string GetOverlayPath(Overlay overlay)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\overlays\\", overlay.FilePath);
        }

        /// <summary>
        /// Helper function for the UI to fecth the file path for an overlay top display.
        /// </summary>
        /// <param name="id">The id of the overlay to find.</param>
        /// <returns>The absolute file path of the overlay.</returns>
        /// <exception cref="Exception">Storage cannot find the desired overlay.</exception>
        public static string GetOverlayPath(string id)
        {
            Overlay? overlayData = StorageManager.LoadOverlayObject(id);

            if(overlayData == null)
            {
                throw new Exception("Overlay not found");
            }
            else
            {
                return overlayData.FilePath;
            }
        }

        /// <summary>
        /// Helper function for the UI to fetch the name for an overlay using its id.
        /// </summary>
        /// <param name="id">The id of the overlay to find.</param>
        /// <returns>The name of the overlay.</returns>
        /// <exception cref="Exception">Storage cannot find the desired overlay.</exception>
        public static string GetOverlayName(string id)
        {
            Overlay? overlayData = StorageManager.LoadOverlayObject(id);

            if (overlayData == null)
            {
                throw new Exception("Overlay not found");
            }
            else
            {
                return overlayData.Name;
            }
        }

        /// <summary>
        /// Gets all of the manifest entries for the Overlays.
        /// </summary>
        /// <returns>List of manifest entries.</returns>
        public static List<ManifestEntry> GetOverlays()
        {
            return manifestOverlays;
        }
    }
}
