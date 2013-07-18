using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace AnotherCastle
{
    public class MapObjectDef
    {
        public string MapObjectType { get; set; }
        public double LaunchTime { get; set; }
        public Vector SpawnPoint { get; set; }
        public string TileName { get; set; }

        public MapObjectDef()
        {
            MapObjectType = "rock_wall";
            LaunchTime = 0;
            SpawnPoint = RandomSpawnPoint();
        }

        public MapObjectDef(string mapObjectType, double launchTime, Tile tile)
        {
            MapObjectType = mapObjectType;
            LaunchTime = launchTime;
            TileName = tile.TileName;
            SpawnPoint = new Vector(tile.X, tile.Y, 0);
        }

        public MapObjectDef(string mapObjectType, double launchTime, Vector spawnPoint)
        {
            MapObjectType = mapObjectType;
            LaunchTime = launchTime;
            SpawnPoint = spawnPoint;
        }

        public Vector RandomSpawnPoint()
        {
            //throw new NotImplementedException();
            return new Vector(0, 0, 0);
        }
    }
}
