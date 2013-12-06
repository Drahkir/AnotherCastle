using System.Windows.Forms;
using Engine;
using Engine.Input;
using Tao.OpenGl;
using Button = Engine.Button;

namespace AnotherCastle
{
    internal class StartMenuState : IGameObject
    {
        private readonly Font _generalFont;
        private readonly Input _input;
        private readonly Renderer _renderer = new Renderer();
        private readonly StateSystem _system;
        private readonly Text _title;
        private VerticalMenu _menu;

        public StartMenuState(Font titleFont, Font generalFont, Input input, StateSystem system)
        {
            _input = input;
            _system = system;
            _generalFont = generalFont;
            InitializeMenu();
            _title = new Text("Another Castle", titleFont);
            _title.SetColor(new Color(0, 0, 0, 1));

            // Center on x and move toward the top
            _title.SetPosition(-_title.Width/2, 300);
        }

        public void Update(double elapsedTime)
        {
            _menu.HandleInput();
        }

        public void Render()
        {
            Gl.glClearColor(1, 1, 1, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            _renderer.DrawText(_title);
            _menu.Render(_renderer);
            _renderer.Render();
        }

        private void InitializeMenu()
        {
            _menu = new VerticalMenu(0, 150, _input);
            var startGame = new Button(delegate { _system.ChangeState("inner_game"); }, new Text("Start", _generalFont));
            var gameSettings = new Button(delegate
            {
                /* Go To Settings */
            }, new Text("Settings", _generalFont));
            var exitGame = new Button(delegate { Application.Exit(); }, new Text("Exit", _generalFont));

            _menu.AddButton(startGame);
            _menu.AddButton(gameSettings);
            _menu.AddButton(exitGame);
        }
    }
}