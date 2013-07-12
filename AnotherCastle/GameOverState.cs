using Engine;
using Engine.Input;
using Tao.OpenGl;

namespace AnotherCastle
{
    class GameOverState : IGameObject
    {
        const double TimeOut = 4;
        double _countDown = TimeOut;

        readonly StateSystem _system;
        readonly Input _input;
        readonly PersistentGameData _gameData;
        readonly Renderer _renderer = new Renderer();

        readonly Text _titleWin;
        readonly Text _blurbWin;

        readonly Text _titleLose;
        readonly Text _blurbLose;

        public GameOverState(PersistentGameData data, StateSystem system, Input input, Font generalFont, Font titleFont)
        {
            _gameData = data;
            _system = system;
            _input = input;
            var generalFont1 = generalFont;
            var titleFont1 = titleFont;

            _titleWin = new Text("Complete!", titleFont1);
            _blurbWin = new Text("Congratulations, you won!", generalFont1);
            _titleLose = new Text("Game Over!", titleFont1);
            _blurbLose = new Text("Please try again...", generalFont1);

            FormatText(_titleWin, 300);
            FormatText(_blurbWin, 200);

            FormatText(_titleLose, 300);
            FormatText(_blurbLose, 200);

        }

        private static void FormatText(Text text, int yPosition)
        {
            text.SetPosition(-text.Width / 2, yPosition);
            text.SetColor(new Color(0, 0, 0, 1));
        }

        #region IGameObject Members

        public void Update(double elapsedTime)
        {
            _countDown -= elapsedTime;

            if (_countDown <= 0 || (_input.Controller != null && _input.Controller.ButtonA.Pressed) || _input.Keyboard.IsKeyPressed(System.Windows.Forms.Keys.Enter))
            {
                Finish();
            }
        }

        private void Finish()
        {
            _gameData.JustWon = false;
            _system.ChangeState("start_menu");
            _countDown = TimeOut;
        }

        public void Render()
        {
            Gl.glClearColor(1, 1, 1, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            if (_gameData.JustWon)
            {
                _renderer.DrawText(_titleWin);
                _renderer.DrawText(_blurbWin);
            }

            else
            {
                _renderer.DrawText(_titleLose);
                _renderer.DrawText(_blurbLose);
            }

            _renderer.Render();
        }
        #endregion
    }
}