using System;

namespace tmp
{
    public class RandomGenerator : IGenerator<int, Chunk>
    {
        private readonly Random rnd = new Random();

        public Chunk Generate(int x, int z)
        {
            var chunk = new Chunk();
            var allBlocks = BaseBlocks.AllBlocks;
            for (byte i = 0; i < Chunk.XLenght; i++)
            for (byte j = 0; j < Chunk.YLength - 1; j++)
            for (byte k = 0; k < Chunk.ZLength; k++)
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