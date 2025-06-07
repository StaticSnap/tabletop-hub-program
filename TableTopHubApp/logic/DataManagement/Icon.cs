namespace TableTopHubApp
{
    internal class Icon
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string FilePath { get; set; }

        public string Type { get; set; }

        public int Width { get; set; } = 1;

        public int Height { get; set; } = 1;

        public string Stats { get; set; }

        public string Attacks { get; set; }

        public int MaxHealth { get; set; } = 1;
    }
}
