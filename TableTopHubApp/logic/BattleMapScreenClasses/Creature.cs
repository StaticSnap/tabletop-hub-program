namespace TableTopHubApp
{
    internal class Creature
    {
        private string name;
        private string stats;
        private string actions;
        private int maxHP;
        private int curHP;

        internal Creature(string name, string stats, string actions, int maxHP)
        {
            this.name = name;
            this.stats = stats;
            this.actions = actions;
            this.maxHP = maxHP;
            this.curHP = maxHP;
        }

        public string Name
        {
            get =>this.name;
        }

        public string Stats
        {
            get => this.stats;
        }

        public string Actions
        {
            get => this.actions;
        }

        public int MaxHP
        {
            get => this.maxHP;
        }

        public int CurHP
        {
            get => this.curHP;
            set => this.curHP = value;
        }
    }
}
