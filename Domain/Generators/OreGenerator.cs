using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class OreGenerator : IGenerator<Chunk<Block>, Chunk<Block>>
    {
        private readonly IGenerator<PointF, float> highGenerator;

        public OreGenerator(IGenerator<PointF, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk<Block> Generate(Chunk<Block> source)
        {
            for (var x = 0; x < Chunk<Block>.XLength; x++)
            {
                for (var z = 0; z < Chunk<Block>.ZLength; z++)
                {
                    for (var y = 0; y < Chunk<Block>.YLength - 3; y++)
                    {
                        var point = new PointI(x, y, z).AsPointB();
                        if (source[point.Add(new PointB(0,3,0))] == null)
                            break;
                        var block = DeterminateOre(point, source.Position);
                        if (block != null)
                            source[point] = block;
                    }
                }
            }

            return source;
        }

        private Block DeterminateOre(PointB point, PointI chunkPosition)
        {
            const float coeff = 0.9f;
            var noiseValue = Get3DNoise(
                point.X * coeff + chunkPosition.X * Chunk<Block>.XLength,
                point.Y * coeff,
                point.Z * coeff + chunkPosition.Z * Chunk<Block>.ZLength);

            if (noiseValue >= 0.7 && noiseValue < 0.72)
                return new Block(BaseBlocks.CoalOre, point);

            if (noiseValue >= 0.01 && noiseValue < 0.015 && point.Y <= 60)
                return new Block(BaseBlocks.GoldOre, point);

            if (noiseValue >= 0.8 && noiseValue < 0.82)
                return new Block(BaseBlocks.IronOre, point);

            if (noiseValue >= -0.307 && noiseValue < -0.3 && point.Y <= 35)
                return new Block(BaseBlocks.DiamondOre, point);
            
            return null;
        }

        private float Get3DNoise(float x, float y, float z)
        {
            var a = highGenerator.Generate(PointF.CreateXZ(x, y));
            var b = highGenerator.Generate(PointF.CreateXZ(y, z));
            var c = highGenerator.Generate(PointF.CreateXZ(z, x));
            return (a + b + c) / 3;
        }
    }
}