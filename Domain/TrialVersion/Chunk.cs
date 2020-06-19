using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class Chunk<T> : IChunk<T>
    {
        public const int XLength = 16;
        public const int YLength = 256;
        public const int ZLength = 16;
        public PointI Position { get; }

        private readonly T[,,] blocks;

        public Chunk(PointI position)
        {
            Position = position;
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

        public static bool CheckBounds(PointB position)
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