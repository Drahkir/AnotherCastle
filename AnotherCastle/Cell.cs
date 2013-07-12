using Engine;

namespace AnotherCastle
{
    public class Cell : Entity
    {
        public double X;
        public double Y;

        public Cell(string cellName, TextureManager textureManager, CellTypes cellType, double x, double y)
        {
            X = x;
            Y = y;
            CellName = cellName;
            CellType = cellType;
            Sprite.SetPosition(x, y);
            Sprite.Texture = textureManager.Get(cellType == CellTypes.RockWall ? "rock_wall" : "dirt_floor");
        }

        public string CellName { get; set; }

        public CellTypes CellType { get; set; }

        // Need a constructor to convert XML (?) data to the cell array
        //public BuildCellArray() {

        //}

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(Sprite);
        }
    }
}
