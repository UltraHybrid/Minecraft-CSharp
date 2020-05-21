using System.Collections.Generic;

namespace tmp
{
    public interface IMover
    {
        Vector Position { get; }
        Vector Front { get; }
        Vector Left { get; }
        Vector Up { get; }
        void Move(Piece piece, Direction direction, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}