using System.Collections.Generic;
using tmp.Domain.TrialVersion;

namespace tmp.Domain
{
    public interface IMover
    {
        Vector Position { get; }
        Vector Front { get; }
        Vector Left { get; }
        Vector Up { get; }
        void Move(Piece piece, IEnumerable<Direction> directions, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}