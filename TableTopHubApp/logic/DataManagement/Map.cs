// <copyright file="Map.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    internal class Map
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string FilePath { get; set; }

        public string Width { get; set; } = "1";

        public string Height { get; set; } = "1";

        public string Type { get; set; } = "STATIC";
    }
}
