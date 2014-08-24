using Engine;

namespace AnotherCastle
{
    public class EnemyMissile : Missile
    {
        public EnemyMissile(Texture texture, Vector vector) : base(texture, vector)
        {
        }

        public override void OnCollision(IEntity entity, Vector amount)
        {
            if (entity.GetType() == typeof (Eyeball))
            {
                return;
            }

            Dead = true;
            //Sprite.SetColor(new Color(0, 0, 1, 1));
        }
    }
}