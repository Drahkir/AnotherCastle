using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Input;

namespace AnotherCastle
{
    public partial class Form1 : Form
    {
        bool _fullscreen = false;
        FastLoop _fastLoop;
        StateSystem _system = new StateSystem();
        Input _input = new Input();
        TextureManager _textureManager = new TextureManager();
        Engine.Font _generalFont;
        Engine.Font _titleFont;
        PersistentGameData _persistentGameData = new PersistentGameData();

        public Form1()
        {
            InitializeComponent();
        }
    }
}
