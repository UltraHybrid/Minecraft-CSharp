using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace tmp.TrialVersion
{
    public class Chunk : IEnumerable<Block>
    {
        private readonly Block[,,] blocks;
        public const int XLenght = 16;
        public const int YLength = 256;
        public const int ZLength = 16;

        public Chunk()
        {
            blocks = new Block[XLenght, YLength, ZLength];
            FillLayers(BaseBlocks.Empty, YLength);
        }

        public void FillLayers(BlockType blockType, int count)
        {
            if (count < 0 || count > YLength)
                throw new ArgumentException();
            for (var j = 0; j < count; j++)
            for (var i = 0; i < XLenght; i++)
            for (var k = 0; k < ZLength; k++)
                blocks[i, j, k] = new Block(blockType, new Point3(i, j, k));
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return blocks.Cast<Block>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool CheckBounds(Point3 position)
        {
            return 0 <= position.X && position.X < XLenght
                                   && 0 <= position.Y && position.Y < YLength
                                   && 0 <= position.Z && position.Z < ZLength;
        }

        public Block this[Point3 position]
        {
            get
            {
                if (!CheckBounds(position))
                    throw new ArgumentException();
                return blocks[position.X, position.Y, position.Z];
            }

            set
            {
                if (!CheckBounds(position))
                    throw new ArgumentException();
                blocks[position.X, position.Y, position.Z] = value;
            }
        }
    }
}