namespace tmp.TrialVersion
{
    public class Block
    {
        public readonly BlockItem BlockItem;
        public int Hardness { get; set; }

        public Block(BlockItem blockItem)
        {    
            BlockItem = blockItem;
            Hardness = blockItem.Hardness;
        }
    }
}