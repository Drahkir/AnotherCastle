using System;
using Engine;

namespace AnotherCastle.AI
{
    public class NorthSouthSkeletonBrain : IEnemyBrain
    {
        private double _elapsedTime;
        private Vector _lastMove;

        public NorthSouthSkeletonBrain()
        {
            _lastMove = new Vector(0, -1, 0);
        }

        public Path Path { get; set; }

        public Vector NextMove(Vector currentPosition, double elapsedTime)
        {
            _elapsedTime += elapsedTime;
            var prng = new Random();
            double ranTime = (double) prng.Next(5, 25)/10;

            if (currentPosition.Y > 370) _lastMove = new Vector(0, -1, 0);
            else if (currentPosition.Y < -370) _lastMove = new Vector(0, 1, 0);

            else if (_elapsedTime > ranTime)
            {
                _elapsedTime = 0;
                int ranNum = prng.Next(0, 3);

                switch (ranNum)
                {
                    case 0:
                        _lastMove = new Vector(0, 1, 0);
                        break;
                    case 1:
                        _lastMove = new Vector(0, -1, 0);
                        break;
                }
            }
            return _lastMove;
        }
    }
}