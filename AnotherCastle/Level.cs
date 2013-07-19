using System;
using System.Collections.Generic;
using System.IO;
using Engine;
using Engine.Input;
using System.Windows.Forms;
using System.Drawing;

namespace AnotherCastle
{
    class Level
    {
        readonly Input _input;
        readonly PlayerCharacter _playerCharacter;
        readonly EnemyManager _enemyManager;
        readonly MissileManager _missileManager = new MissileManager(new RectangleF(-1300 / 2, -750 / 2, 1300, 750));
        readonly EffectsManager _effectsManager;
        readonly Room _currentRoom;
        private const double StartX = -600;
        private const double StartY = 340;
        private const double IncrementX = 85;
        private const double IncrementY = -85;

        public Level(Input input, TextureManager textureManager, Stream mapFile)
        {
            var levelOne = LoadTiles(mapFile, textureManager);
            _currentRoom = new Room(levelOne);
            _input = input;
            var textureManager1 = textureManager;
            _effectsManager = new EffectsManager(textureManager1);
            _playerCharacter = new PlayerCharacter(textureManager1);
            _enemyManager = new EnemyManager(textureManager1, _effectsManager, _missileManager);

            //_background = new ScrollingBackground(textureManager.Get("background"));
            //_background.SetScale(5, 5);
            //_background.Speed = 0.05f;

            //_backgroundLayer = new ScrollingBackground(textureManager.Get("background_layer_1"));
            //_backgroundLayer.Speed = 0.1f;
            //_backgroundLayer.SetScale(2.0, 2.0);
        }

        private static IEnumerable<Tile> LoadTiles(Stream mapFile, TextureManager textureManager)
        {
            var curX = StartX;
            var curY = StartY;
            var tileList = new List<Tile>();

            using(var reader = new StreamReader(mapFile))
            {
                while (!reader.EndOfStream)
                {
                    var tileChar = (char)reader.Read();
                    if (tileChar == '\r' || tileChar == '\n') continue;

                    var tile = LoadTile(tileChar, textureManager);
                    tile.SetPosition(curX, curY);
                    tileList.Add(tile);
                    curX += IncrementX;

                    if (!(curX >= 600)) continue;
                    curX = StartX;
                    curY += IncrementY;
                }
            }
            return tileList;
        }

        private static Tile LoadTile(Char tileChar, TextureManager textureManager)
        {
            switch (tileChar)
            {
                case 'R':
                    return new Tile("rock_wall", textureManager.Get("rock_wall"), TileCollision.Impassable);
                case 'D':
                    return new Tile("dirt_floor", textureManager.Get("dirt_floor"), TileCollision.Passable);
                default:
                    throw new ArgumentNullException("tileChar");
            }
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
            _playerCharacter.Update(elapsedTime);

            //_background.Update((float)elapsedTime);
            //_backgroundLayer.Update((float)elapsedTime);

            UpdateCollisions();
            _enemyManager.Update(elapsedTime, gameTime);
            _missileManager.Update(elapsedTime);
            _effectsManager.Update(elapsedTime);

            UpdateInput(elapsedTime);
        }

        private void UpdateInput(double elapsedTime)
        {

            if (_input.Keyboard.IsKeyHeld(Keys.Space) || _input.Keyboard.IsKeyPressed(Keys.Space) || (_input.Controller != null && _input.Controller.ButtonA.Pressed))
            {
                _playerCharacter.Attack();
            }

            // Get controls and apply to player character
            double x = 0;
            double y = 0;

            if (_input.Controller != null)
            {
                x = _input.Controller.LeftControlStick.X;
                y = _input.Controller.LeftControlStick.Y * -1;
            }
            var controlInput = new Vector(x, y, 0);

            if (!(Math.Abs(controlInput.Length()) < 0.0001)) return;

            // If the input is very small, then the player may not be using
            // a controller; he might be using the keyboard.

            foreach (var tilePair in _currentRoom.TileDictionary)
            {
                var box = _playerCharacter.GetBoundingBox();
                var tile = tilePair.Value;
                if (tile.TileCollision != TileCollision.Impassable) continue;
                var objectBox = tile.GetBoundingBox();

                var depth = box.GetIntersectionDepth(objectBox);

                if (depth == Vector.Zero) continue;
                var absDepthX = Math.Abs(depth.X);
                var absDepthY = Math.Abs(depth.Y);

                _playerCharacter.HandleCollision(absDepthX > absDepthY
                    ? new Vector(0, depth.Y, 0)
                    : new Vector(depth.X, 0, 0));

                // Perform further collisions with the new bounds.
                //bounds = BoundingRectangle;
            }

            foreach (var enemy in _enemyManager.EnemyList)
            {
                var box = enemy.GetBoundingBox();
                foreach (var tilePair in _currentRoom.TileDictionary)
                {
                    var tile = tilePair.Value;
                    if (tile.TileCollision != TileCollision.Impassable) continue;
                    var objectBox = tile.GetBoundingBox();

                    var depth = box.GetIntersectionDepth(objectBox);

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

            if (_input.Keyboard.IsKeyHeld(Keys.Left))
            {
                _playerCharacter.MoveLeft();
                controlInput.X = -1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Right))
            {
                _playerCharacter.MoveRight();
                controlInput.X = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Up))
            {
                _playerCharacter.MoveUp();
                controlInput.Y = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Down))
            {
                _playerCharacter.MoveDown();
                controlInput.Y = -1;
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