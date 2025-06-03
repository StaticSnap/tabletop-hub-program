// <copyright file="Track.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    internal class Track
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string LoopPath { get; set; }

        public string IntroPath { get; set; } = "NULL";
    }
}
