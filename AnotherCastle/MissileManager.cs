using System.Collections.Generic;
using Engine;

namespace AnotherCastle
{
    public class MissileManager
    {
        private readonly List<EnemyMissile> _enemyMissiles = new List<EnemyMissile>();
        private readonly List<Missile> _missiles = new List<Missile>();
        private static MissileManager instance;

        private MissileManager() { }

        public static MissileManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MissileManager();
                }
                return instance;
            }
        }

        public List<Missile> MissileList
        {
            get { return _missiles; }
        }

        public List<EnemyMissile> EnemyMissileList
        {
            get { return _enemyMissiles; }
        }

        public void Shoot(Missile missile)
        {
            _missiles.Add(missile);
        }

        public void EnemyShoot(EnemyMissile missile)
        {
            _enemyMissiles.Add(missile);
        }

        public void Update(double elapsedTime)
        {
            UpdateMissileList(_missiles, elapsedTime);
            UpdateMissileList(_enemyMissiles, elapsedTime);
        }

        public void UpdateMissileList(IList<Missile> missileList, double elapsedTime)
        {
            foreach (var missile in missileList)
            {
                missile.Update(elapsedTime);
            }
            //CheckOutOfBounds(_missiles);
            RemoveDeadMissile(missileList);
        }

        private static void RemoveDeadMissile(IList<Missile> missileList)
        {
            for (int i = missileList.Count - 1; i >= 0; i--)
            {
                if (missileList[i].Dead)
                {
                    missileList.RemoveAt(i);
                }
            }
        }

        public void UpdateMissileList(IList<EnemyMissile> missileList, double elapsedTime)
        {
            foreach (var missile in missileList)
            {
                missile.Update(elapsedTime);
            }
            RemoveDeadMissile(missileList);
        }

        private static void RemoveDeadMissile(IList<EnemyMissile> missileList)
        {
            for (int i = missileList.Count - 1; i >= 0; i--)
            {
                //if(missile.Dead)
                if (missileList[i].Dead)
                {
                    missileList.RemoveAt(i);
                }
            }
        }

        internal void Render(Renderer renderer)
        {
            _missiles.ForEach(x => x.Render(renderer));
            _enemyMissiles.ForEach(x => x.Render(renderer));
        }
    }
}