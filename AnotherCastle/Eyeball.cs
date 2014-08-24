﻿using Engine;

namespace AnotherCastle
{
    public class Eyeball : Enemy
    {
        #region Missile Properties

        private const double FireRecovery = 0.25;
        private readonly MissileManager _missileManager;
        private readonly Texture _missileTexture;
        private double _elapsedTime;
        //public double FireRecovery { get; set; }

        private double _fireRecoveryTime = FireRecovery;
        private double _shootCountDown;

        #endregion Missile Properties

        /// <summary>
        ///     Constructs an enemy given the texture and AI (IEnemyBrain)
        /// </summary>
        /// <param name="texture">The texture for the enemy</param>
        /// <param name="enemyBrain">The AI for the enemy</param>
        /// <param name="position">The spawn point for this enemy</param>
        public Eyeball(Texture texture, IEnemyBrain enemyBrain, Vector position, Texture missileTexture,
            MissileManager missileManager) : base(texture, enemyBrain, position)
        {
            Health = 15;
            Damage = 5;
            Sprite.SetScale(1, 1);
            MaxTimeToShoot = 2;
            MinTimeToShoot = 1;
            _missileManager = missileManager;
            _missileTexture = missileTexture;
        }

        /// <summary>
        ///     Handles the on collision event
        /// </summary>
        /// <param name="playerCharacter">The player</param>
        public void Move(Vector amount)
        {
        }

        public override void Attack(double elapsedTime)
        {
            _elapsedTime += elapsedTime;

            if (_elapsedTime > 1)
            {
                _elapsedTime = 0;
                var dir1 = new Vector(1, 0, 0);
                var dir2 = new Vector(0, 1, 0);
                var dir3 = new Vector(-1, 0, 0);
                var dir4 = new Vector(0, -1, 0);

                var missile1 = new EnemyMissile(_missileTexture, dir1);
                var missile2 = new EnemyMissile(_missileTexture, dir2);
                var missile3 = new EnemyMissile(_missileTexture, dir3);
                var missile4 = new EnemyMissile(_missileTexture, dir4);

                missile1.SetPosition(Sprite.GetPosition() + dir1);
                missile2.SetPosition(Sprite.GetPosition() + dir2);
                missile3.SetPosition(Sprite.GetPosition() + dir3);
                missile4.SetPosition(Sprite.GetPosition() + dir4);

                _missileManager.EnemyShoot(missile1);
                _missileManager.EnemyShoot(missile2);
                _missileManager.EnemyShoot(missile3);
                _missileManager.EnemyShoot(missile4);
            }
        }

        public override void OnCollision(IEntity entity, Vector amount)
        {
            // Do nothing, Eyeball is a rock

            if (entity.GetType() == typeof (Missile))
            {
                var missile = entity as Missile;
                Health -= missile.Damage;
            }
        }
    }
}