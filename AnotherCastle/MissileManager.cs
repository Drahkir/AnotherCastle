using System.Collections.Generic;
using Engine;

namespace AnotherCastle
{
    public class MissileManager
    {
        private readonly List<Missile> _enemyMissiles = new List<Missile>();
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

        public void Shoot(Missile missile)
        {
            _missiles.Add(missile);
        }

        public void EnemyShoot(Missile missile)
        {
            _enemyMissiles.Add(missile);
        }

        public void Update(double elapsedTime)
        {
            UpdateMissileList(_missiles, elapsedTime);
            UpdateMissileList(_enemyMissiles, elapsedTime);
        }

        public void UpdateMissileList(List<Missile> missileList, double elapsedTime)
        {
            missileList.ForEach(x => x.Update(elapsedTime));
            //CheckOutOfBounds(_missiles);
            RemoveDeadMissile(missileList);
        }

        private static void RemoveDeadMissile(IList<Missile> missileList)
        {
            //foreach(Missile missile in missileList)
            for (int i = missileList.Count - 1; i >= 0; i--)
            {
                //if(missile.Dead)
                if (missileList[i].Dead)
                {
                    //missileList.Remove(missile);
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