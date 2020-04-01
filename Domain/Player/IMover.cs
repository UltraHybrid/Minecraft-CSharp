using System.Collections.Generic;

namespace tmp
{
    public interface IMover
    {
        Vector Position { get; }
        Vector Front { get; }
        void Move(Direction direction, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}