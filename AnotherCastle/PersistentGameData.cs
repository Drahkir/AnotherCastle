namespace AnotherCastle
{
    internal class PersistentGameData
    {
        public PersistentGameData()
        {
            JustWon = false;
        }

        public bool JustWon { get; set; }
        public LevelDescription CurrentLevel { get; set; }
    }
}