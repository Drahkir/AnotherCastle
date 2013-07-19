using System;
using Engine;

namespace AnotherCastle
{
    public class PlayerCharacter : Entity
    {
        bool _isInvulnerable;
        double _invulnerabilityTimer;
        private const double Scale = 1;
        private const double Speed = 256;
        readonly Texture _upTexture;
        readonly Texture _downTexture;
        readonly Texture _leftTexture;
        readonly Texture _rightTexture;

        public PlayerCharacter(TextureManager textureManager)
        {
            Sprite.Texture = textureManager.Get("pixela_down");
            _upTexture = textureManager.Get("pixela_up");
            _downTexture = textureManager.Get("pixela_down");
            _leftTexture = textureManager.Get("pixela_left");
            _rightTexture = textureManager.Get("pixela_right");
            Sprite.SetScale(Scale, Scale);
            Health = 100;
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
            Render_Debug();
        }

        /// <summary>
        /// Returns true if the PlayerCharacter is dead
        /// </summary>
        /// <returns>true if dead; else false</returns>
        public bool IsDead { get; private set; }

        public int Health { get; set; }

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

        //internal void OnCollision(MapObject mapObject)
        //{
        //    //Impede Movement

        //}

        public void Update(double elapsedTime)
        {
            // Normally some fire recovery code would go here
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

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Move(Vector amount)
        {
            amount *= Speed;
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
