using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class Chunk<T> : IEnumerable<T>
    {
        private readonly T[,,] blocks;
        public PointI Position { get; set; }
        public const int XLength = 16;
        public const int YLength = 256;
        public const int ZLength = 16;

        public Chunk()
        {
            blocks = new T[XLength, YLength, ZLength];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return blocks.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static bool CheckBounds(PointB position)
        {
            return position.X < XLength && position.Y < YLength && position.Z < ZLength;
        }

        public T this[PointB position]
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