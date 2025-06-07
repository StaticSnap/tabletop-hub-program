// <copyright file="MapManager.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Manager class to open map and icon data files.
    /// </summary>
    internal static class MapManager
    {
        private static List<ManifestEntry> manifestMaps = StorageManager.GetAllMapEntries();
        private static List<ManifestEntry> manifestIcons = StorageManager.GetAllIconEntries();

        /// <summary>
        /// Open up the map data file and read all the data inside.
        /// </summary>
        public static void InitMaps()
        {
            manifestMaps = StorageManager.GetAllMapEntries();
            manifestIcons = StorageManager.GetAllIconEntries();
        }

        /// <summary>
        /// Gets the dictionary of map name to data. 
        /// </summary>
        /// <returns>array of map data.</returns>
        public static List<ManifestEntry> GetMaps()
        {
            return manifestMaps;
        }

        /// <summary>
        /// Helper function to retrieve a loaded map object from an id.
        /// </summary>
        /// <param name="id">The id of the map to retrieve.</param>
        /// <returns>The loaded map object.</returns>
        /// <exception cref="Exception">The program should never try an id that won't work.</exception>
        public static Map GetLoadedMap(string id)
        {
            Map? map = StorageManager.LoadMapObject(id);

            if (map == null)
            {
                throw new Exception("map not found");
            }

            return map;
        }

        /// <summary>
        /// Gets the path associated with a specific icon.
        /// </summary>
        /// <param name="icone">The icon to use..</param>
        /// <returns>string path.</returns>
        public static string GetIconPath(Icon icon)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\icons\\", icon.FilePath);
        }

        /// <summary>
        /// Uses an icons name in order to retrieve data on it. If there is no data then return empty strings.
        /// </summary>
        /// <param name="id">the name of the icon.</param>
        /// <returns>the statistics associated with that icon.</returns>
        public static Icon GetLoadedIcon(string id)
        {
            Icon? icon = StorageManager.LoadIconObject(id);

            if (icon == null)
            {
                throw new Exception("icon not found");
            }

            return icon;
        }

        /// <summary>
        /// Gets the list of icons for UI.
        /// </summary>
        /// <returns>List of manifest entries on icons.</returns>
        public static List<ManifestEntry> GetIcons()
        {
            return manifestIcons;
        }
    }
}
