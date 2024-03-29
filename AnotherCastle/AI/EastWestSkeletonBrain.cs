﻿using System;
using Engine;

namespace AnotherCastle.AI
{
    public class EastWestSkeletonBrain : IEnemyBrain
    {
        private double _elapsedTime;
        private Vector _lastMove;

        public EastWestSkeletonBrain()
        {
            _lastMove = new Vector(0, -1, 0);
        }

        public Path Path { get; set; }

        public Vector NextMove(Vector currentPosition, double elapsedTime)
        {
            _elapsedTime += elapsedTime;
            var prng = new Random();
            double ranTime = (double) prng.Next(5, 25)/10;

            if (currentPosition.X > 600) _lastMove = new Vector(-1, 0, 0);
            else if (currentPosition.X < -600) _lastMove = new Vector(1, 0, 0);

            else if (_elapsedTime > ranTime)
            {
                _elapsedTime = 0;
                int ranNum = prng.Next(0, 3);

                switch (ranNum)
                {
                    case 1:
                        _lastMove = new Vector(1, 0, 0);
                        break;
                    case 2:
                        _lastMove = new Vector(-1, 0, 0);
                        break;
                }
            }
            return _lastMove;
        }
    }
}