using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AnotherCastle.AI;
using Engine;
using Engine.Input;
using Platformer;

namespace AnotherCastle
{
    internal class Level
    {
        //MissileManager _missileManager = new MissileManager(new RectangleF(-1300 / 2, -750 / 2, 1300, 750));
        //private const int xOffset = -650;
        //private const int yOffset = -410;
        private const int xOffset = -722;
        private const int yOffset = -510;
        private readonly Room _currentRoom;
        private readonly EffectsManager _effectsManager;
        private readonly EnemyManager _enemyManager;
        private readonly Input _input;
        private readonly MissileManager _missileManager = MissileManager.Instance;
        private readonly PlayerCharacter _playerCharacter;
        private Tile[,] _tiles;

        public Level(Input input, TextureManager textureManager, Stream mapFile)
        {
            _input = input;
            _effectsManager = new EffectsManager(textureManager);
            _playerCharacter = new PlayerCharacter(textureManager, _missileManager);
            _enemyManager = new EnemyManager();
            LoadTiles(mapFile, textureManager);
            _currentRoom = new Room(_tiles);

            //_background = new ScrollingBackground(textureManager.Get("background"));
            //_background.SetScale(5, 5);
            //_background.Speed = 0.05f;

            //_backgroundLayer = new ScrollingBackground(textureManager.Get("background_layer_1"));
            //_backgroundLayer.Speed = 0.1f;
            //_backgroundLayer.SetScale(2.0, 2.0);
        }

        public bool IsLevelComplete { get; set; }
        public bool AreAllEnemiesDead { get; set; }
        public bool IsGamePaused { get; set; }

        /// <summary>
        ///     Width of level measured in tiles.
        /// </summary>
        public int Width
        {
            get { return _tiles.GetLength(0); }
        }

        /// <summary>
        ///     Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return _tiles.GetLength(1); }
        }

