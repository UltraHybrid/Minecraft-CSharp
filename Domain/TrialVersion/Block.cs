namespace tmp
{
    public class Block
    {
        public readonly BlockType BlockType;
        public int Hardness { get; set; }
        public PointB Position { get; set; }

        private Block(BlockType blockType)
        {
            BlockType = blockType;
            Hardness = blockType.Hardness;
            Position = PointB.Default;
        }

        public Block(BlockType blockType, PointB position) : this(blockType)
        {
            Position = position;
        }

        public bool CanRemove()
        {
            return Hardness <= 0;
        }
    }
}