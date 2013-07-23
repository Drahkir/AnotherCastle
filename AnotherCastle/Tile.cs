using System.Windows.Forms;
using Engine;

namespace AnotherCastle
{
    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,
    }

    public class Tile : Entity
    {
        public double X;
        public double Y;
        public static int Width = 85;
        public static int Height = 85;

        public Tile(TileCollision tileCollision)
        {
            TileCollision = tileCollision;
        }

        public Tile(Texture texture, TileCollision tileCollision)
        {
            Sprite.Texture = texture;
            TileCollision = tileCollision;
        }

        public Tile(string tileName, Texture texture, TileCollision tileCollision, Vector position)
        {
            TileName = tileName;
            TileCollision = tileCollision;
            Sprite.Texture = texture;
            Sprite.SetPosition(position);
        }
        
        public Tile(string tileName, Texture texture, TileCollision tileCollision, double x, double y)
        {
            X = x;
            Y = y;
            TileName = tileName;
            TileCollision = tileCollision;
            Sprite.SetPosition(x, y);
            Sprite.Texture = texture;
        }

        public void SetPosition(double x, double y)
        {
            X = x;
            Y = y;
            Sprite.SetPosition(X, Y);
        }
        public string TileName { get; set; }

        public TileCollision TileCollision { get; set; }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
        }
    }
}
