using System.Linq;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class WorldGenerator : IGenerator<PointI, Chunk<Block>>
    {
        private readonly IGenerator<PointI, Chunk<Block>> baseGenerator;
        private readonly IGenerator<Chunk<Block>, Chunk<Block>>[] additionalGenerators;

        public WorldGenerator(IGenerator<PointI, Chunk<Block>> baseGenerator,
            IGenerator<Chunk<Block>, Chunk<Block>>[] additionalGenerators)
        {
            this.baseGenerator = baseGenerator;
            this.additionalGenerators = additionalGenerators;
        }

        public Chunk<Block> Generate(PointI source)
        {
            return additionalGenerators
                .Aggregate(new ChunkBuilder(baseGenerator), (current, g) => current.UseNext(g))
                .Compile(source);
        }
    }
}