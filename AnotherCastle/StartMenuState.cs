using System;
using Tao.OpenGl;
using Engine;
using Engine.Input;

namespace AnotherCastle
{
    class StartMenuState : IGameObject
    {
        readonly Renderer _renderer = new Renderer();
        readonly Text _title;
        readonly Engine.Font _generalFont;
        readonly Input _input;
        VerticalMenu _menu;
        readonly StateSystem _system;

        public StartMenuState(Engine.Font titleFont, Engine.Font generalFont, Input input, StateSystem system)
        {
            _input = input;
            _system = system;
            _generalFont = generalFont;
            InitializeMenu();
            _title = new Text("Another Castle", titleFont);
            _title.SetColor(new Color(0, 0, 0, 1));

            // Center on x and move toward the top
            _title.SetPosition(-_title.Width / 2, 300);
        }

        private void InitializeMenu()
        {
            _menu = new VerticalMenu(0, 150, _input);
            var startGame = new Button(delegate(object o, EventArgs e) { _system.ChangeState("inner_game"); }, new Text("Start", _generalFont));
            var gameSettings = new Button(delegate(object o, EventArgs e) { /* Go To Settings */ }, new Text("Settings", _generalFont));
            var exitGame = new Button(delegate(object o, EventArgs e) { System.Windows.Forms.Application.Exit(); }, new Text("Exit", _generalFont));

            _menu.AddButton(startGame);
            _menu.AddButton(gameSettings);
            _menu.AddButton(exitGame);
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
    }
}
