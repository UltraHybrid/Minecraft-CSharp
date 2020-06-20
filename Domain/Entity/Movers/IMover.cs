using System.Collections.Generic;
using System.Numerics;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public interface IMover
    {
        PointF Position { get; }
        Vector3 Front { get; }
        Vector3 Left { get; }
        Vector3 Up { get; }
        void Move(Piece piece, IEnumerable<Direction> directions, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}