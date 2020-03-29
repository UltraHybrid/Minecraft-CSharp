namespace tmp
{
    public class Block
    {
        public readonly BlockType BlockType;
        public int Hardness { get; set; }
        public Point3 Position { get; set; }

        public Block(BlockType blockType)
        {
            BlockType = blockType;
            Hardness = blockType.Hardness;
            Position = Point3.Default;
        }

        public Block(BlockType blockType, Point3 position) : this(blockType)
        {
            Position = position;
        }

        public bool CanRemove()
        {
            return Hardness <= 0;
        }
    }
}