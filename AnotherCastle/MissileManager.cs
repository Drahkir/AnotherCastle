﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Engine;

namespace AnotherCastle
{
    public class MissileManager
    {
        private readonly RectangleF _bounds;
        private readonly List<Missile> _enemyMissiles = new List<Missile>();
        private readonly List<Missile> _missiles = new List<Missile>();

        public MissileManager(RectangleF playArea)
        {
            _bounds = playArea;
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

        public void UpdatePlayerCollision(PlayerCharacter playerCharacter)
        {
            foreach (
                Missile missile in
                    _enemyMissiles.Where(
                        missile => missile.GetBoundingBox().IntersectsWith(playerCharacter.GetBoundingBox())))
            {
                missile.Dead = true;
                playerCharacter.OnCollision(missile);
            }
        }

        public void Update(double elapsedTime)
        {
            UpdateMissileList(_missiles, elapsedTime);
            UpdateMissileList(_enemyMissiles, elapsedTime);
        }

        public void UpdateMissileList(List<Missile> missileList, double elapsedTime)
        {
            missileList.ForEach(x => x.Update(elapsedTime));
            CheckOutOfBounds(_missiles);
            RemoveDeadMissile(missileList);
        }

        private void CheckOutOfBounds(IEnumerable<Missile> missileList)
        {
            foreach (Missile missile in missileList.Where(missile => !missile.GetBoundingBox().IntersectsWith(_bounds)))
            {
                missile.Dead = true;
            }
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

        internal void UpdateEnemyCollisions(Enemy enemy)
        {
            foreach (
                Missile missile in
                    _missiles.Where(missile => missile.GetBoundingBox().IntersectsWith(enemy.GetBoundingBox())))
            {
                missile.Dead = true;
                enemy.OnCollision(missile);
            }
        }
    }
}