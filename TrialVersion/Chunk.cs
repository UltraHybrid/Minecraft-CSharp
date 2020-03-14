using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tmp.TrialVersion
{
    public class Chunk : IEnumerable<Block>
    {
        private readonly Block[,,] blocks;
        private const int xLenght = 16;
        private const int yLength = 256;
        private const int zLength = 16;

        public Chunk()
        {
            blocks = new Block[xLenght, yLength, zLength];
        }

        public void FillBlocks(BlockItem blockItem)
        {
            for (var i = 0; i < xLenght; i++)
            for (var j = 0; j < yLength; j++)
            for (var k = 0; k < zLength; k++)
                blocks[i, j, k] = new Block(blockItem);
        }

        public IEnumerable<Block> GetVisibleBlocks()
        {
            for (var i = 0; i < xLenght; i++)
            for (var j = 0; j < yLength; j++)
            for (var k = 0; k < zLength; k++)
            {
                var block = blocks[i, j, k];
                for (var x = 0; x < 3; x++)
                for (var y = 0; y < 3; y++)
                for (var z = 0; z < 3; z++)
                    if (x != 1 && y != 1 && z != 1 && CheckBounds(i + x, j + y, k + z) &&
                        block.BlockItem != BaseBlocks.Empty)
                        yield return block;
            }
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return blocks.Cast<Block>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool CheckBounds(int x, int y, int z)
        {
            return 0 <= x && x < xLenght && 0 <= y && y < yLength && 0 <= z && z < zLength;
        }

        public Block this[int x, int y, int z]
        {
            get
            {
                if (!CheckBounds(x, y, z))
                    throw new ArgumentException();
                return blocks[x, y, z];
            }

            set
            {
                if (!CheckBounds(x, y, z))
                    throw new ArgumentException();
                blocks[x, y, z] = value;
            }
        }
    }
}