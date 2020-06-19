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
        private readonly IGenerator<Chunk<Block>, Chunk<Block>> bedrockGenerator;

        public WorldGenerator(IGenerator<PointI, Chunk<Block>> landGenerator,
            IGenerator<Chunk<Block>, Chunk<Block>> oreGenerator,
            IGenerator<Chunk<Block>, Chunk<Block>> treeGenerator,
            IGenerator<Chunk<Block>, Chunk<Block>> bedrockGenerator)
        {
            this.landGenerator = landGenerator;
            this.oreGenerator = oreGenerator;
            this.treeGenerator = treeGenerator;
            this.bedrockGenerator = bedrockGenerator;
        }

        public Chunk<Block> Generate(PointI source)
        {
            return new ChunkBuilder(landGenerator)
                .UseNext(oreGenerator)
                .UseNext(treeGenerator)
                .UseNext(bedrockGenerator)
                .Compile(source);
        }
    }
}