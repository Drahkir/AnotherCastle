using Engine;

namespace AnotherCastle
{
    public interface IEnemyBrain
    {
        Vector NextMove(Vector currentPosition, double elapsedTime);
    }
}