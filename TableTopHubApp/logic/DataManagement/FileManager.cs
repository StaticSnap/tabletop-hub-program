// <copyright file="FileManager.cs" company="StaticSnap">
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
    using Microsoft.Win32;

    /// <summary>
    /// File Manager class handles all of the IO associated with writing xaml data for resources and copying them to resources folder upon selection.
    /// </summary>
    internal static class FileManager
    {
        private static string currentFilePath = string.Empty;
        private static string currentIntroPath = string.Empty;

        /// <summary>
        /// Gets the current file path.
        /// </summary>
        public static string CurrentFilePath
        {
            get { return currentFilePath; }
        }

        /// <summary>
        /// Gets the current intro path.
        /// </summary>
        public static string CurrentIntroPath
        {
            get { return currentIntroPath; }
        }

        /// <summary>
        /// Sets the paths in storage to emptry string for resetting.
        /// </summary>
        public static void ClearBuffers()
        {
            currentFilePath = string.Empty;
            currentIntroPath = string.Empty;
        }

        /// <summary>
        /// Helper function called at the beginign of runtime to generate resource files if they are missing.
        /// </summary>
        public static void EnsurePathways()
        {
            if(!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\data")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\data"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicIntroFolder")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicIntroFolder"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\soundEffectsFolder")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\soundEffectsFolder"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\videos")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\videos"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\icons")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\icons"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps"));
            }

            if (!Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\overlays")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\overlays"));
            }
        }

        /// <summary>
        /// Opens the file dialog and logs the result.
        /// </summary>
        /// <param name="type">the types of files allowed.</param>
        /// <returns>true on success.</returns>
        /// <exception cref="Exception">throw an exception when unsuported type is encountered.</exception>
        public static bool OpenFile(string type)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if(type == "icon")
            {
                dialog.Filter = "Image Files|*.bmp;*.jpeg;*.jpg;*.png;*.gif";
            }
            else if(type == "music" | type == "sound")
            {
                dialog.Filter = "Audio Files|*.mp3;*.wav;*.mp2";
            }
            else if(type == "overlay")
            {
                dialog.Filter = "Visual Files|*.bmp;*.jpeg;*.jpg;*.png;*.gif;*.mp4;*.mov";
            }
            else if(type == "map")
            {
                dialog.Filter = "Image Files|*.bmp;*.jpeg;*.jpg;*.png;*.gif";
            }
            else
            {
                currentFilePath = string.Empty;
                throw new Exception("invalid file open option");
            }

            if(dialog.ShowDialog() == true)
            {
                currentFilePath = dialog.FileName;

                // no comas allowed punk
                if (currentFilePath.Contains(',')) 
                {
                    currentFilePath = string.Empty;
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Second version of the open file method used for only the intro file as you need to store two files.
        /// </summary>
        /// <returns>true if successful.</returns>
        public static bool OpenIntroFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Audio Files|*.mp3;*.wav;*.mp2";

            if (dialog.ShowDialog() == true)
            {
                currentIntroPath = dialog.FileName;

                // no comas allowed punk
                if (currentIntroPath.Contains(','))
                {
                    currentIntroPath = string.Empty;
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the file from it's origional location to the resources folder for later use.
        /// </summary>
        /// <param name="type">the type of resource determines where it will be placed.</param>
        /// <returns>true if file was copied and fals eif file already exists.</returns>
        public static bool CopyFile(string type)
        {
            string finalLocation = string.Empty;

            if (File.Exists(currentFilePath) || (File.Exists(currentIntroPath) && type == "intro"))
            {
                if (type == "music")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder");
                }
                else if(type == "intro")
                {
                    if(CurrentIntroPath == string.Empty)
                    {
                        return true;
                    }

                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicIntroFolder");

                    for (int i = 0; i < Directory.GetFiles(finalLocation).Count(); i++)
                    {
                        if (Path.GetFileName(Directory.GetFiles(finalLocation)[i]) == Path.GetFileName(CurrentIntroPath))
                        {
                            return false;
                        }
                    }

                    finalLocation = Path.Combine(finalLocation, Path.GetFileName(currentIntroPath));

                    File.Copy(currentIntroPath, finalLocation);
                    currentIntroPath = string.Empty;
                    return true;
                }
                else if (type == "sound")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\soundEffectsFolder");
                }
                else if (type == "icon")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\icons");
                }
                else if (type == "overlay")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\overlays");
                }
                else if (type == "map")
                {
                    finalLocation = Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps");
                }
                else
                {
                    throw new Exception("unimplemented type");
                }

                for(int i = 0; i < Directory.GetFiles(finalLocation).Count(); i++)
                {
                    if (Path.GetFileName(Directory.GetFiles(finalLocation)[i]) == Path.GetFileName(CurrentFilePath))
                    {
                        return false;
                    }
                }
                
                finalLocation = Path.Combine(finalLocation, Path.GetFileName(currentFilePath));

                File.Copy(currentFilePath, finalLocation);
                currentFilePath = string.Empty;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes file data and stores it in respective data text file.
        /// </summary>
        /// <param name="type">type of resource.</param>
        /// <param name="data">data associated with that resource.</param>
        /// <exception cref="Exception">passed type which doesn't exists.</exception>
        public static void AddData(string type, string[] data)
        {
            if (type == "music")
            {
                Track newTrack = new Track { Name = data[0], LoopPath = data[1], IntroPath = data[2] };
                StorageManager.SaveObject(newTrack);
                return;
            }
            else if (type == "sound")
            {
                SoundEffect newSound = new SoundEffect { Name = data[0], FilePath = data[1] };
                StorageManager.SaveObject(newSound);
                return;
            }
            else if (type == "icon")
            {
                int width = 1;
                int height = 1;
                int health = 1;

                int.TryParse(data[3], out width);
                int.TryParse(data[4], out height);
                int.TryParse(data[7], out health);

                Icon newIcon = new Icon { Name = data[0], FilePath = data[1], Type = data[2], Width = width, Height = height, Stats = data[5], Attacks = data[6], MaxHealth = health };
                StorageManager.SaveObject(newIcon);
                return;
            }
            else if (type == "overlay")
            {
                Overlay newOverlay = new Overlay { Name = data[0], FilePath = data[1], Type = data[2], Looping = data[3], ChromaVal = data[4] };
                StorageManager.SaveObject(newOverlay);
                return;
            }
            else if (type == "map")
            {

                Map newMap = new Map { Name = data[0], FilePath = data[1], Width = data[2], Height = data[3] };

                if(Path.GetExtension(newMap.FilePath) == ".gif")
                {
                    newMap.Type = "ANIMATED";
                }

                StorageManager.SaveObject(newMap);
                return;
            }
            else
            {
                throw new Exception("unimplemented type");
            }
        }
    }
}
