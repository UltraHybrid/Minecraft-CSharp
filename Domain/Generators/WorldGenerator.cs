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
            var chunk = baseGenerator.Generate(source);
            if (additionalGenerators == null) return chunk;
            foreach (var g in additionalGenerators)
                chunk = g.Generate(chunk);

            return chunk;
        }
    }
}