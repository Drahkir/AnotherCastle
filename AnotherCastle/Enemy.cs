using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Tao.OpenGl;
using System.Drawing;

namespace AnotherCastle
{
    public class Enemy : Entity
    {
        public int Health { get; set; }
        public int damage = 20;
        double _scale = .2;
        static readonly double HitFlashTime = 0.25;
        double _hitFlashCountDown = 0;
        EffectsManager _effectsManager;
        public Path Path { get; set; }
        public double MaxTimeToShoot { get; set; }
        public double MinTimeToShoot { get; set; }
        Random _random = new Random();
        double _shootCountDown;
        double _speed = 256;
        MissileManager _missileManager;
        Texture _missileTexture;
        IEnemyBrain _enemyBrain;

        public void RestartShootCountDown()
        {
            _shootCountDown = MinTimeToShoot + (_random.NextDouble() * MaxTimeToShoot);
        }

        public Enemy(TextureManager textureManager, EffectsManager effectsManager, MissileManager missileManager)
        {
            _enemyBrain = new SkeletonBrain();
            Health = 50;
            _sprite.Texture = textureManager.Get("villager");
            //_sprite.TextureList.Add(textureManager.Get("skeleton"));
            //_sprite.TextureList.Add(textureManager.Get("skeleton_b"));
            _sprite.SetScale(_scale, _scale);
            //_sprite.SetRotation(Math.PI);
            _sprite.SetPosition(200, 0);
            _sprite.Speed = 1;
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
            _sprite.SetColor(new Engine.Color(1, 1, 0, 1));

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
            return this._sprite.GetPosition() == this._sprite.VertexPositions[this._sprite.VertexPositions.Length - 1];
        }

        private void OnDestroyed()
        {
            _effectsManager.AddExplosion(_sprite.GetPosition());
        }

        public bool IsDead
        {
            get { return Health == 0; }
        }

        internal void SetPosition(Vector position)
        {
            _sprite.SetPosition(position);
        }

        public void Move(Vector amount)
        {
            amount *= _speed;
            _sprite.SetPosition(_sprite.GetPosition() + amount);
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
                Move(_enemyBrain.NextMove(_sprite.GetPosition(), elapsedTime) * elapsedTime);
            }

            if (_hitFlashCountDown != 0)
            {
                _hitFlashCountDown = Math.Max(0, _hitFlashCountDown - elapsedTime);
                double scaledTime = 1 - (_hitFlashCountDown / HitFlashTime);
                _sprite.SetColor(new Engine.Color(1, 1, (float)scaledTime, 1));
            }
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(_sprite);
            //Render_Debug();
        }
    }
}
