// <copyright file="SoundEffect.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    /// <summary>
    /// Reference JSON object class for a sound effect.
    /// </summary>
    internal class SoundEffect
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string FilePath { get; set; }
    }
}
