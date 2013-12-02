﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engine;
using Engine.Input;
using System.Windows.Forms;
using System.Drawing;
using Platformer;

namespace AnotherCastle
{
    class Level
    {
        private Tile[,] tiles;
        readonly Input _input;
        readonly PlayerCharacter _playerCharacter;
        readonly EnemyManager _enemyManager;
        readonly MissileManager _missileManager = new MissileManager(new RectangleF(-1300 / 2, -750 / 2, 1300, 750));
        readonly EffectsManager _effectsManager;
        readonly Room _currentRoom;
        //MissileManager _missileManager = new MissileManager(new RectangleF(-1300 / 2, -750 / 2, 1300, 750));
        private const int xOffset = -650;
        private const int yOffset = -410;
        public bool IsLevelComplete { get; set; }
        public bool IsGamePaused { get; set; }

        public Level(Input input, TextureManager textureManager, Stream mapFile)
        {
            _input = input;
            _effectsManager = new EffectsManager(textureManager);
            _playerCharacter = new PlayerCharacter(textureManager, _missileManager);
            _enemyManager = new EnemyManager();
            LoadTiles(mapFile, textureManager);
            _currentRoom = new Room(tiles);

            //_background = new ScrollingBackground(textureManager.Get("background"));
            //_background.SetScale(5, 5);
            //_background.Speed = 0.05f;

            //_backgroundLayer = new ScrollingBackground(textureManager.Get("background_layer_1"));
            //_backgroundLayer.Speed = 0.1f;
            //_backgroundLayer.SetScale(2.0, 2.0);
        }

        private void LoadTiles(Stream mapFile, TextureManager textureManager)
        {
            // Load the level and ensure all of the lines are the same length.
            int width;
            var lines = new List<string>();
            using (var reader = new StreamReader(mapFile))
            {
                var line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            tiles = new Tile[width, lines.Count];

            // Loop over every tile position,
            for (var y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    var tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, textureManager, x, y);
                }
            }

            // Verify that the level has a beginning and an end.
            //if (Player == null)
            //    throw new NotSupportedException("A level must have a starting point.");
            //if (exit == InvalidPosition)
            //    throw new NotSupportedException("A level must have an exit.");
        }

