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
        private Dictionary<string, Tile> _tileDictionary;
        //private const double startX = -600;
        //private const double startY = 340;
        //private const double incrementX = 85;
        //private const double incrementY = -85;

        // Rooms are constructed such that the cells in the 2D array are assigned from NW (Top-Left) to SE (Bottom-Right)
        public Room(TextureManager textureManager, IEnumerable<Tile> tileList)
        {
            //var curX = startX;
            //var curY = startY;
            _tileDictionary = new Dictionary<string, Tile>();
            var i = 0;

            foreach (var tile in tileList)
            {
                var roomName = Constants.RoomNames[i++];
                //tile.X = curX;
                //tile.Y = curY;
                _tileDictionary.Add(roomName, tile);

                //curX += incrementX;

                //if (!(curX >= 600)) continue;
                //curX = startX;
                //curY += incrementY;
            }
        }

        public Tile GetCell(string cellName)
        {
            return _tileDictionary[cellName];
        }

        public void Render(Renderer renderer)
        {
            foreach (var tilePair in _tileDictionary)
            {
                var tile = tilePair.Value;
                tile.Render(renderer);
            }
        }
    }
}
