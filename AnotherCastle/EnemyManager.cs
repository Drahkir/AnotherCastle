using System;
using System.Collections.Generic;
using Engine;

namespace AnotherCastle
{
    public class ClientBounds
    {
        public int NorthBound;
        public int SouthBound;
        public int WestBound;
        public int EastBound;
    }

    public class EnemyManager
    {
        readonly List<Enemy> _enemies = new List<Enemy>();
        readonly List<EnemyDef> _upComingEnemies = new List<EnemyDef>();
        readonly TextureManager _textureManager;
        readonly EffectsManager _effectsManager;
        readonly MissileManager _missileManager;

        public List<Enemy> EnemyList
        {
            get
            {
                return _enemies;
            }
        }

        public EnemyManager(TextureManager textureManager, EffectsManager effectsManager, MissileManager missileManager)
        {
            _textureManager = textureManager;
            _effectsManager = effectsManager;
            _missileManager = missileManager;
            //_leftBound = leftBound;

            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 30));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 29.5));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 29));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 28.5));

            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 30));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 29.5));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 29));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 28.5));

            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 25));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 24.5));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 24));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder", 23.5));

            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 20));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 19.5));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 19));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_low", 18.5));

            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_straight", 16));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_straight", 15.8));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_straight", 15.6));
            //_upComingEnemies.Add(new EnemyDef("cannon_fodder_straight", 15.4));


            //_upComingEnemies.Add(new EnemyDef("up_l", 10));
            //_upComingEnemies.Add(new EnemyDef("down_l", 9));
            //_upComingEnemies.Add(new EnemyDef("up_l", 8));
            //_upComingEnemies.Add(new EnemyDef("down_l", 7));
            //_upComingEnemies.Add(new EnemyDef("up_l", 6));



            // Sort enemies so the greater launch time appears first.
            _upComingEnemies.Sort(delegate(EnemyDef firstEnemy, EnemyDef secondEnemy)
            {
                return firstEnemy.LaunchTime.CompareTo(secondEnemy.LaunchTime);
            });
        }

        private void QueueUpcomingEnemies(double gameTime)
        {
            var prng = new Random();
            var randomCount = prng.Next(1, 1);
            var launchTime = gameTime + 2;

            for (var i = 0; i < randomCount; i++)
            {
                _upComingEnemies.Add(new EnemyDef(EnemyType.Skeleton.ToString(), launchTime));
                launchTime += 2;
            }
        }

        private void UpdateEnemySpawns(double gameTime)
        {
            // If no upcoming enemies then there's nothing to spawn
            if (_upComingEnemies.Count == 0)
            {
                QueueUpcomingEnemies(gameTime);
            }

            var lastElement = _upComingEnemies[_upComingEnemies.Count - 1];
            if (!(gameTime > lastElement.LaunchTime)) return;
            _upComingEnemies.RemoveAt(_upComingEnemies.Count - 1);
            _enemies.Add(CreateEnemyFromDef(lastElement));
        }

        //private void UpdateEnemySpawns(double gameTime)
        //{
        //    // If no upcoming enemies then there's nothing to spawn
        //    if (_upComingEnemies.Count == 0)
        //    {
        //        return;
        //    }

        //    EnemyDef lastElement = _upComingEnemies[_upComingEnemies.Count - 1];
        //    if (gameTime < lastElement.LaunchTime)
        //    {
        //        _upComingEnemies.RemoveAt(_upComingEnemies.Count - 1);
        //        _enemies.Add(CreateEnemyFromDef(lastElement));
        //    }
        //}

        private Enemy CreateEnemyFromDef(EnemyDef definition)
        {
            var enemy = new Enemy(_textureManager, _effectsManager, _missileManager);
            
            if (definition.EnemyType == "skeleton")
            {
                var pathPoints = new List<Vector>();
                var prng = new Random();
                var ranX = prng.Next(-580, 580);
                var ranY = prng.Next(-350, 350);

                enemy.SetPosition(new Vector(ranX, ranY, 0));
            }

            if (definition.EnemyType == "cannon_fodder")
            {
                var pathPoints = new List<Vector>
                {
                    new Vector(1400, 0, 0),
                    new Vector(0, 0, 0),
                    new Vector(-1400, 0, 0)
                };

                enemy.Path = new Path(pathPoints, 15);
            }

            if (definition.EnemyType == "cannon_fodder_high")
            {
                var pathPoints = new List<Vector>
                {
                    new Vector(1400, 0, 0),
                    new Vector(0, 250, 0),
                    new Vector(-1400, 0, 0)
                };

                enemy.Path = new Path(pathPoints, 15);
            }

            else if (definition.EnemyType == "cannon_fodder_low")
            {
                var pathPoints = new List<Vector>
                {
                    new Vector(1400, 0, 0),
                    new Vector(0, -250, 0),
                    new Vector(-1400, 0, 0)
                };

                enemy.Path = new Path(pathPoints, 15);
            }

            else if (definition.EnemyType == "cannon_fodder_straight")
            {
                var pathPoints = new List<Vector> {new Vector(1400, 0, 0), new Vector(-1400, 0, 0)};

                enemy.Path = new Path(pathPoints, 21);
            }

            else if (definition.EnemyType == "up_l")
            {
                var pathPoints = new List<Vector>
                {
                    new Vector(500, -375, 0),
                    new Vector(500, 0, 0),
                    new Vector(500, 0, 0),
                    new Vector(-1400, 200, 0)
                };

                enemy.Path = new Path(pathPoints, 15);
            }

            else if (definition.EnemyType == "down_l")
            {
                var pathPoints = new List<Vector>
                {
                    new Vector(500, 375, 0),
                    new Vector(500, 0, 0),
                    new Vector(500, 0, 0),
                    new Vector(-1400, -200, 0)
                };

                enemy.Path = new Path(pathPoints, 15);
            }

            return enemy;
        }

        public void Update(double elapsedTime, double gameTime)
        {
            UpdateEnemySpawns(gameTime);
            _enemies.ForEach(x => x.Update(elapsedTime));
            CheckForOutOfBounds();
            RemoveDeadEnemies();
        }

        private void CheckForOutOfBounds()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.IsPathDone())
                    //|| enemy.GetBoundingBox().Left > _clientBounds.EastBound
                    //|| enemy.GetBoundingBox().Top < _clientBounds.NorthBound
                    //|| enemy.GetBoundingBox().Bottom > _clientBounds.SouthBound)
                {
                    enemy.Health = 0;
                }
             }
        }

        public void Render(Renderer renderer)
        {
            _enemies.ForEach(x => x.Render(renderer));
        }

        private void RemoveDeadEnemies()
        {
            for (var i = _enemies.Count - 1; i >= 0; i--)
            {
                if (_enemies[i].IsDead)
                {
                    _enemies.RemoveAt(i);
                }
            }
        }
    }
}