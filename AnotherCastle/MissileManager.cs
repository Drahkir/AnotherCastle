using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Engine;

namespace AnotherCastle
{
    public class MissileManager
    {
        List<Missile> _missiles = new List<Missile>();
        List<Missile> _enemyMissiles = new List<Missile>();

        RectangleF _bounds;

        public MissileManager(RectangleF playArea)
        {
            _bounds = playArea;
        }

        public void Shoot(Missile bullet)
        {
            _missiles.Add(bullet);
        }

        public void EnemyShoot(Missile missile)
        {
            _enemyMissiles.Add(missile);
        }

        public void UpdatePlayerCollision(PlayerCharacter playerCharacter)
        {
            foreach (Missile missile in _enemyMissiles)
            {
                if (missile.GetBoundingBox().IntersectsWith(playerCharacter.GetBoundingBox()))
                {
                    missile.Dead = true;
                    playerCharacter.OnCollision(missile);
                }
            }
        }

        public void Update(double elapsedTime)
        {
            UpdateMissileList(_missiles, elapsedTime);
            UpdateMissileList(_enemyMissiles, elapsedTime);
        }

        public void UpdateMissileList(List<Missile> bulletList, double elapsedTime)
        {
            bulletList.ForEach(x => x.Update(elapsedTime));
            CheckOutOfBounds(_missiles);
            RemoveDeadBullets(bulletList);
        }

        private void CheckOutOfBounds(List<Missile> missileList)
        {
            foreach (Missile missile in missileList)
            {
                if (!missile.GetBoundingBox().IntersectsWith(_bounds))
                {
                    missile.Dead = true;
                }
            }
        }

        private void RemoveDeadBullets(List<Missile> bulletList)
        {
            //foreach(Bullet bullet in bulletList)
            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                //if(bullet.Dead)
                if (bulletList[i].Dead)
                {
                    //bulletList.Remove(bullet);
                    bulletList.RemoveAt(i);
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
            foreach (Missile bullet in _missiles)
            {
                if (bullet.GetBoundingBox().IntersectsWith(enemy.GetBoundingBox()))
                {
                    bullet.Dead = true;
                    enemy.OnCollision(bullet);
                }
            }

        }
    }

}
