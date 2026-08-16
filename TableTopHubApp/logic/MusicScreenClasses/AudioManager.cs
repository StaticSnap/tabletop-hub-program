// <copyright file="AudioManager.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using TableTopHubApp.ui;

    /// <summary>
    /// Manager for audio handles data reading and file opening.
    /// </summary>
    internal static class AudioManager
    {
        private static List<ManifestEntry> manifestTracks = StorageManager.GetAllTrackEntries();
        private static List<ManifestEntry> manifestSounds = StorageManager.GetAllSoundEntries();
        private static List<ManifestEntry> manifestAmbiance = StorageManager.GetAllAmbianceEntries();

        /// <summary>
        /// Opens the data files and fills the dictionaries with data pertaining to the tracks and sounds.
        /// </summary>
        public static void InitTracks()
        {
            manifestTracks = StorageManager.GetAllTrackEntries();
            manifestSounds = StorageManager.GetAllSoundEntries();
            manifestAmbiance = StorageManager.GetAllAmbianceEntries();
        }

        /// <summary>
        /// Gets the dictionary of tracks containing all data on a track.
        /// </summary>
        /// <returns>Dictionary of song title to data.</returns>
        public static List<ManifestEntry> GetTracks()
        {
            return manifestTracks;
        }

        /// <summary>
        /// Gets the dictionary of sound effects containing all data on a sound.
        /// </summary>
        /// <returns>Dictionary of sound effect titles to data.</returns>
        public static List<ManifestEntry> GetSoundEffects()
        {
            return manifestSounds;
        }

        /// <summary>
        /// Gets the dictionary of ambiance containing all data on an ambiance object.
        /// </summary>
        /// <returns>Dictionary of ambiance titles to data.</returns>
        public static List<ManifestEntry> GetAmbiance()
        {
            return manifestAmbiance;
        }

        /// <summary>
        /// Takes a track name and gets it's corresponding file paths.
        /// </summary>
        /// <param name="trackId">id of the track.</param>
        /// <returns>string path formatted to be ready to use.</returns>
        /// <exception cref="Exception">if function is called on a track that is not in the data then raise an exception.</exception>
        public static string[]? GetTrackPath(string trackId)
        {
            Track? trackData = StorageManager.LoadTrackObject(trackId);

            if (trackData == null)
            {
                return null;
            }
            else if(trackData.IntroPath == "NULL")
            {
                return [Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder\\", trackData.LoopPath), "NULL"];
            }
            else
            {
                return [Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicFolder\\", trackData.LoopPath), Path.Combine(Directory.GetCurrentDirectory(), "resources\\musicIntroFolder\\", trackData.IntroPath)];
            }
        }

        public static string GetTrackTitle(string trackId)
        {
            Track? trackData = StorageManager.LoadTrackObject(trackId);

            if(trackData == null)
            {
                throw new Exception("song not found");
            }
            else
            {
                return trackData.Name;
            }
        }

        public static string GetSoundTitle(string soundId)
        {
            SoundEffect? soundData = StorageManager.LoadSoundObject(soundId);

            if (soundData == null)
            {
                throw new Exception("Sound not found");
            }
            else
            {
                return soundData.Name;
            }
        }

        /// <summary>
        /// Retrieves the loaded ambienteffect object from the id associated with it.
        /// </summary>
        /// <param name="ambID">ID for the ambient effect you're looking for.</param>
        /// <returns>a complete AmbientEffect record.</returns>
        /// <exception cref="Exception">If the ID isn't associated with any ambiance then throw.</exception>
        public static AmbientEffect GetAmbientEffect(string ambID)
        {
            AmbientEffect? ambData = StorageManager.LoadAmbianceObject(ambID);

            if (ambData == null)
            {
                throw new Exception("Ambiance not found");
            }
            else
            {
                return ambData;
            }
        }

        /// <summary>
        /// Takes a sond effect and gets its corresponding file path.
        /// </summary>
        /// <param name="soundEffectId">Id of the sound.</param>
        /// <returns>formatted file path.</returns>
        /// <exception cref="Exception">if given a name that the manager does not recognize. throw an exception.</exception>
        public static string? GetSoundEffectPath(string soundEffectId)
        {
            SoundEffect? soundData = StorageManager.LoadSoundObject(soundEffectId);

            if (soundData == null)
            {
                return null;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), "resources\\soundEffectsFolder\\", soundData.FilePath);
        }
    }
}
