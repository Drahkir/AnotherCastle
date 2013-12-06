using System.Windows.Forms;
using Engine;
using Engine.Input;
using Tao.OpenGl;

namespace AnotherCastle
{
    internal class GameOverState : IGameObject
    {
        private const double TimeOut = 4;
        private readonly Text _blurbLose;
        private readonly Text _blurbWin;
        private readonly PersistentGameData _gameData;
        private readonly Input _input;
        private readonly Renderer _renderer = new Renderer();
        private readonly StateSystem _system;

        private readonly Text _titleLose;
        private readonly Text _titleWin;
        private double _countDown = TimeOut;

        public GameOverState(PersistentGameData data, StateSystem system, Input input, Font generalFont, Font titleFont)
        {
            _gameData = data;
            _system = system;
            _input = input;
            Font generalFont1 = generalFont;
            Font titleFont1 = titleFont;

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
            text.SetPosition(-text.Width/2, yPosition);
            text.SetColor(new Color(0, 0, 0, 1));
        }

        #region IGameObject Members

        public void Update(double elapsedTime)
        {
            _countDown -= elapsedTime;

            if (_countDown <= 0 || (_input.Controller != null && _input.Controller.ButtonA.Pressed) ||
                _input.Keyboard.IsKeyPressed(Keys.Enter))
            {
                Finish();
            }
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

        private void Finish()
        {
            _gameData.JustWon = false;
            _system.ChangeState("start_menu");
            _countDown = TimeOut;
        }

        #endregion
    }
}