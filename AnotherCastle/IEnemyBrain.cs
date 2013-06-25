using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Engine;

namespace AnotherCastle
{
    public interface IEnemyBrain
    {
        Vector NextMove(Vector currentPosition, double elapsedTime);
    }
}
