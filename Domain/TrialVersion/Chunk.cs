using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class Chunk : IEnumerable<Block>
    {
        private readonly Block[,,] blocks;
        public PointI Position { get; set; }
        public const int XLength = 16;
        public const int YLength = 256;
        public const int ZLength = 16;

        public Chunk()
        {
            blocks = new Block[XLength, YLength, ZLength];
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return blocks.Cast<Block>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static bool CheckBounds(PointB position)
        {
            return position.X < XLength && position.Y < YLength && position.Z < ZLength;
        }

        public Block this[PointB position]
        {
            get
            {
                if (!CheckBounds(position))
                    throw new ArgumentException("Значение не попадает в диапазон чанка: " + position);
                return blocks[position.X, position.Y, position.Z];
            }

            set
            {
                if (!CheckBounds(position))
                    throw new ArgumentException("Значение не попадает в диапазон чанка: " + position);
                blocks[position.X, position.Y, position.Z] = value;
            }
        }
    }
}