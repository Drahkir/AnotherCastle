using System.Collections.Generic;
using System.Linq;
using Engine;

namespace AnotherCastle
{
    public class ClientBounds
    {
        public int NorthBound;
        public int SouthBound;
        public int WestBound;
        public int EastBound;
    }

    public class EnemyManager
    {
        readonly List<Enemy> _enemies = new List<Enemy>();

        public List<Enemy> EnemyList
        {
            get
            {
                return _enemies;
            }
        }

        public void Update(double elapsedTime, double gameTime)
        {
            _enemies.ForEach(x => x.Update(elapsedTime));
            CheckForOutOfBounds();
            RemoveDeadEnemies();
        }

        private void CheckForOutOfBounds()
        {
            foreach (var enemy in _enemies.Where(enemy => enemy.IsPathDone()))
            {
                enemy.Health = 0;
            }
        }

        public void Render(Renderer renderer)
        {
            _enemies.ForEach(x => x.Render(renderer));
        }

        private void RemoveDeadEnemies()
        {
            for (var i = _enemies.Count - 1; i >= 0; i--)
            {
                if (_enemies[i].IsDead)
                {
                    _enemies.RemoveAt(i);
                }
            }
        }
    }
}