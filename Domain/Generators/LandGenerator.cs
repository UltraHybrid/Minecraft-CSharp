using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class LandGenerator : IGenerator<PointI, Chunk<Block>>
    {
        private readonly IGenerator<PointF, float> highGenerator;

        public LandGenerator(IGenerator<PointF, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk<Block> Generate(PointI point)
        {
            var chunk = new Chunk<Block>(point);
            var (xL, zL) = Chunk<Block>.GetSize;
            foreach (var (i, k) in Utils.DualFor(xL, zL))
            {
                var value = (int) (22 * highGenerator.Generate(PointF.CreateXZ(xL * point.X + i, zL * point.Z + k)));
                value += 60;
                chunk[new PointI(i, value, k)] = new Block(BaseBlocks.Grass);
                // for (var y = value - 1; y >= 0; y--)
                //     chunk[new PointI(i, y, k)] = new Block(BaseBlocks.Dirt);
            }

            return chunk;
        }
    }
}