using System;
using System.Collections;
using System.Collections.Generic;
using OpenTK;

namespace tmp
{
    public class World : IEnumerable<Chunk>
    {
        private readonly Chunk[,] chunks;
        public const int MaxCount = 15;

        public World(IGenerator generator)
        {
            chunks = new Chunk[MaxCount, MaxCount];
            for (var x = 0; x < MaxCount; x++)
            for (var z = 0; z < MaxCount; z++)
            {
                var chunk = generator.Generate(x, z);
                chunk.Position = new Point3(x, 0, z);
                chunks[x, z] = chunk;
            }
        }

        public Dictionary<string[], List<Vector3>> GetVisibleBlock(int x, int z)
        {
            var chunk = chunks[x, z];
            var result = new Dictionary<string[], List<Vector3>>();
            foreach (var block in chunk)
            {
                var position = GetAbsolutPosition(block, x, z);
                if (block.BlockType == BaseBlocks.Empty || !IsBorderOnEmpty(block, position))
                    continue;

                var key = block.BlockType.TextureName;
                if (result.ContainsKey(key))
                    result[key].Add(position);
                else
                    result[key] = new List<Vector3>() {position};
            }

            return result;
        }

        public Dictionary<string[], List<Vector3>> GetVisibleBlock(Chunk chunk)
        {
            return GetVisibleBlock(chunk.Position.X, chunk.Position.Z);
        }

        private Point3 GetAbsolutPosition(Block block, int x, int z)
        {
            return block.Position + new Point3(x * Chunk.XLenght, 0, z * Chunk.ZLength);
        }

        private bool IsBorderOnEmpty(Block block, Point3 position)
        {
            var offsets = new Point3[]
            {
                new Point3(-1, 0, 0), new Point3(1, 0, 0),
                new Point3(0, -1, 0), new Point3(0, 1, 0),
                new Point3(0, 0, -1), new Point3(0, 0, 1)
            };
            for (var i = 0; i < offsets.Length; i++)
            {
                if (!IsCorrectIndex(position + offsets[i]))
                    return true;
                if (this[position + offsets[i]].BlockType == BaseBlocks.Empty)
                    return true;
            }

            return false;
        }

        private bool IsCorrectIndex(Point3 position)
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

        private bool CheckChunckBounds(Point3 position)
        {
            var numberX = position.X / Chunk.XLenght;
            var numberZ = position.Z / Chunk.ZLength;
            return CheckChunckBounds(numberX, numberZ);
        }

        private bool CheckChunckBounds(int x, int z)
        {
            return 0 <= x && x < MaxCount && 0 <= z && z < MaxCount;
        }

        public Block this[Point3 position]
        {
            get
            {
                if (!CheckChunckBounds(position))
                    throw new ArgumentException();
                var numberX = position.X / Chunk.XLenght;
                var numberZ = position.Z / Chunk.ZLength;
                var chunckX = position.X % Chunk.XLenght;
                var chunckZ = position.Z % Chunk.ZLength;
                return chunks[numberX, numberZ][new Point3(chunckX, position.Y, chunckZ)];
            }
            set
            {
                if (!CheckChunckBounds(position))
                    throw new ArgumentException();
                var numberX = position.X / Chunk.XLenght;
                var numberZ = position.Z / Chunk.ZLength;
                var chunckX = position.X % Chunk.XLenght;
                var chunckZ = position.Z % Chunk.ZLength;
                chunks[numberX, numberZ][new Point3(chunckX, position.Y, chunckZ)] = value;
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