using System.Collections.Generic;

namespace tmp.Interfaces
{
    public interface IWorld<TContainer, TItem>
    {
        PointI Offset { get; set; }
        int Size { get; }
        int Count { get; }
        TItem GetItem(PointI position);
        bool TrySetItem(PointI position, TItem value);
        IReadOnlyList<PointI> GetNeedlessChunks();
        bool TryDeleteChunk(PointI position);
        IReadOnlyList<PointI> GetPointOfGaps();
        TContainer this[PointI position] { get; set; }
    }
}