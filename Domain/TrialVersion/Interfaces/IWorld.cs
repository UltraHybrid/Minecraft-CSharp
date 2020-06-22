using System.Collections.Generic;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public interface IWorld<TContainer, TItem>
    {
        PointI Offset { get; set; }
        int Size { get; }
        int Count { get; }
        TItem GetItem(PointL position);
        bool TrySetItem(PointL position, TItem value);
        IReadOnlyList<PointI> GetNeedlessChunks();
        bool TryDeleteChunk(PointI position);
        IReadOnlyList<PointI> GetPointOfGaps();
        TContainer this[PointI position] { get; set; }
    }
}