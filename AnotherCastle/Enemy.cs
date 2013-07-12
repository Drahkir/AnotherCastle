using System;
using Engine;

namespace AnotherCastle
{
    public class Enemy : Entity
    {
        public int Health { get; set; }
        public int Damage = 20;
        private const double Scale = .2;
        private const double HitFlashTime = 0.25;
        double _hitFlashCountDown;
        readonly EffectsManager _effectsManager;
        public Path Path { get; set; }
        public double MaxTimeToShoot { get; set; }
        public double MinTimeToShoot { get; set; }
        readonly Random _random = new Random();
        double _shootCountDown;
        private const double Speed = 256;
        MissileManager _missileManager;
        readonly IEnemyBrain _enemyBrain;

        public void RestartShootCountDown()
        {
            _shootCountDown = MinTimeToShoot + (_random.NextDouble() * MaxTimeToShoot);
        }

        public Enemy(TextureManager textureManager, EffectsManager effectsManager, MissileManager missileManager)
        {
            _enemyBrain = new SkeletonBrain();
            Health = 50;
            Sprite.Texture = textureManager.Get("villager");
            //_sprite.TextureList.Add(textureManager.Get("skeleton"));
            //_sprite.TextureList.Add(textureManager.Get("skeleton_b"));
            Sprite.SetScale(Scale, Scale);
            //_sprite.SetRotation(Math.PI);
            Sprite.SetPosition(200, 0);
            Sprite.Speed = 1;
            _effectsManager = effectsManager;
            _missileManager = missileManager;
            //_missileTexture = textureManager.Get("missile");
            MaxTimeToShoot = 12;
            MinTimeToShoot = 1;
            RestartShootCountDown();
        }

        internal void OnCollision(PlayerCharacter playerCharacter)
        {
            playerCharacter.OnCollision(this);
        }

        internal void OnCollision(Missile missile)
        {
            if (Health == 0)
            {
                return;
            }

            Health = Math.Max(0, Health - 25);
            _hitFlashCountDown = HitFlashTime;
            Sprite.SetColor(new Color(1, 1, 0, 1));

            if (Health == 0)
            {
                OnDestroyed();
            }
        }

        internal void OnCollision(Enemy enemy)
        {
            ReverseCourse();
        }

        public void ReverseCourse()
        {
            //comment
        }

        public bool IsPathDone()
        {
            return Sprite.GetPosition() == Sprite.VertexPositions[Sprite.VertexPositions.Length - 1];
        }

        private void OnDestroyed()
        {
            _effectsManager.AddExplosion(Sprite.GetPosition());
        }

        public bool IsDead
        {
            get { return Health == 0; }
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

        public void Update(double elapsedTime)
        {
            _shootCountDown = _shootCountDown - elapsedTime;

            //if (_shootCountDown <= 0)
            //{
            //    Missile missile = new Missile(_missileTexture);
            //    missile.Speed = 350;
            //    missile.Direction = new Vector(-1, 0, 0);
            //    missile.SetPosition(_sprite.GetPosition());
            //    missile.SetColor(new Engine.Color(1, 0, 0, 1));
            //    _missileManager.EnemyShoot(missile);
            //    RestartShootCountDown();
            //}

            //if (Path != null)
            //{
            //    Path.UpdatePosition(elapsedTime, this);
            //}
            if (_enemyBrain != null)
            {
                Move(_enemyBrain.NextMove(Sprite.GetPosition(), elapsedTime) * elapsedTime);
            }

            if (_hitFlashCountDown != 0)
            {
                _hitFlashCountDown = Math.Max(0, _hitFlashCountDown - elapsedTime);
                double scaledTime = 1 - (_hitFlashCountDown / HitFlashTime);
                Sprite.SetColor(new Color(1, 1, (float)scaledTime, 1));
            }
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
            //Render_Debug();
        }
    }
}
