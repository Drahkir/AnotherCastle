using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Drawing;

namespace AnotherCastle
{
    public class Cell : Entity
    {
        private CellTypes _cellType;
        private string _cellName;
        public double X;
        public double Y;

        public Cell(string cellName, TextureManager textureManager, CellTypes cellType, double x, double y)
        {
            X = x;
            Y = y;
            CellName = cellName;
            CellType = cellType;
            _sprite.SetPosition(x, y);
            if (cellType == CellTypes.rock_wall)
                _sprite.Texture = textureManager.Get("rock_wall");
            else
                _sprite.Texture = textureManager.Get("dirt_floor");
        }

        public string CellName
        {
            get { return _cellName; }
            set { _cellName = value; }
        }

        public CellTypes CellType
        {
            get { return _cellType; }
            set { _cellType = value; }
        }
        // Need a constructor to convert XML (?) data to the cell array
        //public BuildCellArray() {

        //}

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(_sprite);
        }
    }
}
