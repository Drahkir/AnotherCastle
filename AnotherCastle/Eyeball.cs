using Engine;

namespace AnotherCastle
{
    public class Eyeball : Enemy
    {
        #region Missile Properties

        private MissileManager _missileManager;
        private Texture _missileTexture;
        private double _shootCountDown;
        public double FireRecovery { get; set; }

        #endregion Missile Properties

        /// <summary>
        ///     Constructs an enemy given the texture and AI (IEnemyBrain)
        /// </summary>
        /// <param name="texture">The texture for the enemy</param>
        /// <param name="enemyBrain">The AI for the enemy</param>
        /// <param name="position">The spawn point for this enemy</param>
        public Eyeball(Texture texture, IEnemyBrain enemyBrain, Vector position) : base(texture, enemyBrain, position)
        {
            Health = 15;
            Damage = 5;
            Sprite.SetScale(1, 1);
            MaxTimeToShoot = 2;
            MinTimeToShoot = 1;
        }

        /// <summary>
        ///     Handles the on collision event
        /// </summary>
        /// <param name="playerCharacter">The player</param>
        public void Move(Vector amount)
        {
        }

        public void Attack()
        {
            if (base.FireRecovery > 0)
            {
                return;
            }

            var dir1 = new Vector(1, 0, 0);
            var dir2 = new Vector(0, 1, 0);
            var dir3 = new Vector(0, 0, 0);
            var dir4 = new Vector(1, 1, 0);

            var missile1 = new Missile(_missileTexture, dir1);
            var missile2 = new Missile(_missileTexture, dir2);
            var missile3 = new Missile(_missileTexture, dir3);
            var missile4 = new Missile(_missileTexture, dir4);

            //missile.SetColor(new Color(0, 1, 0, 1));
            missile1.SetPosition(Sprite.GetPosition() + dir1);
            missile2.SetPosition(Sprite.GetPosition() + dir2);
            missile3.SetPosition(Sprite.GetPosition() + dir3);
            missile4.SetPosition(Sprite.GetPosition() + dir4);

            _missileManager.Shoot(missile1);
            _missileManager.Shoot(missile2);
            _missileManager.Shoot(missile3);
            _missileManager.Shoot(missile4);
        }

        public override void HandleCollision(Vector amount)
        {
            // Do nothing cause Eyeball should be like a rock!
        }
    }
}