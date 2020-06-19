using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.TrialVersion.Blocks
{
    public class Block
    {
        public static readonly Block Either = new Block(BaseBlocks.Corpuscle, PointB.Zero);
        public readonly BlockType BlockType;
        public int Hardness { get; set; }
        public PointB Position { get; set; }

        public Block(BlockType blockType, PointB position)
        {
            BlockType = blockType;
            Hardness = blockType.Hardness;
            Position = position;
        }

        public Geometry GetHitBox()
        {
            var offset = BlockType.Form.Shift(Position.AsPointI().AsVector());
            return  Geometry.Identity(offset.Shift(BlockType.Form.I / 2 + BlockType.Form.K / 2));
        }

        public bool CanRemove()
        {
            return Hardness <= 0;
        }
    }
}