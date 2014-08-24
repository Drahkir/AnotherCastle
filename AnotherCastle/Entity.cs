using System.Drawing;
using Engine;
using Tao.OpenGl;

namespace AnotherCastle
{
    public interface IEntity
    {
        RectangleF GetBoundingBox();
        void OnCollision(IEntity collidingEntity, Vector amount);
    }

    public class Entity : IEntity
    {
        protected Sprite Sprite = new Sprite();

        public RectangleF GetBoundingBox()
        {
            var width = (float) (Sprite.Texture.Width*Sprite.ScaleX);
            var height = (float) (Sprite.Texture.Height*Sprite.ScaleY);
            return new RectangleF((float) Sprite.GetPosition().X - width/2, (float) Sprite.GetPosition().Y - height/2,
                width, height);
        }

        public virtual void OnCollision(IEntity collidingEntity, Vector amount)
        {
            // If OnCollision is not overridden it will simply return
        }

        // Render a bounding box
        public void Render_Debug()
        {
            Gl.glDisable(Gl.GL_TEXTURE_2D);

            RectangleF bounds = GetBoundingBox();

            Gl.glBegin(Gl.GL_LINE_LOOP);
            {
                Gl.glColor3f(1, 0, 0);
                Gl.glVertex2f(bounds.Left, bounds.Top);
                Gl.glVertex2f(bounds.Right, bounds.Top);
                Gl.glVertex2f(bounds.Left, bounds.Bottom);
                Gl.glVertex2f(bounds.Right, bounds.Bottom);
            }

            Gl.glEnd();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }
    }
}