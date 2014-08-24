using Engine;

namespace AnotherCastle
{
    public class EyeballBrain : IEnemyBrain
    {
        private double _elapsedTime;
        private Vector _lastMove;

        public EyeballBrain()
        {
            _lastMove = new Vector(0, -1, 0);
        }

        public Path Path { get; set; }

        public Vector NextMove(Vector currentPosition, double elapsedTime)
        {
            return new Vector(1, 0, 0);
        }
    }
}