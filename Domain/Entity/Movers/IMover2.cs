using System.Collections.Generic;
using System.Numerics;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public interface IMover2
    {
        PointF Position { get; }
        Vector3 Front { get; }
        void Move(Piece piece, IEnumerable<Direction> directions, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}