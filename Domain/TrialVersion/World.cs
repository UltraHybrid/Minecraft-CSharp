using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class World : IEnumerable<Chunk>
    {
        public PointI globalOffset { get; set; }
        private readonly Chunk[,] chunks;
        public readonly int Size;

        public World(int worldSize, PointI startOffset)
        {
            Size = worldSize;
            globalOffset = startOffset;
            chunks = new Chunk[Size, Size];
        }

        public PointI GetAbsolutePosition(Block block, PointI chunkPosition)
        {
            return new PointI(chunkPosition.X * Chunk.XLength, 0, chunkPosition.Z * Chunk.ZLength).Add(block.Position);
        }

        public bool IsCorrectIndex(PointI position)
        {
            return globalOffset.X * Chunk.XLength <= position.X
                   && position.X < (globalOffset.X + Size) * Chunk.XLength
                   && 0 <= position.Y && position.Y < Chunk.YLength
                   && globalOffset.Z * Chunk.ZLength <= position.Z
                   && position.Z < (globalOffset.Z + Size) * Chunk.ZLength;
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

        private bool CheckChunkBounds(PointI position)
        {
            var numberX = position.X / Chunk.XLength - globalOffset.X;
            var numberZ = position.Z / Chunk.ZLength - globalOffset.Z;
            return CheckChunkBounds(numberX, numberZ);
        }

        private bool CheckChunkBounds(int x, int z)
        {
            return 0 <= x && x < Size && 0 <= z && z < Size;
        }

        public PointI Convert2ChunkPoint(PointI point)
        {
            return new PointI(point.X / Chunk.XLength, 0, point.Z / Chunk.ZLength);
        }

        public Block this[PointI position]
        {
            get
            {
                if (!CheckChunkBounds(position))
                    throw new ArgumentException("Was get PointI " + position);
                //Console.WriteLine(position);
                var numberX = position.X / Chunk.XLength - globalOffset.X;
                var numberZ = position.Z / Chunk.ZLength - globalOffset.Z;
                var chunckX = position.X % Chunk.XLength;
                var chunckZ = position.Z % Chunk.ZLength;
                //if (chunks[numberX, numberZ] == null)
                 //   return new Block(BaseBlocks.Bedrock, (PointB) new PointI(chunckX, position.Y, chunckZ));
                //Console.WriteLine(numberX + " " + numberZ + " " + chunckX + " " + chunckZ);
                return chunks[numberX, numberZ][(PointB) new PointI(chunckX, position.Y, chunckZ)];
            }
            set
            {
                if (!CheckChunkBounds(position))
                    throw new ArgumentException();
                var numberX = position.X / Chunk.XLength - globalOffset.X;
                var numberZ = position.Z / Chunk.ZLength - globalOffset.Z;
                var chunkX = position.X % Chunk.XLength;
                var chunkZ = position.Z % Chunk.ZLength;
                chunks[numberX, numberZ][(PointB) new PointI(chunkX, position.Y, chunkZ)] = value;
            }
        }

        public Chunk this[int x, int z]
        {
            get
            {
                if (!CheckChunkBounds(x, z))
                    throw new ArgumentException();
                return chunks[x, z];
            }
            set
            {
                if (!CheckChunkBounds(x, z))
                    throw new ArgumentException();
                chunks[x, z] = value;
            }
        }
    }
}