        /// <summary>
        /// Loads an individual tile's appearance and behavior.
        /// </summary>
        /// <param name="tileChar">
        /// The character loaded from the structure file which
        /// indicates what should be loaded.
        /// </param>
        /// <param name="textureManager">
        /// The texture manager
        /// </param>
        /// <param name="x">
        /// The X location of this tile in tile space.
        /// </param>
        /// <param name="y">
        /// The Y location of this tile in tile space.
        /// </param>
        /// <returns>The loaded tile.</returns>
        private Tile LoadTile(Char tileChar, TextureManager textureManager, int x, int y)
        {
            var position = GetBounds(x, y).GetBottomCenter();

            switch (tileChar)
            {
                case '1':
                    return LoadPlayer(textureManager.Get("skeleton"), textureManager.Get("dirt_floor"), position);
                case 'R':
                    return new Tile("rock_wall", textureManager.Get("rock_wall"), TileCollision.Impassable, position);
                case 'D':
                    return new Tile("dirt_floor", textureManager.Get("dirt_floor"), TileCollision.Passable, position);
                case 'S':
                    return LoadSkeleton(textureManager.Get("skeleton"), textureManager.Get("dirt_floor"), position);
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileChar, x, y));
            }
        }

        private Tile LoadPlayer(Texture texture, Texture floorTexture, Vector position)
        {
            _playerCharacter.SetPosition(position);

            return new Tile("dirt_floor", floorTexture, TileCollision.Passable, position);
        }

        private Tile LoadSkeleton(Texture texture, Texture floorTexture, Vector position)
        {
            _enemyManager.EnemyList.Add(new Enemy(texture, new SkeletonBrain(), position));

            return new Tile("dirt_floor", floorTexture, TileCollision.Passable, position);
        }

        /// <summary>
        /// Gets the bounding rectangle of a tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle((x * Tile.Width) + xOffset, (y * Tile.Height) + yOffset, Tile.Width, Tile.Height);
        }

        /// <summary>
        /// Width of level measured in tiles.
        /// </summary>
        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        public bool HasPlayerDied()
        {
            return _playerCharacter.IsDead;
        }

        private void UpdateCollisions()
        {
            _missileManager.UpdatePlayerCollision(_playerCharacter);

            foreach (var enemy in _enemyManager.EnemyList)
            {
                if (enemy.GetBoundingBox().IntersectsWith(_playerCharacter.GetBoundingBox()))
                {
                    enemy.OnCollision(_playerCharacter);
                    _playerCharacter.OnCollision(enemy);
                }

                _missileManager.UpdateEnemyCollisions(enemy);
            }
        }

        public void Update(double elapsedTime, double gameTime)
        {
            if (!IsGamePaused)
            {
                _playerCharacter.Update(elapsedTime);

                //_background.Update((float)elapsedTime);
                //_backgroundLayer.Update((float)elapsedTime);

                UpdateCollisions();
                _enemyManager.Update(elapsedTime, gameTime);
                _missileManager.Update(elapsedTime);
                _effectsManager.Update(elapsedTime);
            }

            if (_enemyManager.EnemyList.Count <= 0)
            {
                IsLevelComplete = true;
            }

            UpdateInput(elapsedTime);
        }

        private void UpdateInput(double elapsedTime)
        {

            //if (_input.Keyboard.IsKeyHeld(Keys.Space) || _input.Keyboard.IsKeyPressed(Keys.Space) || (_input.Controller != null && _input.Controller.ButtonA.Pressed))
            //{
            //    _playerCharacter.Attack();
            //}

            // Get controls and apply to player character

            if (_input.Controller.ButtonStart.Pressed || _input.Keyboard.IsKeyHeld(Keys.Escape))
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

            foreach (var tilePair in _currentRoom.TileDictionary)
            {
                Vector depth;
                double absDepthX, absDepthY;
                var tile = tilePair.Value;
                if (tile.TileCollision != TileCollision.Impassable) continue;
                var objectBox = tile.GetBoundingBox();

                // Check missiles for collisions
                foreach (var missile in _missileManager.MissileList.Where(a => a.Dead == false))
                {
                    var missileBox = missile.GetBoundingBox();

                    var missileDepth = missileBox.GetIntersectionDepth(objectBox);

                    if (missileDepth == Vector.Zero) continue;

                    missile.HandleCollision();
                }

                // Check enemies for collisions
                foreach (var enemy in _enemyManager.EnemyList)
                {
                    var enemyBox = enemy.GetBoundingBox();

                    depth = enemyBox.GetIntersectionDepth(objectBox);

                    if (depth == Vector.Zero) continue;
                    absDepthX = Math.Abs(depth.X);
                    absDepthY = Math.Abs(depth.Y);

                    enemy.HandleCollision(absDepthX > absDepthY
                        ? new Vector(0, depth.Y, 0)
                        : new Vector(depth.X, 0, 0));
                }

                var box = _playerCharacter.GetBoundingBox();

                depth = box.GetIntersectionDepth(objectBox);

                if (depth == Vector.Zero) continue;
                absDepthX = Math.Abs(depth.X);
                absDepthY = Math.Abs(depth.Y);

                _playerCharacter.HandleCollision(absDepthX > absDepthY
                    ? new Vector(0, depth.Y, 0)
                    : new Vector(depth.X, 0, 0));

                // Perform further collisions with the new bounds.
                //bounds = BoundingRectangle;

            }

            foreach (var enemy in _enemyManager.EnemyList)
            {
                var box = enemy.GetBoundingBox();

                foreach (var otherEnemy in _enemyManager.EnemyList.Where(a => a != enemy))
                {
                    var otherEnemyBox = otherEnemy.GetBoundingBox();

                    var depth = box.GetIntersectionDepth(otherEnemyBox);

                    if (depth == Vector.Zero) continue;
                    var absDepthX = Math.Abs(depth.X);
                    var absDepthY = Math.Abs(depth.Y);

                    enemy.HandleCollision(absDepthX > absDepthY
                        ? new Vector(0, depth.Y, 0)
                        : new Vector(depth.X, 0, 0));
                    // Perform further collisions with the new bounds.
                    //bounds = BoundingRectangle;
                }
            }

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