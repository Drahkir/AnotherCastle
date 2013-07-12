using Tao.OpenGl;
using Engine;
using Engine.Input;

namespace AnotherCastle
{
    class InnerGameState : IGameObject
    {
        Level _level;
        readonly TextureManager _textureManager;
        readonly Renderer _renderer = new Renderer();
        readonly Input _input;
        readonly StateSystem _system;
        readonly PersistentGameData _gameData;
        Font _generalFont;

        double _gameTime;

        public InnerGameState(StateSystem system, Input input, TextureManager textureManager, PersistentGameData gameData, Font generalFont)
        {
            _system = system;
            _input = input;
            _textureManager = textureManager;
            _gameData = gameData;
            _generalFont = generalFont;
            OnGameStart();
        }

        public void OnGameStart()
        {
            _level = new Level(_input, _textureManager, _gameData);
            _gameTime = _gameData.CurrentLevel.Time;
        }

        #region IGameObject Members

        public void Update(double elapsedTime)
        {
            _level.Update(elapsedTime, _gameTime);
            _gameTime += elapsedTime;

            if (_gameTime <= 0)
            {
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
