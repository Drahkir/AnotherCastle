using System;
using Engine;

namespace AnotherCastle
{
    public class Enemy : Entity
    {
        private readonly EffectsManager _effectsManager;
        private readonly IEnemyBrain _enemyBrain;
        private readonly Random _random = new Random();
        private double _hitFlashCountDown;
        private double _shootCountDown;

        #region Missile Properties

        private MissileManager _missileManager;
        private Texture _missileTexture;
        public double FireRecovery { get; set; }

        #endregion Missile Properties

        /// <summary>
        ///     Constructs an enemy given the texture and AI (IEnemyBrain)
        /// </summary>
        /// <param name="texture">The texture for the enemy</param>
        /// <param name="enemyBrain">The AI for the enemy</param>
        /// <param name="position">The spawn point for this enemy</param>
        public Enemy(Texture texture, IEnemyBrain enemyBrain, Vector position)
        {
            _enemyBrain = enemyBrain;
            Sprite.Texture = texture;
            Sprite.SetPosition(position);
            RestartShootCountDown();
        }

        public int Health { get; set; }
        public int Damage { get; set; }
        private double Scale { get; set; }
        private double HitFlashTime { get; set; }
        public double Speed { get; set; }
        public Path Path { get; set; }
        public double MaxTimeToShoot { get; set; }
        public double MinTimeToShoot { get; set; }

        public bool IsDead
        {
            get { return Health <= 0; }
        }

        public void RestartShootCountDown()
        {
            _shootCountDown = MinTimeToShoot + (_random.NextDouble() * MaxTimeToShoot);
        }

        public override void OnCollision(IEntity entity, Vector amount)
        {
            if (entity.GetType() == typeof(Tile))
            {
                Sprite.SetPosition(Sprite.GetPosition() + amount);
            }

            else if (entity.GetType() == typeof(Enemy))
            {
                Sprite.SetPosition(Sprite.GetPosition() + amount);
            }


            else if (entity.GetType() == typeof(Eyeball))
            {
                Sprite.SetPosition(Sprite.GetPosition() + amount);
            }

            else if (entity.GetType() == typeof(Missile))
            {
                var missile = entity as Missile;
                if (Health == 0)
                {
                    OnDestroyed();
                    return;
                }

                Health = Math.Max(0, Health - 25);
                _hitFlashCountDown = HitFlashTime;
                Sprite.SetColor(new Color(1, 1, 0, 1));
                Health -= missile.Damage;
            }
        }

        public void ReverseCourse()
        {
            throw new NotImplementedException();
        }

        public bool IsPathDone()
        {
            return Sprite.GetPosition() == Sprite.VertexPositions[Sprite.VertexPositions.Length - 1];
        }

        private void OnDestroyed()
        {
            //_effectsManager.AddExplosion(Sprite.GetPosition());
        }

        internal void SetPosition(Vector position)
        {
            Sprite.SetPosition(position);
        }

        public void Move(Vector amount)
        {
            amount *= Speed;
            Sprite.SetPosition(Sprite.GetPosition() + amount);
        }

        public virtual void Attack(double elapsedTime)
        {

        }

        public void Update(double elapsedTime)
        {
            _shootCountDown = _shootCountDown - elapsedTime;

            if (_enemyBrain != null)
            {
                Move(_enemyBrain.NextMove(Sprite.GetPosition(), elapsedTime) * elapsedTime);
                Attack(elapsedTime);
            }

            if (_hitFlashCountDown == 0) return;
            _hitFlashCountDown = Math.Max(0, _hitFlashCountDown - elapsedTime);
            double scaledTime = 1 - (_hitFlashCountDown / HitFlashTime);
            Sprite.SetColor(new Color(1, 1, (float)scaledTime, 1));
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
            //Render_Debug();
        }
    }
}