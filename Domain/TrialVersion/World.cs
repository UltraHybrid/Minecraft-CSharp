using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class World : IEnumerable<Chunk>
    {
        private readonly Chunk[,] chunks;
        public readonly int Size;

        public World(IGenerator<int, Chunk> generator, int worldSize)
        {
            Size = worldSize;
            chunks = new Chunk[Size, Size];
            for (var x = 0; x < Size; x++)
            for (var z = 0; z < Size; z++)
            {
                var chunk = generator.Generate(x, z);
                chunk.Position = new PointI(x, 0, z);
                chunks[x, z] = chunk;
            }
        }

        public PointI GetAbsolutPosition(Block block, int x, int z)
        {
            return new PointI(x * Chunk.XLenght, 0, z * Chunk.ZLength).Add(block.Position);
        }

        public bool IsCorrectIndex(PointI position)
        {
            return 0 <= position.X && position.X < Size * Chunk.XLenght && 0 <= position.Y &&
                   position.Y < Chunk.YLength && 0 <= position.Z && position.Z < Size * Chunk.ZLength;
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
            var numberX = position.X / Chunk.XLenght;
            var numberZ = position.Z / Chunk.ZLength;
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
                    throw new ArgumentException();
                var numberX = position.X / Chunk.XLenght;
                var numberZ = position.Z / Chunk.ZLength;
                var chunckX = position.X % Chunk.XLenght;
                var chunckZ = position.Z % Chunk.ZLength;
                return chunks[numberX, numberZ][(PointB) new PointI(chunckX, position.Y, chunckZ)];
            }
            set
            {
                if (!CheckChunckBounds(position))
                    throw new ArgumentException();
                var numberX = position.X / Chunk.XLenght;
                var numberZ = position.Z / Chunk.ZLength;
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