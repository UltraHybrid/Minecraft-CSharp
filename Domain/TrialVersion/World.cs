using System.Collections.Generic;
using System.Linq;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public abstract class World<TContainer,TItem> : IWorld<TContainer, TItem> where TContainer: Chunk<TItem>
    {
        public PointI Offset { get; set; }
        public int Size { get; }
        public int Count => chunks.Count;

        protected readonly Dictionary<PointI, TContainer> chunks;

        protected World(PointI startOffset, int size)
        {
            Size = size;
            Offset = startOffset;
            chunks = new Dictionary<PointI, TContainer>();
        }

        public static PointI GetAbsolutePosition(PointB blockPosition, PointI chunkPosition)
        {
            return PointI.CreateXZ(chunkPosition.X * Chunk<TItem>.XLength, chunkPosition.Z * Chunk<TItem>.ZLength)
                .Add(blockPosition.AsPointI());
        }

        public (PointI cPosition, PointI elementPosition) Translate2LocalNotation(PointI point)
        {
            var elementX = point.X % Chunk<TItem>.XLength;
            var elementZ = point.Z % Chunk<TItem>.ZLength;
            var chunkX = point.X / Chunk<TItem>.XLength;
            var chunkZ = point.Z / Chunk<TItem>.ZLength;
            return (PointI.CreateXZ(chunkX, chunkZ), new PointI(elementX, point.Y, elementZ));
        }

        public abstract TItem GetItem(PointI position);

        public abstract bool TrySetItem(PointI position, TItem value);

        public bool IsChunkInBounds(PointI point)
        {
            return point.X >= Offset.X && point.X < Offset.X + Size &&
                   point.Z >= Offset.Z && point.Z < Offset.Z + Size;
        }

        public IReadOnlyList<PointI> GetNeedlessChunks()
        {
            return chunks.Keys.Where(k => !IsChunkInBounds(k)).ToList();
        }

        public bool TryDeleteChunk(PointI position)
        {
            return chunks.Remove(position);
        }

        public IReadOnlyList<PointI> GetPointOfGaps()
        {
            var result = new List<PointI>();
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    var point = Offset.Add(PointI.CreateXZ(i, j));
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
        

        public TContainer this[PointI position]
        {
            get => chunks.ContainsKey(position) ? chunks[position] : null;
            set => chunks[position] = value;
        }
    }
}