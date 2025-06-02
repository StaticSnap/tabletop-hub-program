// <copyright file="StorageManager.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Text.Json;

    internal static class StorageManager
    {
        private static string dataSoundDir = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\SoundEffect");
        private static string manifestSoundPath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\SoundEffectManifest.json");
        private static List<ManifestEntry> manifestSound = new();

        public static void Init()
        {
            if (!Path.Exists(dataSoundDir))
            { 
                Directory.CreateDirectory(dataSoundDir);
            }

            LoadManifests();
        }

        private static void LoadManifests()
        {
            if (File.Exists(manifestSoundPath))
            {
                string json = File.ReadAllText(manifestSoundPath);
                manifestSound = JsonSerializer.Deserialize<List<ManifestEntry>>(json) ?? new();
            }
        }

        private static void SaveManifests()
        {
            string json = JsonSerializer.Serialize(manifestSound, new JsonSerializerOptions { WriteIndented = true});
            File.WriteAllText(manifestSoundPath, json);
        }

        public static void SaveObject(SoundEffect obj)
        {
            string fileName = $"{obj.Id}.gz";
            string filePath = Path.Combine(dataSoundDir, fileName);

            using FileStream fs = new(filePath, FileMode.Create);
            using GZipStream gzip = new(fs, CompressionLevel.Optimal);
            JsonSerializer.Serialize(gzip, obj);

            manifestSound.RemoveAll(e => e.Id == obj.Id);
            manifestSound.Add(new ManifestEntry {  Id = obj.Id, Name = obj.Name, FileName = fileName });
            SaveManifests();
        }

        public static SoundEffect? LoadObject(string id)
        {
            ManifestEntry? entry = manifestSound.Find(e => e.Id == id);
            if (entry == null)
            {
                return null;
            }

            string filePath = Path.Combine(dataSoundDir , entry.FileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            using FileStream fs = new(filePath, FileMode.Open);
            using GZipStream gzip = new(fs, CompressionMode.Decompress);
            return JsonSerializer.Deserialize<SoundEffect>(gzip);
        }

        public static bool DeleteObject(string id)
        {
            ManifestEntry? entry = manifestSound.Find(e => e.Id == id);
            if (entry == null)
            {
                return false;
            }

            string filePath = Path.Combine(dataSoundDir, entry.FileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            manifestSound.RemoveAll(e => e.Id == id);
            SaveManifests();
            return true;
        }

        public static List<ManifestEntry> GetAllEntries()
        {
            return new List<ManifestEntry>(manifestSound);
        }
    }
}
