﻿using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
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
            for (byte i = 0; i < Chunk<Block>.XLength; i++)
            {
                for (byte k = 0; k < Chunk<Block>.ZLength; k++)
                {
                    var innerPoint = PointF.CreateXZ(point.X * Chunk<Block>.XLength + i,
                        point.Z * Chunk<Block>.ZLength + k);
                    var value = (int) (highGenerator.Generate(innerPoint) * 22f + 100);
                    var acme = new PointB(i, (byte) value, k);
                    chunk[acme] = new Block(BaseBlocks.Grass, acme);
                    for (var y = value - 1; y >= 0; y--)
                    {
                        var position = new PointB(i, (byte) y, k);
                        chunk[position] = new Block(BaseBlocks.Dirt, position);
                    }
                }
            }

            return chunk;
        }
    }
}