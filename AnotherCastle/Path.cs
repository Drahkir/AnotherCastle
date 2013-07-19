using System.Collections.Generic;
using Engine;

namespace AnotherCastle
{
    public class Path
    {
        readonly Spline _spline = new Spline();
        readonly Tween _tween;

        public Path(IEnumerable<Vector> points, double travelTime)
        {
            foreach (Vector v in points)
            {
                _spline.AddPoint(v);
            }
            _tween = new Tween(0, 1, travelTime);
        }

        public void UpdatePosition(double elapsedTime, Enemy enemy)
        {
            _tween.Update(elapsedTime);
            Vector position = _spline.GetPositionOnLine(_tween.Value());
            enemy.SetPosition(position);
        }
    }
}
