using Engine;

namespace AnotherCastle
{
    public class EnemyDef
    {
        public EnemyDef()
        {
            EnemyType = "cannon_fodder";
            LaunchTime = 0;
            SpawnPoint = RandomSpawnPoint();
        }

        public EnemyDef(string enemyType, double launchTime)
        {
            EnemyType = enemyType;
            LaunchTime = launchTime;
            SpawnPoint = RandomSpawnPoint();
        }

        public EnemyDef(string enemyType, double launchTime, Vector spawnPoint)
        {
            EnemyType = enemyType;
            LaunchTime = launchTime;
            SpawnPoint = spawnPoint;
        }

        public string EnemyType { get; set; }
        public double LaunchTime { get; set; }
        public Vector SpawnPoint { get; set; }

        public Vector RandomSpawnPoint()
        {
            return new Vector(0, 0, 0);
        }
    }
}