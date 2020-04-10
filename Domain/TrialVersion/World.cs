using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class World : IEnumerable<Chunk>
    {
        public PointI gloabalOffset;
        private readonly Chunk[,] chunks;
        public readonly int Size;

        public World(int worldSize, PointI startOffset)
        {
            Size = worldSize;
            gloabalOffset = startOffset;
            chunks = new Chunk[Size, Size];
        }

        public PointI GetAbsolutPosition(Block block, PointI chunkPosition)
        {
            return new PointI(chunkPosition.X * Chunk.XLenght, 0, chunkPosition.Z * Chunk.ZLength).Add(block.Position);
        }

        public bool IsCorrectIndex(PointI position)
        {
            return gloabalOffset.X * Chunk.XLenght <= position.X
                   && position.X < (gloabalOffset.X + Size) * Chunk.XLenght
                   && 0 <= position.Y && position.Y < Chunk.YLength
                   && gloabalOffset.Z * Chunk.ZLength <= position.Z
                   && position.Z < (gloabalOffset.Z + Size) * Chunk.ZLength;
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            for (var x = 0; x < Size; x++)
            for (var z = 0; z < Size; z++)
                yield return chunks[x, z];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool CheckChunckBounds(PointI position)
        {
            var numberX = position.X / Chunk.XLenght - gloabalOffset.X;
            var numberZ = position.Z / Chunk.ZLength - gloabalOffset.Z;
            return CheckChunckBounds(numberX, numberZ);
        }

        private bool CheckChunckBounds(int x, int z)
        {
            return 0 <= x && x < Size && 0 <= z && z < Size;
        }

        public Block this[PointI position]
        {
            get
            {
                if (!CheckChunckBounds(position))
                    throw new ArgumentException("Was get PointI " + position);
                var numberX = position.X / Chunk.XLenght - gloabalOffset.X;
                var numberZ = position.Z / Chunk.ZLength - gloabalOffset.Z;
                var chunckX = position.X % Chunk.XLenght;
                var chunckZ = position.Z % Chunk.ZLength;
                return chunks[numberX, numberZ][(PointB) new PointI(chunckX, position.Y, chunckZ)];
            }
            set
            {
                if (!CheckChunckBounds(position))
                    throw new ArgumentException();
                var numberX = position.X / Chunk.XLenght - gloabalOffset.X;
                var numberZ = position.Z / Chunk.ZLength - gloabalOffset.Z;
                var chunckX = position.X % Chunk.XLenght;
                var chunckZ = position.Z % Chunk.ZLength;
                chunks[numberX, numberZ][(PointB) new PointI(chunckX, position.Y, chunckZ)] = value;
            }
        }

        public Chunk this[int x, int z]
        {
            get
            {
                if (!CheckChunckBounds(x, z))
                    throw new ArgumentException();
                return chunks[x, z];
            }
            set
            {
                if (!CheckChunckBounds(x, z))
                    throw new ArgumentException();
                chunks[x, z] = value;
            }
        }
    }
}