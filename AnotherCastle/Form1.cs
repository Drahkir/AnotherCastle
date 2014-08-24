using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Input;
using Tao.DevIl;
using Tao.OpenGl;
using Font = Engine.Font;

namespace AnotherCastle
{
    public partial class Form1 : Form
    {
        private const bool Fullscreen = false;
        private readonly PersistentGameData _gameData = new PersistentGameData();
        private readonly Input _input = new Input();
        private readonly StateSystem _system = new StateSystem();
        private readonly TextureManager _textureManager = new TextureManager();
        private Font _generalFont;
        private Font _titleFont;
        //SoundManager _soundManager = new SoundManager();

        public Form1()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();

            _input.Mouse = new Mouse(this, simpleOpenGlControl1);
            _input.Keyboard = new Keyboard(simpleOpenGlControl1);

            InitializeDisplay();
            InitializeSounds();
            InitializeTextures();
            InitializeFonts();
            InitializeGameState();

            new FastLoop(GameLoop);
        }

        private void InitializeFonts()
        {
            _titleFont = new Font(_textureManager.Get("title_font"), FontParser.Parse("./Content/Fonts/title_font.fnt"));
            _generalFont = new Font(_textureManager.Get("general_font"),
                FontParser.Parse("./Content/Fonts/general_font.fnt"));
        }

        private void InitializeSounds()
        {
            // Sounds are loaded here
        }

        private void InitializeGameState()
        {
            var level = new LevelDescription();
            _gameData.CurrentLevel = level;

            // Game states are loaded here
            _system.AddState("start_menu", new StartMenuState(_titleFont, _generalFont, _input, _system));
            _system.AddState("inner_game", new InnerGameState(_system, _input, _textureManager, _gameData));
            _system.AddState("game_over", new GameOverState(_gameData, _system, _input, _generalFont, _titleFont));
            _system.ChangeState("start_menu");
        }

        private void InitializeTextures()
        {
            // Init DevIl
            Il.ilInit();
            Ilu.iluInit();
            Ilut.ilutInit();
            Ilut.ilutRenderer(Ilut.ILUT_OPENGL);

            // Textures are loaded here
            _textureManager.LoadTexture("title_font", "./Content/Fonts/title_font.tga");
            _textureManager.LoadTexture("general_font", "./Content/Fonts/general_font.tga");
            _textureManager.LoadTexture("pixela_up", "./Content/Sprites/pixela_up.png");
            _textureManager.LoadTexture("pixela_down", "./Content/Sprites/pixela_down.png");
            _textureManager.LoadTexture("pixela_left", "./Content/Sprites/pixela_left.png");
            _textureManager.LoadTexture("pixela_right", "./Content/Sprites/pixela_right.png");
            _textureManager.LoadTexture("villager", "./Content/Sprites/villager.png");
            _textureManager.LoadTexture("skeleton", "./Content/Sprites/skeleton.png");
            _textureManager.LoadTexture("background", "./Content/Backgrounds/background.png");
            _textureManager.LoadTexture("background_layer_1", "./Content/Backgrounds/background_p.tga");
            _textureManager.LoadTexture("dirt_floor2", "./Content/Tiles/dirt_floor2.png");
            _textureManager.LoadTexture("rock_wall2", "./Content/Tiles/rock_wall2.png");
            _textureManager.LoadTexture("heart_missile", "./Content/Sprites/heart_missile.png");
            _textureManager.LoadTexture("fireball", "./Content/Sprites/fireball.png");
            _textureManager.LoadTexture("eyeball", "./Content/Sprites/eyeball.png");
        }

        private void UpdateInput(double elapsedTime)
        {
            _input.Update(elapsedTime);
        }

        private void GameLoop(double elapsedTime)
        {
            UpdateInput(elapsedTime);
            _system.Update(elapsedTime);
            _system.Render();
            simpleOpenGlControl1.Refresh();
        }

        private void InitializeDisplay()
        {
            if (Fullscreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            ClientSize = new Size(1280, 720);
            //ClientSize = new Size(256, 240);
            Setup2DGraphics(ClientSize.Width, ClientSize.Height);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            Gl.glViewport(0, 0, ClientSize.Width, ClientSize.Height);
            Setup2DGraphics(ClientSize.Width, ClientSize.Height);
        }

        private static void Setup2DGraphics(double width, double height)
        {
            double halfWidth = width/2;
            double halfHeight = height/2;
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(-halfWidth, halfWidth, -halfHeight, halfHeight, -100, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }
    }
}