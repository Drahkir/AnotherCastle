using Engine;

namespace AnotherCastle
{
    public class ScrollingBackground
    {
        private readonly Sprite _background = new Sprite();

        public ScrollingBackground(Texture background)
        {
            _background.Texture = background;
            Speed = 0.05f;
            Direction = new Vector(1, 0, 0);
        }

        public float Speed { get; set; }
        public Vector Direction { get; set; }

        public void SetScale(double x, double y)
        {
            _background.SetScale(x, y);
        }

        public void Update(float elapsedTime)
        {
            //_background.SetUVs(_topLeft, _bottomRight);
            //_topLeft.X += (float)(0.15f * Direction.X * elapsedTime);
            //_bottomRight.X += (float)(0.15f * Direction.X * elapsedTime);
            //_topLeft.Y += (float)(0.15f * Direction.Y * elapsedTime);
            //_bottomRight.Y += (float)(0.15f * Direction.Y * elapsedTime);
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(_background);
        }
    }
}