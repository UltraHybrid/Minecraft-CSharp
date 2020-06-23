using System.Collections.Generic;
using System.Numerics;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    public interface IMover
    {
        PointF Position { get; }
        Vector3 Front { get; }
        void Move(Piece piece, IReadOnlyList<Direction> directions, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}