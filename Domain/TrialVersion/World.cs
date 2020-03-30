using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class World : IEnumerable<Chunk>
    {
        private readonly Chunk[,] chunks;
        public const int MaxCount = 22;

        public World(IGenerator<Chunk> generator)
        {
            chunks = new Chunk[MaxCount, MaxCount];
            for (var x = 0; x < MaxCount; x++)
            for (var z = 0; z < MaxCount; z++)
            {
                var chunk = generator.Generate(x, z);
                chunk.Position = new PointI(x, 0, z);
                chunks[x, z] = chunk;
            }
        }

        public ILookup<string[], PointI> GetVisibleBlock(int x, int z)
        {
            var chunk = chunks[x, z];
            var result = chunk
                .Where(b => b != null)
                .Where(b => IsBorderOnEmpty(GetAbsolutPosition(b, x, z)))
                .ToLookup(block => block.BlockType.TextureName,
                    block => GetAbsolutPosition(block, x, z));
            //Console.WriteLine(result.Count);
            return result;
        }

        public ILookup<string[], PointI> GetVisibleBlock(Chunk chunk)
        {
            return GetVisibleBlock(chunk.Position.X, chunk.Position.Z);
        }

        private PointI GetAbsolutPosition(Block block, int x, int z)
        {
            return new PointI(x * Chunk.XLenght, 0, z * Chunk.ZLength).Add(block.Position);
        }

        private bool IsBorderOnEmpty(PointI position)
        {
            var offsets = new PointI[]
            {
                new PointI(-1, 0, 0), new PointI(1, 0, 0),
                new PointI(0, -1, 0), new PointI(0, 1, 0),
                new PointI(0, 0, -1), new PointI(0, 0, 1)
            };
            for (var i = 0; i < offsets.Length; i++)
            {
                if (!IsCorrectIndex(position.Add(offsets[i])))
                    return true;
                if (this[position.Add(offsets[i])] == null)
                    return true;
            }

            return false;
        }

        private bool IsCorrectIndex(PointI position)
        {
            return 0 <= position.X && position.X < MaxCount * Chunk.XLenght && 0 <= position.Y &&
                   position.Y < Chunk.YLength && 0 <= position.Z && position.Z < MaxCount * Chunk.ZLength;
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            for (var x = 0; x < MaxCount; x++)
            for (var z = 0; z < MaxCount; z++)
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
            return 0 <= x && x < MaxCount && 0 <= z && z < MaxCount;
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