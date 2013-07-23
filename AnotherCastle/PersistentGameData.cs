using System.Drawing;

namespace AnotherCastle
{
    class PersistentGameData
    {
        public bool JustWon { get; set; }
        public LevelDescription CurrentLevel { get; set; }

        public PersistentGameData()
        {
              JustWon = false;
        }
    }
}
