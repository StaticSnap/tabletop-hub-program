namespace TableTopHubApp
{
    class AmbientEffect
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public List<SoundInfo> Sounds { get; set; }
    }
}
