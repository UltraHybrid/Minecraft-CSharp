using System;

namespace tmp.TrialVersion
{
    public static class Extensions
    {
        public static Chunk GetTestChunk(this World world)
        {
            var chunk = new Chunk();
            var rnd = new Random();
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
    }
}