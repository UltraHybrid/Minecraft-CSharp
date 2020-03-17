using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace tmp.TrialVersion
{
    public class World : IEnumerable<Chunk>
    {
        private readonly Chunk[,] chunks;
        public const int MaxCount = 5;

        public World()
        {
            chunks = new Chunk[MaxCount, MaxCount];
            for (var x = 0; x < MaxCount; x++)
            for (var z = 0; z < MaxCount; z++)
            {
                var chunk = new Chunk();
                chunk.FillLayers(BaseBlocks.Dirt, 30);
                chunks[x, z] = chunk;
            }
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            return chunks.Cast<Chunk>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool CheckBounds(Point3 position)
        {
            var numberX = position.X / Chunk.XLenght;
            var numberZ = position.Z / Chunk.ZLength;
            return CheckBounds(numberX, numberZ);
        }

        private bool CheckBounds(int x, int z)
        {
            return 0 <= x && x < MaxCount && 0 <= z && z < MaxCount;
        }

        public Block this[Point3 position]
        {
            get
            {
                if (!CheckBounds(position))
                    throw new ArgumentException();
                var numberX = position.X / Chunk.XLenght;
                var numberZ = position.Z / Chunk.ZLength;
                var chunckX = position.X % Chunk.XLenght;
                var chunckZ = position.Z % Chunk.ZLength;
                return chunks[numberX, numberZ][new Point3(chunckX, position.Y, chunckZ)];
            }
            set
            {
                if (!CheckBounds(position))
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
                if (!CheckBounds(x, z))
                    throw new ArgumentException();
                return chunks[x, z];
            }
            set
            {
                if (!CheckBounds(x, z))
                    throw new ArgumentException();
                chunks[x, z] = value;
            }
        }
    }
}