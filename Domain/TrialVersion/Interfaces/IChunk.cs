using System.Collections.Generic;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public interface IChunk<T> : IEnumerable<T>
    {
        PointI Position { get; }
        T this[PointB position] { get; set; }
    }
}