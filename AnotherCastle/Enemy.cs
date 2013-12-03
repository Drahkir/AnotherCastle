﻿using System;
using Engine;

namespace AnotherCastle
{
    public class Enemy : Entity
    {
        public int Health { get; set; }
        public int Damage = 20;
        private const double Scale = 1.8;
        private const double HitFlashTime = 0.25;
        double _hitFlashCountDown;
        readonly EffectsManager _effectsManager;
        public Path Path { get; set; }
        public double MaxTimeToShoot { get; set; }
        public double MinTimeToShoot { get; set; }
        readonly Random _random = new Random();
        double _shootCountDown;
        private const double Speed = 256;
        readonly IEnemyBrain _enemyBrain;

        public void RestartShootCountDown()
        {
            _shootCountDown = MinTimeToShoot + (_random.NextDouble() * MaxTimeToShoot);
        }

        /// <summary>
        /// Constructs a default skeleton if the constructor is only given managers as arguments
        /// </summary>
        /// <param name="textureManager">The texture manager</param>
        /// <param name="effectsManager">The effects manager</param>
        public Enemy(TextureManager textureManager, EffectsManager effectsManager)
        {
            _enemyBrain = new SkeletonBrain();
            Health = 100;
            Sprite.Texture = textureManager.Get("skeleton");
            Sprite.SetScale(Scale, Scale);
            Sprite.SetPosition(200, 0);
            Sprite.Speed = 1;
            _effectsManager = effectsManager;
            MaxTimeToShoot = 12;
            MinTimeToShoot = 1;
            RestartShootCountDown();
        }

        /// <summary>
        /// Constructs an enemy given the texture and AI (IEnemyBrain)
        /// </summary>
        /// <param name="texture">The texture for the enemy</param>
        /// <param name="enemyBrain">The AI for the enemy</param>
        /// <param name="position">The spawn point for this enemy</param>
        public Enemy(Texture texture, IEnemyBrain enemyBrain, Vector position)
        {
            _enemyBrain = enemyBrain;
            Health = 15;
            Sprite.Texture = texture;
            Sprite.SetPosition(position);
            Sprite.SetScale(Scale, Scale);
            MaxTimeToShoot = 12;
            MinTimeToShoot = 1;
            RestartShootCountDown();
        }
        /// <summary>
        /// Handles the on collision event
        /// </summary>
        /// <param name="playerCharacter">The player</param>
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

        public void HandleCollision(Vector amount)
        {
            Sprite.SetPosition(Sprite.GetPosition() + amount);
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

            if (_enemyBrain != null)
            {
                Move(_enemyBrain.NextMove(Sprite.GetPosition(), elapsedTime) * elapsedTime);
            }

            if (_hitFlashCountDown == 0) return;
            _hitFlashCountDown = Math.Max(0, _hitFlashCountDown - elapsedTime);
            var scaledTime = 1 - (_hitFlashCountDown / HitFlashTime);
            Sprite.SetColor(new Color(1, 1, (float)scaledTime, 1));
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
            //Render_Debug();
        }
    }
}
