namespace tmp.Interfaces
{
    public interface IChunkManager<T>
    {
        PointI MakeFirstLunch();
        void Update();
        void MakeShift(PointI offset, PointI playerPosition);
    }
}