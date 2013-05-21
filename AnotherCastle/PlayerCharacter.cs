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
        double _speed = 512; // pixels per second
        double _scale = 1;
        MissileManager _missileManager;
        Texture _missileTexture;

        public PlayerCharacter(TextureManager textureManager, MissileManager missileManager)
        {
            _missileManager = missileManager;
            _missileTexture = textureManager.Get("arrow");
            _sprite.Texture = textureManager.Get("player_character");
            _sprite.SetScale(_scale, _scale);
            //_sprite.SetRotation(Math.PI / 2);
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(_sprite);
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
            // Probably do something like this
            // Health -= enemy.Damage;
            throw new NotImplementedException();
        }

        internal void OnCollision(Missile missile)
        {
            // Probably do something like this
            // Health -= bullet.Damage;
            throw new NotImplementedException();
        }

        public void Move(Vector amount)
        {
            amount *= _speed;
            _sprite.SetPosition(_sprite.GetPosition() + amount);
        }

        public void Update(double elapsedTime)
        {
            // Normally some fire recovery code would go here
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }
    }
}
