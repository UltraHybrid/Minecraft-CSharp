namespace tmp
{
    public interface IGenerator
    {
        Chunk Generate(int x, int z);
        Point3 GetHigh(Point3 point);
    }
}