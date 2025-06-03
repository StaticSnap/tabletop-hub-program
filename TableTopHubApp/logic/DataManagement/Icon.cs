namespace TableTopHubApp
{
    internal class Icon
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string FilePath { get; set; }

        public string Type { get; set; }

        public string Width { get; set; } = "1";

        public string Height { get; set; } = "1";
    }
}
