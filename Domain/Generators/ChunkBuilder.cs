﻿using System.Collections.Generic;
using System.Linq;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public class ChunkBuilder
    {
        // ReSharper disable once InconsistentNaming
        private readonly IGenerator<PointI, Chunk<Block>> startGenerator;
        // ReSharper disable once InconsistentNaming
        private readonly List<IGenerator<Chunk<Block>, Chunk<Block>>> generators;

        public ChunkBuilder(IGenerator<PointI, Chunk<Block>> generator)
        {
            startGenerator = generator;
            generators = new List<IGenerator<Chunk<Block>, Chunk<Block>>>();
        }

        public ChunkBuilder UseNext(IGenerator<Chunk<Block>, Chunk<Block>> generator)
        {
            generators.Add(generator);
            return this;
        }

        public Chunk<Block> Compile(PointI position)
        {
            var chunk = startGenerator.Generate(position);
            return generators.Aggregate(chunk, (current, generator) => generator.Generate(current));
        }
    }
}