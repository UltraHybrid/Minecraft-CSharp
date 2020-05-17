using System.Collections.Generic;

namespace tmp.Interfaces
{
    public interface IWorld<T>
    {
        PointI Offset { get; set; }
        int Size { get; }
        int Count { get; }
        T GetItem(PointI position);
        bool TrySetItem(PointI position, T value);
        IReadOnlyList<PointI> GetNeedlessChunks();
        bool TryDeleteChunk(PointI position);
        IReadOnlyList<PointI> GetPointOfGaps();
        Chunk<T> this[PointI position] { get; set; }
    }
}