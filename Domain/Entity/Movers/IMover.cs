﻿using System.Collections.Generic;
using System.Numerics;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public interface IMover
    {
        PointF Position { get; }
        Vector3 Front { get; }
        void Move(Piece piece, IReadOnlyList<Direction> directions, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}