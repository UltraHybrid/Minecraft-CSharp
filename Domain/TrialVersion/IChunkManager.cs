namespace tmp
{
    public interface IChunkManager
    {
        Chunk<Block> Load(int x, int z);
        Chunk<Block> Create(int x, int z);
        void Save(Chunk<Block> chunk);
    }
}