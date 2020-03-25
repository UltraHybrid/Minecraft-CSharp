using System.Collections.Generic;
using OpenTK.Platform.Windows;

namespace tmp
{
    public interface IMover
    {
        Vector Position { get; }
        Vector Front { get; }
        void Move(List<Direction> directions, float time);
        void Rotate(float deltaYaw, float deltaPitch);
    }
}