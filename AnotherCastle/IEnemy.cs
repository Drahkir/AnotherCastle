using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;

namespace AnotherCastle
{
    public interface IEnemy
    {
        int Health { get; set; }
        int Damage { get; set; }
        bool IsDead { get; set; }

        bool IsPathDone();
        void Update(double elapsedTime);
        void Render(Renderer renderer);
    }
}
