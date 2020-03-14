namespace tmp.TrialVersion
{
    public class Block
    {
        public readonly BlockItem BlockItem;
        public int Hardness { get; set; }
        public Point3 Position { get; set; }

        public Block(BlockItem blockItem)
        {
            BlockItem = blockItem;
            Hardness = blockItem.Hardness;
            Position = Point3.Default;
        }

        public Block(BlockItem blockItem, Point3 position) : this(blockItem)
        {
            Position = position;
        }

        public bool CanRemove()
        {
            return Hardness <= 0;
        }
    }
}