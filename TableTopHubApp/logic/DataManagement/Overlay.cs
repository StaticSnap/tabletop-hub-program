namespace TableTopHubApp
{
    class Overlay
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string FilePath { get; set; }

        public string Type { get; set; }

        public string Looping { get; set; } = "FALSE";

        public string ChromaVal { get; set; } = "NULL";
    }
}
