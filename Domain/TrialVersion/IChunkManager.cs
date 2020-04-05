namespace tmp
{
    public interface IChunkManager
    {
        Chunk Load(int x, int z);
        Chunk Create(int x, int z);
        void Save(Chunk chunk);
    }
}