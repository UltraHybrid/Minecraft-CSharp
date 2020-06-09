using System;
using tmp.Domain.TrialVersion;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public class RandomGenerator : IGenerator<int, Chunk<Block>>
    {
        public Chunk<Block> Generate(int x, int z)
        {
            var rnd = new Random();
            var chunk = new Chunk<Block>(PointI.CreateXZ(x, z));
            var allBlocks = BaseBlocks.AllBlocks;
            for (byte i = 0; i < Chunk<Block>.XLength; i++)
            for (byte j = 0; j < Chunk<Block>.YLength - 1; j++)
            for (byte k = 0; k < Chunk<Block>.ZLength; k++)
            {
                var position = new PointB(i, j, k);
                var rndValue = rnd.Next(0, allBlocks.Count * 2);
                if (rndValue < allBlocks.Count)
                    chunk[position] = new Block(allBlocks[rndValue], position);
            }

            return chunk;
        }
    }
}