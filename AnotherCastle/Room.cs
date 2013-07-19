using System.Collections.Generic;
using System.Linq;
using Engine;

namespace AnotherCastle
{
    public class Room
    {
        public readonly Dictionary<string, Tile> TileDictionary;

        // Rooms are constructed such that the cells in the 2D array are assigned from NW (Top-Left) to SE (Bottom-Right)
        public Room(IEnumerable<Tile> tileList)
        {
            TileDictionary = new Dictionary<string, Tile>();
            var i = 0;

            foreach (var tile in tileList)
            {
                var roomName = Constants.RoomNames[i++];
                TileDictionary.Add(roomName, tile);
            }
        }

        public Tile GetCell(string cellName)
        {
            return TileDictionary[cellName];
        }

        public void Render(Renderer renderer)
        {
            foreach (var tile in TileDictionary.Select(tilePair => tilePair.Value))
            {
                tile.Render(renderer);
            }
        }
    }
}
