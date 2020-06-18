using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public class WorldGenerator : IGenerator<PointI, Chunk<Block>>
    {
        private readonly IGenerator<PointI, Chunk<Block>> landGenerator;
        private readonly IGenerator<Chunk<Block>, Chunk<Block>> oreGenerator;
        private readonly IGenerator<Chunk<Block>, Chunk<Block>> treeGenerator;

        public WorldGenerator(IGenerator<PointI, Chunk<Block>> landGenerator,
            IGenerator<Chunk<Block>, Chunk<Block>> oreGenerator,
            IGenerator<Chunk<Block>, Chunk<Block>> treeGenerator)
        {
            this.landGenerator = landGenerator;
            this.oreGenerator = oreGenerator;
            this.treeGenerator = treeGenerator;
        }

        public Chunk<Block> Generate(PointI source)
        {
            var landChunk = landGenerator.Generate(source);
            var oreChunk = oreGenerator.Generate(landChunk);
            var treeChunk = treeGenerator.Generate(oreChunk);
            return treeChunk;
        }
    }
}