using System;
using Engine;

namespace AnotherCastle
{
    public class NorthSouthSkeleton : Enemy
    {
        /// <summary>
        /// Constructs an enemy given the texture and AI (IEnemyBrain)
        /// </summary>
        /// <param name="texture">The texture for the enemy</param>
        /// <param name="enemyBrain">The AI for the enemy</param>
        /// <param name="position">The spawn point for this enemy</param>
        public NorthSouthSkeleton(Texture texture, IEnemyBrain enemyBrain, Vector position) : base(texture, enemyBrain, position)
        {
            Health = 15;
            Damage = 10;
            Sprite.SetScale(1.8, 1.8);
            Speed = 200;
            MaxTimeToShoot = 12;
            MinTimeToShoot = 1;
            RestartShootCountDown();
        }
    }
}
