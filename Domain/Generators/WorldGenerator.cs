﻿using System.Globalization;
using System.Linq;
using Ninject;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
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