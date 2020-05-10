using System;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class World2
    {
        public PointI GlobalOffset { get; set; }
        public readonly int Size;
        private readonly Dictionary<PointI, Chunk> chunks;
        public int Count => chunks.Count;

        public World2(PointI startOffset, int size)
        {
            Size = size;
            GlobalOffset = startOffset;
            chunks = new Dictionary<PointI, Chunk>();
        }

        public static PointI GetAbsolutePosition(PointB blockPosition, PointI chunkPosition)
        {
            return PointI.CreateXZ(chunkPosition.X * Chunk.XLength, chunkPosition.Z * Chunk.ZLength).Add(blockPosition);
        }

        public (PointI cPosition, PointI bPosition) Translate2LocalNotation(PointI point)
        {
            var blockX = point.X % Chunk.XLength;
            var blockZ = point.Z % Chunk.ZLength;
            var chunkX = point.X / Chunk.XLength;
            var chunkZ = point.Z / Chunk.ZLength;
            return (new PointI(chunkX, 0, chunkZ), new PointI(blockX, point.Y, blockZ));
        }

        public bool TryDeleteChunk(PointI point)
        {
            return chunks.Remove(point);
        }

        public bool IsChunkInBounds(PointI point)
        {
            return point.X >= GlobalOffset.X && point.X < GlobalOffset.X + Size &&
                   point.Z >= GlobalOffset.Z && point.Z < GlobalOffset.Z + Size;
        }

        public IReadOnlyList<PointI> GetNeedLessChunks()
        {
            return chunks.Keys.Where(k => !IsChunkInBounds(k)).ToList();
        }

        public List<PointI> GetPointOfGaps()
        {
            var result = new List<PointI>();
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    var point = GlobalOffset.Add(PointI.CreateXZ(i, j));
                    if (!chunks.ContainsKey(point))
                        result.Add(point);
                }
            }

            return result;
        }

        public bool ContainsChunk(PointI point)
        {
            return chunks.ContainsKey(point);
        }

        public Block GetBlock(PointI point)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(point);
            if (chunks.ContainsKey(cPosition))
                return chunks[cPosition][(PointB) bPosition];
            return null;
        }

        public void SetBlock(PointI point, Block value)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(point);
            if (chunks.ContainsKey(cPosition))
                chunks[cPosition][(PointB) bPosition] = value;
        }

        public Chunk this[PointI point]
        {
            get => chunks.ContainsKey(point) ? chunks[point] : null;
            set => chunks[point] = value;
        }
    }
}