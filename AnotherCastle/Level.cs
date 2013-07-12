using System;
using System.Collections.Generic;
using Engine;
using Engine.Input;
using System.Windows.Forms;
using System.Drawing;

namespace AnotherCastle
{
    class Level
    {
        readonly Input _input;
        PersistentGameData _gameData;
        readonly PlayerCharacter _playerCharacter;
        //List<Enemy> _enemyList = new List<Enemy>();
        readonly EnemyManager _enemyManager;
        readonly MapObjectManager _mapObjectManager;
        readonly MissileManager _missileManager = new MissileManager(new RectangleF(-1300 / 2, -750 / 2, 1300, 750));
        readonly EffectsManager _effectsManager;
        readonly Room _currentRoom;

        public Level(Input input, TextureManager textureManager, PersistentGameData gameData)
        {
            var levelOne = new List<CellTypes> { 
                CellTypes.RockWall, CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,
                CellTypes.RockWall, CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.RockWall,    
                CellTypes.RockWall, CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.RockWall,    
                CellTypes.RockWall, CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.RockWall,    
                CellTypes.RockWall, CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.RockWall,    
                CellTypes.RockWall, CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.RockWall,    
                CellTypes.RockWall, CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.RockWall,    
                CellTypes.RockWall, CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.DirtFloor,  CellTypes.RockWall,    
                CellTypes.RockWall, CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,  CellTypes.RockWall,        
            };

            _currentRoom = new Room(textureManager, levelOne);
            _input = input;
            _gameData = gameData;
            var textureManager1 = textureManager;
            _effectsManager = new EffectsManager(textureManager1);
            _playerCharacter = new PlayerCharacter(textureManager1, _missileManager);
            _enemyManager = new EnemyManager(textureManager1, _effectsManager, _missileManager);
            _mapObjectManager = new MapObjectManager(textureManager1, _effectsManager, _currentRoom);
            //_enemyList.Add(new Enemy(_textureManager, _effectsManager));

            //_background = new ScrollingBackground(textureManager.Get("background"));
            //_background.SetScale(5, 5);
            //_background.Speed = 0.05f;

            //_backgroundLayer = new ScrollingBackground(textureManager.Get("background_layer_1"));
            //_backgroundLayer.Speed = 0.1f;
            //_backgroundLayer.SetScale(2.0, 2.0);
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

            foreach (var mapObject in _mapObjectManager.MapObjectList)
            {
                if (!mapObject.GetBoundingBox().IntersectsWith(_playerCharacter.GetBoundingBox())) continue;
                mapObject.OnCollision(_playerCharacter);
                _playerCharacter.OnCollision(mapObject);
            }
        }
        public void Update(double elapsedTime, double gameTime)
        {
            _playerCharacter.Update(elapsedTime);

            //_background.Update((float)elapsedTime);
            //_backgroundLayer.Update((float)elapsedTime);

            UpdateCollisions();
            _enemyManager.Update(elapsedTime, gameTime);
            _mapObjectManager.Update(elapsedTime, gameTime);
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
            var box = _playerCharacter.GetBoundingBox();

            foreach (var mapObject in _mapObjectManager.MapObjectList)
            {
                var objectBox = mapObject.GetBoundingBox();

                //if (box.IntersectsWith(objectBox))
                //{
                //    if (!leftBlock && (Math.Abs(objectBox.Right - left) < Math.Abs(objectBox.Left - left)) && !(objectBox.Top > bottom || objectBox.Bottom < top))
                //        leftBlock = true;

                //    if (!rightBlock && (Math.Abs(objectBox.Left - right) < Math.Abs(objectBox.Right - right)) && !(objectBox.Top > bottom || objectBox.Bottom < top))
                //        rightBlock = true;

                //    if (!topBlock && (Math.Abs(objectBox.Bottom - top) < Math.Abs(objectBox.Top - top)) && !(objectBox.Left > right || objectBox.Right < left))
                //        bottomBlock = true;

                //    if (!bottomBlock && (Math.Abs(objectBox.Top - bottom) < Math.Abs(objectBox.Bottom - bottom)) && !(objectBox.Left > right || objectBox.Right < left))
                //        topBlock = true;
                //}

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
            _mapObjectManager.Render(renderer);
            _playerCharacter.Render(renderer);
            _missileManager.Render(renderer);
            _effectsManager.Render(renderer);
            renderer.Render();
        }
    }
}