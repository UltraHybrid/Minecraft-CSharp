using System;

namespace tmp
{
    public class RandomGenerator: IGenerator
    {
        private readonly Random rnd = new Random(); 
        public Chunk Generate(int x, int z)
        {
            var chunk = new Chunk();
            var allBlocks = BaseBlocks.AllBlocks;
            for (var i = 0; i < Chunk.XLenght; i++)
            for (var j = 0; j < Chunk.YLength; j++)
            for (var k = 0; k < Chunk.ZLength; k++)
            {
                var position = new Point3(i, j, k);
                var rndValue = rnd.Next(allBlocks.Count * 2);
                var blockItem = rndValue < allBlocks.Count ? allBlocks[rndValue] : BaseBlocks.Empty;
                chunk[position] = new Block(blockItem, position);
            }

            return chunk;
        }

        public Point3 GetHigh(Point3 point)
        {
            throw new System.NotImplementedException();
        }
    }
}