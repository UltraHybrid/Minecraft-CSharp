using System.Collections.Generic;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    public interface IChunk<T> : IEnumerable<T>
    {
        PointI Position { get; }
        T this[PointI position] { get; set; }
    }
}