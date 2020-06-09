using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public interface IChunkManager<T>
    {
        PointI MakeFirstLunch();
        void Update();
        void MakeShift(PointI offset, PointI playerPosition);
    }
}