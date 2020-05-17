using System.Collections.Generic;

namespace tmp.Interfaces
{
    public interface IChunk<T> : IEnumerable<T>
    {
        PointI Position { get; }
        T this[PointB position] { get; set; }
    }
}