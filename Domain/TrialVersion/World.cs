using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace tmp
{
    public class World : IEnumerable<Chunk>
    {
        private readonly Chunk[,] chunks;
        public const int MaxCount = 3;

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

        public IEnumerable<(string[], PointI)> GetVisibleBlock(int x, int z)
        {
            var chunk = chunks[x, z];

            return chunk
                .Where(b => b != null)
                .Select(block => GetAbsolutPosition(block, x, z))
                .Select(position => (IsBorderOnEmpty(position), position))
                .Where(p => p.Item1 != null)
                .ToList();
        }

        public IEnumerable<(string[], PointI)> GetVisibleBlock(Chunk chunk)
        {
            return GetVisibleBlock(chunk.Position.X, chunk.Position.Z);
        }

        private PointI GetAbsolutPosition(Block block, int x, int z)
        {
            return new PointI(x * Chunk.XLenght, 0, z * Chunk.ZLength).Add(block.Position);
        }

        private string[] IsBorderOnEmpty(PointI position)
        {
            var offsets = new[]
            {
                new PointI(0, 0, -1),
                new PointI(1, 0, 0),
                new PointI(0, 0, 1),
                new PointI(0, 1, 0),
                new PointI(-1, 0, 0),
                new PointI(0, -1, 0)
            };
            var result = new string[6];
            var textures = this[position].BlockType.TextureName;
            var flag = false;
            for (var i = 0; i < offsets.Length; i++)
            {
                if (IsCorrectIndex(position.Add(offsets[i])) && this[position.Add(offsets[i])] == null)
                {
                    result[i] = textures[i];
                    flag = true;
                }
                else
                {
                    result[i] = null;
                }
            }

            return flag ? result : null;
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