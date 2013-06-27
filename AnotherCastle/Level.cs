using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Input;
using System.Windows.Forms;
using System.Drawing;
using Tao.OpenGl;

namespace AnotherCastle
{
    class Level
    {
        Input _input;
        PersistentGameData _gameData;
        PlayerCharacter _playerCharacter;
        TextureManager _textureManager;
        ScrollingBackground _background;
        ScrollingBackground _backgroundLayer;
        //List<Enemy> _enemyList = new List<Enemy>();
        EnemyManager _enemyManager;
        MapObjectManager _mapObjectManager;
        MissileManager _missileManager = new MissileManager(new RectangleF(-1300 / 2, -750 / 2, 1300, 750));
        EffectsManager _effectsManager;
        Room[][] _rooms;
        Room _currentRoom;
        private List<CellTypes> _levelOne;

        public Level(Input input, TextureManager textureManager, PersistentGameData gameData)
        {
            _levelOne = new List<CellTypes> { 
                CellTypes.rock_wall, CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,
                CellTypes.rock_wall, CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.rock_wall,    
                CellTypes.rock_wall, CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.rock_wall,    
                CellTypes.rock_wall, CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.rock_wall,    
                CellTypes.rock_wall, CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.rock_wall,    
                CellTypes.rock_wall, CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.rock_wall,    
                CellTypes.rock_wall, CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.rock_wall,    
                CellTypes.rock_wall, CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.dirt_floor,  CellTypes.rock_wall,    
                CellTypes.rock_wall, CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,  CellTypes.rock_wall,        
            };
            _currentRoom = new Room(textureManager, _levelOne);
            _input = input;
            _gameData = gameData;
            _textureManager = textureManager;
            _effectsManager = new EffectsManager(_textureManager);
            _playerCharacter = new PlayerCharacter(_textureManager, _missileManager);
            _enemyManager = new EnemyManager(_textureManager, _effectsManager, _missileManager, new ClientBounds { NorthBound = 400, SouthBound = -400, EastBound = -640, WestBound = 640, });
            _mapObjectManager = new MapObjectManager(_textureManager, _effectsManager, _currentRoom);
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

            foreach (Enemy enemy in _enemyManager.EnemyList)
            {
                if (enemy.GetBoundingBox().IntersectsWith(_playerCharacter.GetBoundingBox()))
                {
                    enemy.OnCollision(_playerCharacter);
                    _playerCharacter.OnCollision(enemy);
                }

                _missileManager.UpdateEnemyCollisions(enemy);
            }

            foreach (MapObject mapObject in _mapObjectManager.MapObjectList)
            {
                if (mapObject.GetBoundingBox().IntersectsWith(_playerCharacter.GetBoundingBox()))
                {
                    mapObject.OnCollision(_playerCharacter);
                    _playerCharacter.OnCollision(mapObject);
                }
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
            double _x = 0;
            double _y = 0;

            if (_input.Controller != null)
            {
                _x = _input.Controller.LeftControlStick.X;
                _y = _input.Controller.LeftControlStick.Y * -1;
            }
            Vector controlInput = new Vector(_x, _y, 0);

            if (Math.Abs(controlInput.Length()) < 0.0001)
            {
                // If the input is very small, then the player may not be using
                // a controller; he might be using the keyboard.
                var box = _playerCharacter.GetBoundingBox();
                var left = box.Left;
                var right = box.Right;
                var top = box.Top;
                var bottom = box.Bottom;
                var leftBlock = false;
                var rightBlock = false;
                var topBlock = false;
                var bottomBlock = false;

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

                    Vector depth = RectangleExtensions.GetIntersectionDepth(box, objectBox);


                    if (depth != Vector.Zero)
                    {
                        double absDepthX = Math.Abs(depth.X);
                        double absDepthY = Math.Abs(depth.Y);

                        if (absDepthX > absDepthY)
                        {
                            // Resolve the collision along the X axis.
                            _playerCharacter.HandleCollision(new Vector(0, depth.Y, 0));
                        }
                        else
                        {
                            // Resolve the collision along the Y axis.
                            _playerCharacter.HandleCollision(new Vector(depth.X, 0, 0));                            
                        }
                        // Perform further collisions with the new bounds.
                        //bounds = BoundingRectangle;
                    }
                }

                if (_input.Keyboard.IsKeyHeld(Keys.Left))
                {
                    _playerCharacter.MoveLeft();
                    if (leftBlock) controlInput.X = 0;
                    else controlInput.X = -1;
                }

                if (_input.Keyboard.IsKeyHeld(Keys.Right))
                {
                    _playerCharacter.MoveRight();
                    if (rightBlock) controlInput.X = 0;
                    else controlInput.X = 1;
                }

                if (_input.Keyboard.IsKeyHeld(Keys.Up))
                {
                    _playerCharacter.MoveUp();
                    if (topBlock) controlInput.X = 0;
                    else controlInput.Y = 1;
                }

                if (_input.Keyboard.IsKeyHeld(Keys.Down))
                {
                    _playerCharacter.MoveDown();
                    if (bottomBlock) controlInput.X = 0;
                    else controlInput.Y = -1;
                }

                _playerCharacter.Move(controlInput * elapsedTime);
            }
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