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

        private static string dataTrackDir = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\Track");
        private static string manifestTrackPath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\TrackManifest.json");
        private static List<ManifestEntry> manifestTrack = new();

        private static string dataMapDir = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\Map");
        private static string manifestMapPath = Path.Combine(Directory.GetCurrentDirectory(), "resources\\data\\MapManifest.json");
        private static List<ManifestEntry> manifestMap = new();

        public static void Init()
        {
            if (!Path.Exists(dataSoundDir))
            { 
                Directory.CreateDirectory(dataSoundDir);
            }

            if (!Path.Exists(dataTrackDir))
            {
                Directory.CreateDirectory(dataTrackDir);
            }

            if (!Path.Exists(dataMapDir))
            {
                Directory.CreateDirectory(dataMapDir);
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

            if (File.Exists(manifestTrackPath))
            {
                string json = File.ReadAllText(manifestTrackPath);
                manifestTrack = JsonSerializer.Deserialize<List<ManifestEntry>>(json) ?? new();
            }

            if (File.Exists(manifestMapPath))
            {
                string json = File.ReadAllText(manifestMapPath);
                manifestMap = JsonSerializer.Deserialize<List<ManifestEntry>>(json) ?? new();
            }
        }

        private static void SaveSoundManifest()
        {
            string json = JsonSerializer.Serialize(manifestSound, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(manifestSoundPath, json);
        }

        private static void SaveTrackManifest()
        {
            string json = JsonSerializer.Serialize(manifestTrack, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(manifestTrackPath, json);
        }

        private static void SaveMapManifest()
        {
            string json = JsonSerializer.Serialize(manifestMap, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(manifestMapPath, json);
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
            SaveSoundManifest();
        }

        public static void SaveObject(Track obj)
        {
            string fileName = $"{obj.Id}.gz";
            string filePath = Path.Combine(dataTrackDir, fileName);

            using FileStream fs = new(filePath, FileMode.Create);
            using GZipStream gzip = new(fs, CompressionLevel.Optimal);
            JsonSerializer.Serialize(gzip, obj);

            manifestTrack.RemoveAll(e => e.Id == obj.Id);
            manifestTrack.Add(new ManifestEntry { Id = obj.Id, Name = obj.Name, FileName = fileName });
            SaveTrackManifest();
        }

        public static void SaveObject(Map obj)
        {
            string fileName = $"{obj.Id}.gz";
            string filePath = Path.Combine(dataMapDir, fileName);

            using FileStream fs = new(filePath, FileMode.Create);
            using GZipStream gzip = new(fs, CompressionLevel.Optimal);
            JsonSerializer.Serialize(gzip, obj);

            manifestMap.RemoveAll(e => e.Id == obj.Id);
            manifestMap.Add(new ManifestEntry { Id = obj.Id, Name = obj.Name, FileName = fileName });
            SaveMapManifest();
        }

        public static SoundEffect? LoadSoundObject(string id)
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

        public static Track? LoadTrackObject(string id)
        {
            ManifestEntry? entry = manifestTrack.Find(e => e.Id == id);
            if (entry == null)
            {
                return null;
            }

            string filePath = Path.Combine(dataTrackDir, entry.FileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            using FileStream fs = new(filePath, FileMode.Open);
            using GZipStream gzip = new(fs, CompressionMode.Decompress);
            return JsonSerializer.Deserialize<Track>(gzip);
        }

        public static Map? LoadMapObject(string id)
        {
            ManifestEntry? entry = manifestMap.Find(e => e.Id == id);
            if (entry == null)
            {
                return null;
            }

            string filePath = Path.Combine(dataMapDir, entry.FileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            using FileStream fs = new(filePath, FileMode.Open);
            using GZipStream gzip = new(fs, CompressionMode.Decompress);
            return JsonSerializer.Deserialize<Map>(gzip);
        }

        public static bool DeleteObject(string id)
        {
            ManifestEntry? entry = manifestSound.Find(e => e.Id == id);
            if (entry != null)
            {
                string filePath = Path.Combine(dataSoundDir, entry.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                manifestSound.RemoveAll(e => e.Id == id);
                SaveSoundManifest();
                return true;
            }

            entry = manifestTrack.Find(e => e.Id == id);
            if (entry != null)
            {
                string filepath = Path.Combine(dataTrackDir , entry.FileName);
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                manifestTrack.RemoveAll(e => e.Id == id);
                SaveTrackManifest();
                return true;
            }

            entry = manifestMap.Find(e => e.Id == id);
            if (entry != null)
            {
                string filepath = Path.Combine(dataMapDir, entry.FileName);
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                manifestMap.RemoveAll(e => e.Id == id);
                SaveMapManifest();
                return true;
            }

            return false;
        }

        public static List<ManifestEntry> GetAllSoundEntries()
        {
            return new List<ManifestEntry>(manifestSound);
        }

        public static List<ManifestEntry> GetAllTrackEntries()
        {
            return new List<ManifestEntry>(manifestTrack);
        }

        public static List<ManifestEntry> GetAllMapEntries()
        {
            return new List<ManifestEntry>(manifestMap);
        }
    }
}
