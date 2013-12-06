﻿using System;
using Engine;

namespace AnotherCastle
{
    public class PlayerCharacter : Entity
    {
        private const double Scale = 1;
        private const double Speed = 256;
        private readonly Texture _downTexture;
        private readonly Texture _leftTexture;
        private readonly Texture _rightTexture;
        private readonly Texture _upTexture;
        private double _invulnerabilityTimer;
        private bool _isInvulnerable;

        #region Missile Properties

        private const double FireRecovery = 0.25;
        private readonly MissileManager _missileManager;
        private readonly Texture _missileTexture;
        private double _fireRecoveryTime = FireRecovery;

        #endregion Missile Properties

        public PlayerCharacter(TextureManager textureManager, MissileManager missileManager)
        {
            _missileManager = missileManager;
            _missileTexture = textureManager.Get("heart_missile");
            Sprite.Texture = textureManager.Get("pixela_down");
            _upTexture = textureManager.Get("pixela_up");
            _downTexture = textureManager.Get("pixela_down");
            _leftTexture = textureManager.Get("pixela_left");
            _rightTexture = textureManager.Get("pixela_right");
            Sprite.SetScale(Scale, Scale);
            Health = 20;
        }

        /// <summary>
        ///     Returns true if the PlayerCharacter is dead
        /// </summary>
        /// <returns>true if dead; else false</returns>
        public bool IsDead { get; private set; }

        public int Health { get; set; }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
            //Render_Debug();
        }

        internal void OnCollision(Enemy enemy)
        {
            if (_isInvulnerable) return;
            Health -= enemy.Damage;
            _isInvulnerable = true;
        }

        internal void OnCollision(Missile missile)
        {
            if (!_isInvulnerable)
            {
                //Health -= missile.damage;
                _isInvulnerable = true;
            }
        }

        public void Update(double elapsedTime)
        {
            _fireRecoveryTime = Math.Max(0, (_fireRecoveryTime - elapsedTime));
            if (Health <= 0) IsDead = true;
            if (!_isInvulnerable) return;
            if (_invulnerabilityTimer == -1)
            {
                _invulnerabilityTimer = 0;
            }

            else if (_invulnerabilityTimer > 2)
            {
                _invulnerabilityTimer = -1;
                _isInvulnerable = false;
            }

            else
            {
                _invulnerabilityTimer += elapsedTime;
            }
        }

        public void Attack(Vector direction)
        {
            if (_fireRecoveryTime > 0)
            {
                return;
            }

            _fireRecoveryTime = FireRecovery;
            var missile = new Missile(_missileTexture, direction);
            //missile.SetColor(new Color(0, 1, 0, 1));
            missile.SetPosition(Sprite.GetPosition() + direction);
            _missileManager.Shoot(missile);
        }

        public void Move(Vector amount)
        {
            amount *= Speed;

            if (Math.Abs(amount.X) > Math.Abs(amount.Y))
            {
                if (amount.X > 0)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
            }

            else
            {
                if (amount.Y > 0)
                {
                    MoveUp();
                }
                else
                {
                    MoveDown();
                }
            }

            Sprite.SetPosition(Sprite.GetPosition() + amount);
        }

        public void SetPosition(Vector position)
        {
            Sprite.SetPosition(position);
        }

        public void HandleCollision(Vector amount)
        {
            Sprite.SetPosition(Sprite.GetPosition() + amount);
        }

        public void MoveUp()
        {
            Sprite.Texture = _upTexture;
        }

        public void MoveDown()
        {
            Sprite.Texture = _downTexture;
        }

        public void MoveLeft()
        {
            Sprite.Texture = _leftTexture;
        }

        public void MoveRight()
        {
            Sprite.Texture = _rightTexture;
        }
    }
}