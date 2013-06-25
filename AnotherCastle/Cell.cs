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

        public Cell(TextureManager textureManager, CellTypes cellType, double x, double y)
        {
            _cellType = cellType;
            _sprite.SetPosition(x, y);
            if (cellType == CellTypes.rock_wall)
                _sprite.Texture = textureManager.Get("rock_wall");
            else
                _sprite.Texture = textureManager.Get("dirt_floor");
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
