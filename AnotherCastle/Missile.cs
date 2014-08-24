using Engine;

namespace AnotherCastle
{
    public class Missile : Entity
    {
        public Missile()
        {
        }

        public Missile(Texture missileTexture, Vector direction)
        {
            Sprite.Texture = missileTexture;

            // Some default values
            Dead = false;
            Direction = direction;
            Speed = 512; // pixels per second
            Damage = 10;
        }

        public bool Dead { get; set; }
        public Vector Direction { get; set; }
        public double Speed { get; set; }
        public int Damage { get; set; }

        public double X
        {
            get { return Sprite.GetPosition().X; }
        }

        public double Y
        {
            get { return Sprite.GetPosition().Y; }
        }

        public void SetPosition(Vector position)
        {
            Sprite.SetPosition(position);
        }

        public void SetColor(Color color)
        {
            Sprite.SetColor(color);
        }

        public void Render(Renderer renderer)
        {
            if (Dead)
            {
                return;
            }
            renderer.DrawSprite(Sprite);
        }

        public override void OnCollision(IEntity entity, Vector amount)
        {
            if (entity.GetType() == typeof (PlayerCharacter))
            {
                return;
            }

            Dead = true;
            //Sprite.SetColor(new Color(0, 0, 1, 1));
        }

        public void Update(double elapsedTime)
        {
            if (Dead)
            {
                return;
            }

            Vector position = Sprite.GetPosition();
            position += Direction*Speed*elapsedTime;
            Sprite.SetPosition(position);
        }
    }
}