using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace AnotherCastle
{
    public class MapObjectManager
    {
        List<MapObject> _mapObjects = new List<MapObject>();
        List<MapObjectDef> _upComingMapObjects = new List<MapObjectDef>();
        TextureManager _textureManager;
        EffectsManager _effectsManager;
        Room _room;

        public List<MapObject> MapObjectList
        {
            get
            {
                return _mapObjects;
            }
        }

        public MapObjectManager(TextureManager textureManager, EffectsManager effectsManager, Room room)
        {
            _textureManager = textureManager;
            _effectsManager = effectsManager;
            _room = room;
            //_leftBound = leftBound;


            _upComingMapObjects.Add(new MapObjectDef("rock_wall", 0, _room.GetCell("5E")));
            _upComingMapObjects.Add(new MapObjectDef("rock_wall", 0, _room.GetCell("5F")));
            //_upComingMapObjects.Add(new MapObjectDef("rock_wall", 0, _room.GetCell("5G")));
            // Sort mapObjects so the greater launch time appears first.
            //_upComingMapObjects.Sort(delegate(MapObjectDef firstMapObject, MapObjectDef secondMapObject)
            //{
            //    return firstMapObject.LaunchTime.CompareTo(secondMapObject.LaunchTime);
            //});
        }

        private void QueueUpcomingMapObjects(double gameTime)
        {
            //_upComingMapObjects.Add(new MapObjectDef("rock_wall", 0, _room.GetCell("5B")));
            //var prng = new Random();
            //var typesList = Enum.GetNames(typeof(MapObjectType));
            //int numTypes = typesList.Length;
            //var randomMapObject = typesList[prng.Next(0, numTypes)];
            //var randomCount = prng.Next(1, 1);
            //var launchTime = gameTime + 2;

            //for (int i = 0; i < randomCount; i++)
            //{
            //    _upComingMapObjects.Add(new MapObjectDef(MapObjectType.rock_wall.ToString(), launchTime));
            //    launchTime += 2;
            //}
        }

        private void UpdateMapObjectSpawns(double gameTime)
        {
            // If no upcoming mapObjects then there's nothing to spawn
            if (_upComingMapObjects.Count == 0)
            {
                //QueueUpcomingMapObjects(gameTime);
                return;
            }

            MapObjectDef lastElement = _upComingMapObjects[_upComingMapObjects.Count - 1];
            if (gameTime > lastElement.LaunchTime)
            {
                _upComingMapObjects.RemoveAt(_upComingMapObjects.Count - 1);
                _mapObjects.Add(CreateMapObjectFromDef(lastElement));
            }
        }

        //private void UpdateMapObjectSpawns(double gameTime)
        //{
        //    // If no upcoming mapObjects then there's nothing to spawn
        //    if (_upComingmapObjects.Count == 0)
        //    {
        //        return;
        //    }

        //    MapObjectDef lastElement = _upComingmapObjects[_upComingmapObjects.Count - 1];
        //    if (gameTime < lastElement.LaunchTime)
        //    {
        //        _upComingmapObjects.RemoveAt(_upComingmapObjects.Count - 1);
        //        _mapObjects.Add(CreateMapObjectFromDef(lastElement));
        //    }
        //}

        private MapObject CreateMapObjectFromDef(MapObjectDef definition)
        {
            MapObject mapObject = new MapObject(_textureManager, _effectsManager);
            mapObject.SetPosition(definition.SpawnPoint);
            //if (definition.MapObjectType == "skeleton")
            //{
            //    List<Vector> _pathPoints = new List<Vector>();
            //    var prng = new Random();
            //    var ranX = prng.Next(-580, 580);
            //    var ranY = prng.Next(-350, 350);

            //    mapObject.SetPosition(new Vector(ranX, ranY, 0));
            //}

            //if (definition.MapObjectType == "cannon_fodder")
            //{
            //    List<Vector> _pathPoints = new List<Vector>();
            //    _pathPoints.Add(new Vector(1400, 0, 0));
            //    _pathPoints.Add(new Vector(0, 0, 0));
            //    _pathPoints.Add(new Vector(-1400, 0, 0));

            //    mapObject.Path = new Path(_pathPoints, 15);
            //}

            //if (definition.MapObjectType == "cannon_fodder_high")
            //{
            //    List<Vector> _pathPoints = new List<Vector>();
            //    _pathPoints.Add(new Vector(1400, 0, 0));
            //    _pathPoints.Add(new Vector(0, 250, 0));
            //    _pathPoints.Add(new Vector(-1400, 0, 0));

            //    mapObject.Path = new Path(_pathPoints, 15);
            //}

            //else if (definition.MapObjectType == "cannon_fodder_low")
            //{
            //    List<Vector> _pathPoints = new List<Vector>();
            //    _pathPoints.Add(new Vector(1400, 0, 0));
            //    _pathPoints.Add(new Vector(0, -250, 0));
            //    _pathPoints.Add(new Vector(-1400, 0, 0));

            //    mapObject.Path = new Path(_pathPoints, 15);
            //}

            //else if (definition.MapObjectType == "cannon_fodder_straight")
            //{
            //    List<Vector> _pathPoints = new List<Vector>();
            //    _pathPoints.Add(new Vector(1400, 0, 0));
            //    _pathPoints.Add(new Vector(-1400, 0, 0));

            //    mapObject.Path = new Path(_pathPoints, 21);
            //}

            //else if (definition.MapObjectType == "up_l")
            //{
            //    List<Vector> _pathPoints = new List<Vector>();
            //    _pathPoints.Add(new Vector(500, -375, 0));
            //    _pathPoints.Add(new Vector(500, 0, 0));
            //    _pathPoints.Add(new Vector(500, 0, 0));
            //    _pathPoints.Add(new Vector(-1400, 200, 0));

            //    mapObject.Path = new Path(_pathPoints, 15);
            //}

            //else if (definition.MapObjectType == "down_l")
            //{
            //    List<Vector> _pathPoints = new List<Vector>();
            //    _pathPoints.Add(new Vector(500, 375, 0));
            //    _pathPoints.Add(new Vector(500, 0, 0));
            //    _pathPoints.Add(new Vector(500, 0, 0));
            //    _pathPoints.Add(new Vector(-1400, -200, 0));

            //    mapObject.Path = new Path(_pathPoints, 15);
            //}

            //else
            //{
            //    System.Diagnostics.Debug.Assert(false, "Unknown mapObject type.");
            //}
            return mapObject;
        }

        public void Update(double elapsedTime, double gameTime)
        {
            UpdateMapObjectSpawns(gameTime);
            _mapObjects.ForEach(x => x.Update(elapsedTime));
            //CheckForOutOfBounds();
            //RemoveDeadMapObjects();
        }

        //private void CheckForOutOfBounds()
        //{
        //    foreach (MapObject mapObject in _mapObjects)
        //    {
        //        if (mapObject.IsPathDone())
        //        //|| mapObject.GetBoundingBox().Left > _clientBounds.EastBound
        //        //|| mapObject.GetBoundingBox().Top < _clientBounds.NorthBound
        //        //|| mapObject.GetBoundingBox().Bottom > _clientBounds.SouthBound)
        //        {
        //            mapObject.Health = 0;
        //        }
        //    }
        //}

        public void Render(Renderer renderer)
        {
            _mapObjects.ForEach(x => x.Render(renderer));
        }

        //private void RemoveDeadMapObjects()
        //{
        //    for (int i = _mapObjects.Count - 1; i >= 0; i--)
        //    {
        //        if (_mapObjects[i].IsDead)
        //        {
        //            _mapObjects.RemoveAt(i);
        //        }
        //    }
        //}
    }
}