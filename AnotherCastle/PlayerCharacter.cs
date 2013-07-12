using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Tao.OpenGl;

namespace AnotherCastle
{
    public class PlayerCharacter : Entity
    {
        int _health;
        bool _isDead;
        bool _isInvulnerable;
        double _invulnerabilityTimer;
        double _scale = 1;
        double _speed = 256;
        MissileManager _missileManager;
        Texture _missileTexture;
        Texture _upTexture;
        Texture _downTexture;
        Texture _leftTexture;
        Texture _rightTexture;

        public PlayerCharacter(TextureManager textureManager, MissileManager missileManager)
        {
            _missileManager = missileManager;
            //_missileTexture = textureManager.Get("arrow");
            Sprite.Texture = textureManager.Get("pixela_down");
            _upTexture = textureManager.Get("pixela_up");
            _downTexture = textureManager.Get("pixela_down");
            _leftTexture = textureManager.Get("pixela_left");
            _rightTexture = textureManager.Get("pixela_right");
            Sprite.SetScale(_scale, _scale);
            //_sprite.SetRotation(Math.PI / 2);
            Health = 100;
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
            Render_Debug();
        }

        public bool IsDead
        {
            get { return _isDead; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        internal void OnCollision(Enemy enemy)
        {
            if (!_isInvulnerable)
            {
                Health -= enemy.Damage;
                _isInvulnerable = true;
            }
        }

        internal void OnCollision(Missile missile)
        {
            if (!_isInvulnerable)
            {
                //Health -= missile.damage;
                _isInvulnerable = true;
            }
        }

        internal void OnCollision(MapObject mapObject)
        {
            //Impede Movement

        }

        public void Update(double elapsedTime)
        {
            // Normally some fire recovery code would go here
            if (Health <= 0) _isDead = true;
            if (_isInvulnerable)
            {
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
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Move(Vector amount)
        {
            amount *= _speed;
            Sprite.SetPosition(Sprite.GetPosition() + amount);
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
