using System;
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
        public bool WaitingAtExit;
        private float _blinkAlpha = (float)0.0;
        private const float BlinkAlphaMax = (float) 0.9;
        private const float BlinkAlphaMin = (float) 0.1;
        private float _blinkAlphaAdjustor = (float)0.01;

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
            if (_isInvulnerable)
            {
                // Make pixela blink while she is invulnerable
                if (_blinkAlpha >= BlinkAlphaMax)
                {
                    _blinkAlphaAdjustor *= -1;
                    _blinkAlpha = BlinkAlphaMax;
                }

                if (_blinkAlpha <= BlinkAlphaMin)
                {
                    _blinkAlphaAdjustor *= -1;
                    _blinkAlpha = BlinkAlphaMin;
                }
                _blinkAlpha += _blinkAlphaAdjustor;
                var color = new Color(1, 1, 1, _blinkAlpha);
                Sprite.SetColor(color);
            }

            else
            {
                Sprite.SetColor(new Color(1, 1, 1, 1));
            }
            renderer.DrawSprite(Sprite);
            //Render_Debug();
        }

        public override void OnCollision(IEntity entity, Vector amount)
        {
            var type = entity.GetType();
            var enemy = entity as Enemy;

            if (type == typeof(Tile))
            {
                var tile = entity as Tile;
                if (tile != null && tile.TileName == "exit")
                {
                    WaitingAtExit = true;
                }
                Sprite.SetPosition(Sprite.GetPosition() + amount);
            }

            else if (enemy != null) 
            {
                if (_isInvulnerable) return;
                Health -= enemy.Damage;
                _isInvulnerable = true;
                var sprPos = Sprite.GetPosition();
                Sprite.SetPosition(sprPos + new Vector(0, 20, 0));
            }

            else if (type == typeof(EnemyMissile))
            {
                if (_isInvulnerable) return;
                var missile = entity as EnemyMissile;
                Health -= missile.Damage;
                _isInvulnerable = true;
                Sprite.SetPosition(Sprite.GetPosition() + amount);
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

        //public void HandleCollision(Vector amount)
        //{
        //    Sprite.SetPosition(Sprite.GetPosition() + amount);
        //}

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