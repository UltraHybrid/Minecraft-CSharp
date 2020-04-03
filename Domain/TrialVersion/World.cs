using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;

namespace tmp
{
    public class World : IEnumerable<Chunk>
    {
        private readonly Chunk[,] chunks;
        public readonly int Size;

        public World(IGenerator<Chunk> generator, int worldSize)
        {
            if (worldSize > 40)
                throw new PingException("Ну куда столько то!?");
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

        public ILookup<string[], (bool[], PointI)> GetVisibleBlock(int x, int z)
        {
            var chunk = chunks[x, z];
            return chunk
                .Where(b => b != null)
                .Select(b => (b, GetAbsolutPosition(b, x, z)))
                .Select(p => (p.Item1, IsBorderOnEmpty(p.Item2), p.Item2))
                .Where(p => p.Item2.Any(x => x))
                .ToLookup(
                    k => k.Item1.BlockType.TextureName,
                    v => (v.Item2, v.Item3)
                );
        }

        public ILookup<string[], (bool[], PointI)> GetVisibleBlock(Chunk chunk)
        {
            return GetVisibleBlock(chunk.Position.X, chunk.Position.Z);
        }

        private PointI GetAbsolutPosition(Block block, int x, int z)
        {
            return new PointI(x * Chunk.XLenght, 0, z * Chunk.ZLength).Add(block.Position);
        }

        private bool[] IsBorderOnEmpty(PointI position)
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
            var result = new bool[6];
            for (var i = 0; i < offsets.Length; i++)
            {
                result[i] = (IsCorrectIndex(position.Add(offsets[i])) && this[position.Add(offsets[i])] == null);
            }

            return result;
        }

        private bool IsCorrectIndex(PointI position)
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