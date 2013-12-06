using System;
using System.IO;
using Engine;
using Engine.Input;
using Tao.OpenGl;

namespace AnotherCastle
{
    internal class InnerGameState : IGameObject
    {
        private const int MaxLevel = 2;
        private readonly PersistentGameData _gameData;
        private readonly Input _input;
        private readonly Renderer _renderer = new Renderer();
        private readonly StateSystem _system;
        private readonly TextureManager _textureManager;
        private int _currentLevel;
        private double _gameTime;
        private Level _level;

        public InnerGameState(StateSystem system, Input input, TextureManager textureManager,
            PersistentGameData gameData)
        {
            _system = system;
            _input = input;
            _textureManager = textureManager;
            _gameData = gameData;
            OnGameStart();
        }

        public void OnGameStart()
        {
            using (FileStream fileStream = File.Open("./Content/Levels/0.txt", FileMode.Open))
            {
                _level = new Level(_input, _textureManager, fileStream);
                //_gameTime = _gameData.CurrentLevel.Time;
            }
        }

        private void LoadNextLevel()
        {
            string fileString = String.Format("./Content/Levels/{0}.txt", ++_currentLevel);

            using (FileStream fileStream = File.Open(fileString, FileMode.Open))
            {
                _level = new Level(_input, _textureManager, fileStream);
            }
        }

        #region IGameObject Members

        public void Update(double elapsedTime)
        {
            _level.Update(elapsedTime, _gameTime);
            _gameTime += elapsedTime;

            if (_level.IsLevelComplete)
            {
                if (_currentLevel == MaxLevel) _currentLevel = -1;
                _gameTime = 0;
                LoadNextLevel();
                //OnGameStart();
                //_gameData.JustWon = true;
                //_system.ChangeState("game_over");
            }

            if (_level.HasPlayerDied())
            {
                OnGameStart();
                _gameData.JustWon = false;
                _system.ChangeState("game_over");
            }
        }

        public void Render()
        {
            //Gl.glClearColor(1, 0, 1, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            _level.Render(_renderer);
            _renderer.Render();
        }

        #endregion
    }
}