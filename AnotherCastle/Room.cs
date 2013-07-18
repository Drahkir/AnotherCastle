using System.Collections.Generic;
using Engine;

namespace AnotherCastle
{
    public class Room
    {
        private readonly Dictionary<string, Tile> _tileDictionary;
        //private const double incrementY = -85;

        // Rooms are constructed such that the cells in the 2D array are assigned from NW (Top-Left) to SE (Bottom-Right)
        public Room(IEnumerable<Tile> tileList)
        {
            _tileDictionary = new Dictionary<string, Tile>();
            var i = 0;

            foreach (var tile in tileList)
            {
                var roomName = Constants.RoomNames[i++];
                _tileDictionary.Add(roomName, tile);
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
