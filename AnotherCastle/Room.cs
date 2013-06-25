using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;

namespace AnotherCastle
{
    public class Room
    {
        private List<Cell> _cells;
        private const double startX = -600;
        private const double startY = 340;
        private const double incrementX = 85;
        private const double incrementY = -85;

        // Rooms are constructed such that the cells in the 2D array are assigned from NW (Top-Left) to SE (Bottom-Right)
        public Room(TextureManager textureManager, List<CellTypes> cellTypeList)
        {
            var curX = startX;
            var curY = startY;
            _cells = new List<Cell>();

            foreach (var cellType in cellTypeList)
            {
                _cells.Add(new Cell(textureManager, cellType, curX, curY));
                curX += incrementX;

                if(curX >= 600) {
                    curX = startX;
                    curY += incrementY;
                }
            }
        }


        public void Render(Renderer renderer)
        {
            foreach (var cell in _cells)
            {
                cell.Render(renderer);
            }
        }
    }
}