        private void LoadTiles(Stream mapFile, TextureManager textureManager)
        {
            // Load the level and ensure all of the lines are the same length.
            int width = 0;
            var lines = new List<string>();
            using (var reader = new StreamReader(mapFile))
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    width = line.Length;
                    while (line != null)
                    {
                        lines.Add(line);
                        //if (line.Length != width)
                        //    throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                        line = reader.ReadLine();
                    }
                }
            }

            // Allocate the tile grid.
            _tiles = new Tile[width, lines.Count];

            // Loop over every tile position,
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    _tiles[x, y] = LoadTile(tileType, textureManager, x, y);
                }
            }

            // Verify that the level has a beginning and an end.
            //if (Player == null)
            //    throw new NotSupportedException("A level must have a starting point.");
            //if (exit == InvalidPosition)
            //    throw new NotSupportedException("A level must have an exit.");
        }

        /// <summary>
        ///     Loads an individual tile's appearance and behavior.
        /// </summary>
        /// <param name="tileChar">
        ///     The character loaded from the structure file which
        ///     indicates what should be loaded.
        /// </param>
        /// <param name="textureManager">
        ///     The texture manager
        /// </param>
        /// <param name="x">
        ///     The X location of this tile in tile space.
        /// </param>
        /// <param name="y">
        ///     The Y location of this tile in tile space.
        /// </param>
        /// <returns>The loaded tile.</returns>
        private Tile LoadTile(Char tileChar, TextureManager textureManager, int x, int y)
        {
            Vector position = GetBounds(x, y).GetBottomCenter();

            switch (tileChar)
            {
                case 'B':
                    return new Tile("border", textureManager.Get("dirt_floor"), TileCollision.Impassable, position);
                case 'R':
                    return new Tile("rock_wall", textureManager.Get("rock_wall"), TileCollision.Impassable, position);
                case 'D':
                    return new Tile("dirt_floor", textureManager.Get("dirt_floor"), TileCollision.Passable, position);
                case 'S':
                    return LoadSkeleton(textureManager.Get("skeleton"), textureManager.Get("dirt_floor"), position);
                case 'N':
                    return LoadNorthSouthSkeleton(textureManager.Get("skeleton"), textureManager.Get("dirt_floor"),
                        position);
                case 'E':
                    return LoadEastWestSkeleton(textureManager.Get("skeleton"), textureManager.Get("dirt_floor"),
                        position);
                case 'Y':
                    return LoadEyeball(textureManager.Get("eyeball"), textureManager.Get("dirt_floor"), position, textureManager.Get("fireball"));
                case 'Z':
                    return LoadPlayer(textureManager.Get("skeleton"), textureManager.Get("dirt_floor"), position);
                case 'X':
                    return new Tile("exit", textureManager.Get("dirt_floor"), TileCollision.Impassable, position);
                default:
                    throw new NotSupportedException(
                        String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileChar, x, y));
            }
        }

        private Tile LoadPlayer(Texture texture, Texture floorTexture, Vector position)
        {
            _playerCharacter.SetPosition(position);

            return new Tile("entrance", floorTexture, TileCollision.Passable, position);
        }

        private Tile LoadSkeleton(Texture texture, Texture floorTexture, Vector position)
        {
            _enemyManager.EnemyList.Add(new Enemy(texture, new SkeletonBrain(), position));

            return new Tile("dirt_floor", floorTexture, TileCollision.Passable, position);
        }

        private Tile LoadEyeball(Texture texture, Texture floorTexture, Vector position, Texture missileTexture)
        {
            _enemyManager.EnemyList.Add(new Eyeball(texture, new EyeballBrain(), position, missileTexture, MissileManager.Instance));

            return new Tile("dirt_floor", floorTexture, TileCollision.Passable, position);
        }

        private Tile LoadEastWestSkeleton(Texture texture, Texture floorTexture, Vector position)
        {
            _enemyManager.EnemyList.Add(new EastWestSkeleton(texture, new EastWestSkeletonBrain(), position));

            return new Tile("dirt_floor", floorTexture, TileCollision.Passable, position);
        }

        private Tile LoadNorthSouthSkeleton(Texture texture, Texture floorTexture, Vector position)
        {
            _enemyManager.EnemyList.Add(new NorthSouthSkeleton(texture, new NorthSouthSkeletonBrain(), position));

            return new Tile("dirt_floor", floorTexture, TileCollision.Passable, position);
        }

        /// <summary>
        ///     Gets the bounding rectangle of a tile in world space.
        /// </summary>
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle((x * Tile.Width) + xOffset, (y * Tile.Height) + yOffset, Tile.Width, Tile.Height);
        }

        public bool HasPlayerDied()
        {
            return _playerCharacter.IsDead;
        }

        private static void UpdateCollisions<T>(List<List<T>> entityList) where T : IEntity
        {
            foreach (var entList in entityList)
            {
                foreach (var otherList in entityList)
                {
                    if (entList == otherList) continue;
                    foreach (var ent in entList)
                    {
                        var entBox = ent.GetBoundingBox();

                        foreach (var other in otherList)
                        {
                            var otherBox = other.GetBoundingBox();

                            if (!entBox.IntersectsWith(otherBox)) continue;
                            var depth = entBox.GetIntersectionDepth(otherBox);
                            if (depth == Vector.Zero) continue;
                            var absDepthX = Math.Abs(depth.X);
                            var absDepthY = Math.Abs(depth.Y);
                            var depthVector = absDepthX > absDepthY
                                ? new Vector(0, depth.Y, 0)
                                : new Vector(depth.X, 0, 0);
                            ent.OnCollision(other, depthVector);
                            other.OnCollision(ent, depthVector);
                        }

                    }
                }
            }
        }

        public void Update(double elapsedTime, double gameTime)
        {
            if (!IsGamePaused)
            {
                _playerCharacter.Update(elapsedTime);

                //_background.Update((float)elapsedTime);
                //_backgroundLayer.Update((float)elapsedTime);
                //var entityList = new List<List<IEntity>();
                var enemyList = new List<IEntity>(_enemyManager.EnemyList.ToList());
                var missileList = new List<IEntity>(_missileManager.MissileList.ToList());
                var enemyMissileList = new List<IEntity>(_missileManager.EnemyMissileList.ToList());
                System.Diagnostics.Debug.WriteLine("Enemy Count: " + enemyList.Count);
                System.Diagnostics.Debug.WriteLine("Missile Count: " + missileList.Count);
                System.Diagnostics.Debug.WriteLine("Enemy Missile Count: " + enemyMissileList.Count);
                var tileList = new List<IEntity>(_currentRoom.TileDictionary.Values.ToList().Where(a => a.TileCollision == TileCollision.Impassable));
                var entityList = new List<List<IEntity>>
                {
                    enemyList,
                    missileList,
                    enemyMissileList,
                    tileList,
                    new List<IEntity> {_playerCharacter}
                };

                UpdateCollisions(entityList);
                _enemyManager.Update(elapsedTime, gameTime);
                _missileManager.Update(elapsedTime);
                _effectsManager.Update(elapsedTime);
            }

            if (_enemyManager.EnemyList.Count <= 0)
            {
                AreAllEnemiesDead = true;
            }

            if (AreAllEnemiesDead && _playerCharacter.WaitingAtExit)
            {
                IsLevelComplete = true;
                _playerCharacter.WaitingAtExit = false;
                AreAllEnemiesDead = false;
            }
            else
            {
                AreAllEnemiesDead = false;
                _playerCharacter.WaitingAtExit = false;
                IsLevelComplete = false;
            }

            UpdateInput(elapsedTime);
        }

        private void UpdateInput(double elapsedTime)
        {
            if ((_input.Controller != null && _input.Controller.ButtonStart.Pressed) || _input.Keyboard.IsKeyHeld(Keys.Escape))
            {
                IsGamePaused = !IsGamePaused;
            }

            if (IsGamePaused) return;

            double x = 0;
            double y = 0;
            double u = 0;
            double v = 0;

            if (_input.Controller != null)
            {
                x = _input.Controller.LeftControlStick.X;
                y = _input.Controller.LeftControlStick.Y * -1;
                u = _input.Controller.RightControlStick.X;
                v = _input.Controller.RightControlStick.Y * -1;

                if (Math.Abs(u) > Math.Abs(v))
                {
                    u = u < 0 ? -1 : 1;
                }
                else if (Math.Abs(v) > Math.Abs(u))
                {
                    v = v < 0 ? -1 : 1;
                }
            }
            var controlInput = new Vector(x, y, 0);

            var attackInput = new Vector(u, v, 0);

            //if (!(Math.Abs(controlInput.Length()) < 0.0001)) return;

            // If the input is very small, then the player may not be using
            // a controller; he might be using the keyboard.

            if (_input.Keyboard.IsKeyHeld(Keys.W))
            {
                controlInput.Y = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.A))
            {
                controlInput.X = -1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.S))
            {
                controlInput.Y = -1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.D))
            {
                controlInput.X = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Left))
            {
                attackInput.X = -1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Right))
            {
                attackInput.X = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Up))
            {
                attackInput.Y = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Down))
            {
                attackInput.Y = -1;
            }

            if (attackInput != Vector.Zero)
            {
                _playerCharacter.Attack(attackInput);
            }

            _playerCharacter.Move(controlInput * elapsedTime);
        }

        public void Render(Renderer renderer)
        {
            //_background.Render(renderer);
            //_backgroundLayer.Render(renderer);

            _currentRoom.Render(renderer);

            _enemyManager.Render(renderer);
            _playerCharacter.Render(renderer);
            _missileManager.Render(renderer);
            _effectsManager.Render(renderer);
            renderer.Render();
        }
    }
}