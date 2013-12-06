using System.Collections.Generic;
using Engine;

namespace AnotherCastle
{
    public class EffectsManager
    {
        private readonly List<AnimatedSprite> _effects = new List<AnimatedSprite>();
        private readonly TextureManager _textureManager;

        public EffectsManager(TextureManager textureManager)
        {
            _textureManager = textureManager;
        }

        public void AddExplosion(Vector position)
        {
            var explosion = new AnimatedSprite {Texture = _textureManager.Get("explosion")};
            explosion.SetAnimation(4, 4);
            explosion.SetPosition(position);
            _effects.Add(explosion);
        }

        public void Update(double elapsedTime)
        {
            _effects.ForEach(x => x.Update(elapsedTime));
            RemoveDeadExplosions();
        }

        public void Render(Renderer renderer)
        {
            _effects.ForEach(renderer.DrawSprite);
        }

        private void RemoveDeadExplosions()
        {
            for (var i = _effects.Count - 1; i >= 0; i--)
            {
                if (_effects[i].Finished)
                {
                    _effects.RemoveAt(i);
                }
            }
        }
    }
}