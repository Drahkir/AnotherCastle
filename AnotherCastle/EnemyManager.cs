using System.Collections.Generic;
using Engine;

namespace AnotherCastle
{
    public class ClientBounds
    {
        public int EastBound;
        public int NorthBound;
        public int SouthBound;
        public int WestBound;
    }

    public class EnemyManager
    {
        private readonly List<Enemy> _enemies = new List<Enemy>();

        public List<Enemy> EnemyList
        {
            get { return _enemies; }
        }

        public void Update(double elapsedTime, double gameTime)
        {
            _enemies.ForEach(x => x.Update(elapsedTime));
            RemoveDeadEnemies();
        }

        public void Render(Renderer renderer)
        {
            _enemies.ForEach(x => x.Render(renderer));
        }

        private void RemoveDeadEnemies()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                if (_enemies[i].IsDead)
                {
                    _enemies.RemoveAt(i);
                }
            }
        }
    }
}