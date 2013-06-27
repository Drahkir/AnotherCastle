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
        private Dictionary<string, Cell> _cellDictionary;
        private const double startX = -600;
        private const double startY = 340;
        private const double incrementX = 85;
        private const double incrementY = -85;

        // Rooms are constructed such that the cells in the 2D array are assigned from NW (Top-Left) to SE (Bottom-Right)
        public Room(TextureManager textureManager, List<CellTypes> cellTypeList)
        {
            var curX = startX;
            var curY = startY;
            _cellDictionary = new Dictionary<string, Cell>();
            int i = 0;

            foreach (var cellType in cellTypeList)
            {
                var roomName = Constants.RoomNames[i++];
                _cellDictionary.Add(roomName, new Cell(roomName, textureManager, cellType, curX, curY));
                curX += incrementX;

                if(curX >= 600) {
                    curX = startX;
                    curY += incrementY;
                }
            }
        }

        public Cell GetCell(string cellName)
        {
            return _cellDictionary[cellName];
        }

        public void Render(Renderer renderer)
        {
            foreach (var cellPair in _cellDictionary)
            {
                var cell = cellPair.Value;
                cell.Render(renderer);
            }
        }
    }
}
