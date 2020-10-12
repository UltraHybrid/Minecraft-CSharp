using System.Numerics;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.TrialVersion.Blocks
{
    public class Block
    {
        public static readonly Block Either = new Block(BaseBlocks.Corpuscle);
        public readonly BlockType BlockType;
        public int Hardness { get; set; }

        public Block(BlockType blockType)
        {
            BlockType = blockType;
            Hardness = blockType.Hardness;
        }

        public bool CanRemove()
        {
            return Hardness <= 0;
        }

        public static Geometry GetGeometry(BlockType blockType, PointL position)
        {
            return Geometry.Identity(blockType.Form.Shift(position.AsVector()));
        }
    }
}