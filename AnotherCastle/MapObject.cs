using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;

namespace AnotherCastle
{
    public class MapObject : Entity
    {

        public MapObject()
        {

        }

        public MapObject(TextureManager textureManager, EffectsManager effectsManager)
        {
            Sprite.Texture = textureManager.Get("rock_wall");
        }

        public void Update(double elapsedTime)
        {

        }

        public void OnCollision(PlayerCharacter _playerCharacter)
        {
            
        }

        internal void SetPosition(Vector position)
        {
            Sprite.SetPosition(position);
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
            Render_Debug();
        }
    }
}
